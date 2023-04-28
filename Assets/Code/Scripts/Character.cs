using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

namespace StormDreams
{
    public class Character : PoolableObject
    {
        public class OnCharacterHealthChangedArgs : EventArgs
        {
            public float HealthPercentage;
        }
        public event EventHandler<OnCharacterHealthChangedArgs> OnCharacterHealthChanged;

        [Header("Components")]
        [SerializeField]
        private CharacterAnimator characterAnimator;
        [SerializeField]
        private Transform spawnBulletPoint;
        [SerializeField]
        private Weapon weapon;

        [Header("Properties")]
        [SerializeField]
        private float moveSpeed = 4.0f;
        [SerializeField]
        private float rotateSpeed = 20.0f;
        [SerializeField]
        private float healthMax = 100.0f;

        private NavMeshAgent navMeshAgent;
        private CharacterInfoUI characterInfoUI;

        private Vector2 moveInput;
        private Vector2 lookInput;
        private Vector3 moveDirection;

        private float health;
        private float attackSingleTimer;
        private float attackSingleTimerMax = 0.5f;
        private float attackAutoTimer;
        private float attackAutoTimerMax = 0.1f;
        private bool isAttacking;
        private bool isAttackingAuto;
        private Character firstTargetCharacterInRange;

        private void Awake()
        {
            Initialize();
        }

        private void Start()
        {
            Spawn();
        }

        private void Update()
        {
            Execute();
        }

        public virtual void Initialize()
        {
            navMeshAgent = GetComponent<NavMeshAgent>();
        }

        public virtual void Spawn()
        {
            characterInfoUI = ResourceManager.Instance.CharacterInfoUIPool.GetPrefabInstance();
            characterInfoUI.transform.SetParent(UIManager.Instance.GetUICanvas<GameplayCanvas>().transform, false);
            characterInfoUI.Initialize(this);

            health = healthMax;

            OnCharacterHealthChanged?.Invoke(this, new OnCharacterHealthChangedArgs
            {
                HealthPercentage = health / healthMax
            });

            DisableAttack();
        }

        public virtual void Despawn()
        {
            characterInfoUI.Despawn();
            characterInfoUI = null;
        }

        public virtual void Execute()
        {
            if (IsDead())
            {
                return;
            }

            HandleInput(out moveInput, out lookInput);
            HandleMovement();
            HandleRotation();
            HandleMovementAnimation();
            HandleAttack();
        }

        public bool IsDead()
        {
            return health <= 0.0f;
        }

        public bool IsAttacking()
        {
            return isAttacking;
        }

        public Character FirstTargetCharacterInRange()
        {
            return firstTargetCharacterInRange;
        }

        public void EnableAttack(Character character)
        {
            if (isAttacking)
            {
                return;
            }

            isAttacking = true;

            firstTargetCharacterInRange = character;

            HandleAttackAnimation();
        }

        public void DisableAttack()
        {
            if (!isAttacking)
            {
                return;
            }

            isAttacking = false;

            firstTargetCharacterInRange = null;

            HandleAttackAnimation();
        }

        public void ToggleAttackAuto()
        {
            isAttackingAuto = !isAttackingAuto;

            HandleAttackAnimation();
        }

        public void GetHit(Character character, float damage)
        {
            if (IsDead())
            {
                return;
            }

            health -= damage;
            if (health < 0.0f)
            {
                health = 0.0f;
            }

            OnCharacterHealthChanged?.Invoke(this, new OnCharacterHealthChangedArgs
            {
                HealthPercentage = health / healthMax
            });

            if (!IsDead())
            {
                characterAnimator.ChangeBaseState(CharacterAnimator.BaseState.GetHit);
            }
            else
            {
                HandleDeath();
            }
        }

        public void SetPosition(Vector3 position)
        {
            navMeshAgent.Warp(position);
        }

        protected virtual void HandleDeath()
        {
            DisableAttack();

            characterAnimator.ChangeBaseState(CharacterAnimator.BaseState.Die);

            Invoke(nameof(Despawn), 3.0f);
        }

        protected virtual void HandleInput(out Vector2 moveInput, out Vector2 lookInput)
        {
            moveInput = Vector2.zero;
            lookInput = Vector2.zero;
        }

        private void HandleMovement()
        {
            moveDirection = new Vector3(moveInput.x, 0.0f, moveInput.y);

            navMeshAgent.Move(moveDirection * moveSpeed * Time.deltaTime);
        }

        private void HandleRotation()
        {
            if (lookInput != Vector2.zero)
            {
                Vector3 lookDirection = new Vector3(lookInput.x, 0.0f, lookInput.y);
                Quaternion targetRotation = Quaternion.LookRotation(lookDirection);

                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);
            }
        }

        private void HandleMovementAnimation()
        {
            if (moveDirection == Vector3.zero)
            {
                characterAnimator.ChangeBaseState(CharacterAnimator.BaseState.Idle);
            }
            else
            {
                characterAnimator.ChangeBaseState(CharacterAnimator.BaseState.Run);
            }

            Vector3 animationMoveDirection = Quaternion.Euler(0.0f, -transform.eulerAngles.y, 0.0f) * moveDirection;

            characterAnimator.SetHorizontalVertical(animationMoveDirection.x, animationMoveDirection.z);
        }

        private void HandleAttack()
        {
            if (!isAttacking)
            {
                attackSingleTimer = attackSingleTimerMax;
                attackAutoTimer = attackAutoTimerMax;
            }
            else
            {
                if (!isAttackingAuto)
                {
                    attackSingleTimer += Time.deltaTime;
                    if (attackSingleTimer >= attackSingleTimerMax)
                    {
                        attackSingleTimer = 0.0f;

                        SpawnBullet();
                    }

                    attackAutoTimer = attackAutoTimerMax;
                }
                else
                {
                    attackAutoTimer += Time.deltaTime;
                    if (attackAutoTimer >= attackAutoTimerMax)
                    {
                        attackAutoTimer = 0.0f;

                        SpawnBullet();
                    }

                    attackSingleTimer = attackSingleTimerMax;
                }
            }
        }

        private void HandleAttackAnimation()
        {
            if (!isAttacking)
            {
                characterAnimator.SetAttackState(CharacterAnimator.AttackState.StopAttack);
            }
            else
            {
                if (!isAttackingAuto)
                {
                    characterAnimator.SetAttackState(CharacterAnimator.AttackState.AttackSingle);
                }
                else
                {
                    characterAnimator.SetAttackState(CharacterAnimator.AttackState.AttackAuto);
                }
            }
        }

        private void SpawnBullet()
        {
            weapon.SpawnBullet(spawnBulletPoint.position, spawnBulletPoint.rotation);
        }
    }
}
