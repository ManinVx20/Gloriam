using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace StormDreams
{
    public class Character : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField]
        private CharacterAnimator characterAnimator;
        [SerializeField]
        private GameObject bulletPrefab;
        [SerializeField]
        private Transform spawnBulletPoint;

        [Header("Properties")]
        [SerializeField]
        private float moveSpeed = 4.0f;
        [SerializeField]
        private float rotateSpeed = 20.0f;

        private NavMeshAgent navMeshAgent;

        private Vector2 moveInput;
        private Vector2 lookInput;
        private Vector3 moveDirection;

        private float attackSingleTimer;
        private float attackSingleTimerMax = 0.5f;
        private float attackAutoTimer;
        private float attackAutoTimerMax = 0.1f;
        private bool isAttacking;
        private bool isAttackingAuto;

        protected virtual void Awake()
        {
            navMeshAgent = GetComponent<NavMeshAgent>();
        }

        protected virtual void Start()
        {
            DisableAttack();
        }

        protected virtual void Update()
        {
            HandleInput(out moveInput, out lookInput);
            HandleMovement();
            HandleRotation();
            HandleMoveAnimation();
            HandleAttack();
        }

        public void EnableAttack()
        {
            if (isAttacking)
            {
                return;
            }

            isAttacking = true;

            HandleAttackAnimation();
        }

        public void DisableAttack()
        {
            if (!isAttacking)
            {
                return;
            }

            isAttacking = false;

            HandleAttackAnimation();
        }

        public void ToggleAttackAuto()
        {
            isAttackingAuto = !isAttackingAuto;

            HandleAttackAnimation();
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

        private void HandleMoveAnimation()
        {
            if (moveDirection == Vector3.zero)
            {
                characterAnimator.ChangeMoveState(CharacterAnimator.MoveState.Idle);
            }
            else
            {
                characterAnimator.ChangeMoveState(CharacterAnimator.MoveState.Run);
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
            Bullet bullet = Instantiate(bulletPrefab, spawnBulletPoint.position, spawnBulletPoint.rotation).GetComponent<Bullet>();
            bullet.Initialize(this);
        }
    }
}
