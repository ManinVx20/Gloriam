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
        public class OnCharacterDeadArgs : EventArgs
        {
            public Character Character;
        }
        public static event EventHandler<OnCharacterDeadArgs> OnAnyCharacterDead;

        public class OnCharacterHealthChangedArgs : EventArgs
        {
            public float HealthPercentage;
        }
        public event EventHandler<OnCharacterHealthChangedArgs> OnCharacterHealthChanged;
        public class OnCharacterEnergyChangedArgs : EventArgs
        {
            public float EnergyPercentage;
        }
        public event EventHandler<OnCharacterEnergyChangedArgs> OnCharacterEnergyChanged;

        [Header("Components")]
        [SerializeField]
        private CharacterAnimator _characterAnimator;
        [SerializeField]
        private CharacterVisual _characterVisual;
        [SerializeField]
        private Weapon _weaponPrefab;

        [Header("Properties")]
        [SerializeField]
        private float _moveSpeed = 4.0f;
        [SerializeField]
        private float _rotateSpeed = 20.0f;
        [SerializeField]
        private float _healthMax = 100.0f;
        [SerializeField]
        private float _energyMax = 100.0f;

        private NavMeshAgent _navMeshAgent;
        private CapsuleCollider _capsuleCollider;
        private Weapon _weapon;
        private CharacterInfoUI _characterInfoUI;

        private Vector2 _moveInput;
        private Vector2 _lookInput;
        private Vector3 _moveDirection;

        private float _health;
        private float _energy;
        private float _attackSingleTimer;
        private float _attackSingleTimerMax = 0.6f;
        private float _attackAutoTimer;
        private float _attackAutoTimerMax = 0.2f;
        private bool _isAttacking;
        private bool _isAttackingAuto;

        private Character _targetCharacterInRange;
        private Coroutine _loseHealthOverTimeCoroutine;
        private Coroutine _restoreEnergyOverTimeCoroutine;

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

        private void OnDestroy()
        {
            if (_characterInfoUI != null)
            {
                _characterInfoUI.Dispose();
            }
        }

        public virtual void Initialize()
        {
            _navMeshAgent = GetComponent<NavMeshAgent>();
            _capsuleCollider = GetComponent<CapsuleCollider>();
        }

        public virtual void Spawn()
        {
            _navMeshAgent.enabled = true;
            _capsuleCollider.enabled = true;

            _moveInput = Vector2.zero;
            _lookInput = Vector2.zero;

            _characterAnimator.ChangeBaseState(CharacterAnimator.BaseState.Idle);

            if (_weapon == null)
            {
                _weapon = Instantiate(_weaponPrefab);
                _characterVisual.SetWeapon(_weapon);
            }

            _characterAnimator.SetAnimController(ResourceManager.Instance.GetCharAnimControllerByWeaponType(_weapon.GetWeaponType()));

            DisableAttack();

            if (_characterInfoUI == null)
            {
                _characterInfoUI = ResourceManager.Instance.CharacterInfoUIPool.GetPrefabInstance();
                _characterInfoUI.transform.SetParent(UIManager.Instance.GetUICanvas<GameplayCanvas>().transform, false);
                _characterInfoUI.Initialize(this);
            }

            ChangeHealth(_healthMax);
            ChangeEnergy(_energyMax);

            if (_loseHealthOverTimeCoroutine != null)
            {
                StopCoroutine(_loseHealthOverTimeCoroutine);

                _loseHealthOverTimeCoroutine = null;
            }

            if (_restoreEnergyOverTimeCoroutine == null)
            {
                _restoreEnergyOverTimeCoroutine = StartCoroutine(RestoreEnergyOverTimeCoroutine());
            }
        }

        public virtual void Despawn()
        {
            if (_characterInfoUI != null)
            {
                _characterInfoUI.Despawn();
                _characterInfoUI = null;
            }
        }

        public virtual void Execute()
        {
            if (IsDead())
            {
                return;
            }

            HandleInput(out _moveInput, out _lookInput);
            HandleMovement();
            HandleRotation();
            HandleMovementAnimation();
            HandleAttack();
        }

        public bool IsDead()
        {
            return _health <= 0.0f;
        }

        public bool IsAttacking()
        {
            return _isAttacking;
        }

        public Character TargetCharacterInRange()
        {
            return _targetCharacterInRange;
        }

        public void SetTargetCharacterInRange(Character character)
        {
            _targetCharacterInRange = character;
        }

        public void EnableAttack()
        {
            if (_isAttacking)
            {
                return;
            }

            _isAttacking = true;

            HandleAttackAnimation();
        }

        public void DisableAttack()
        {
            if (!_isAttacking)
            {
                return;
            }

            _isAttacking = false;

            _targetCharacterInRange = null;

            HandleAttackAnimation();
        }

        public void ToggleAttackAuto()
        {
            _isAttackingAuto = !_isAttackingAuto;

            HandleAttackAnimation();
        }

        public void GetHit(Character character, float damage)
        {
            if (IsDead())
            {
                return;
            }

            ChangeHealth(-damage);

            if (!IsDead())
            {
                _characterAnimator.ChangeBaseState(CharacterAnimator.BaseState.GetHit);
            }
            else
            {
                HandleDeath(character);
            }
        }

        public void GainKill()
        {
            if (IsDead())
            {
                return;
            }

            ChangeHealth(10.0f);
        }

        public void EnableLoseHealthOverTime()
        {
            if (_loseHealthOverTimeCoroutine == null)
            {
                _loseHealthOverTimeCoroutine = StartCoroutine(LoseHealthOverTimeCoroutine());
            }
        }

        public void DisableLoseHealthOverTime()
        {
            if (_loseHealthOverTimeCoroutine != null)
            {
                StopCoroutine(_loseHealthOverTimeCoroutine);

                _loseHealthOverTimeCoroutine = null;
            }
        }

        public void ChangeHealth(float amount)
        {
            _health = Mathf.Clamp(_health + amount, 0.0f, _healthMax);

            OnCharacterHealthChanged?.Invoke(this, new OnCharacterHealthChangedArgs
            {
                HealthPercentage = _health / _healthMax
            });
        }

        public void ChangeEnergy(float amount)
        {
            _energy = Mathf.Clamp(_energy + amount, 0.0f, _energyMax);

            OnCharacterEnergyChanged?.Invoke(this, new OnCharacterEnergyChangedArgs
            {
                EnergyPercentage = _energy / _energyMax
            });
        }

        public void SetPosition(Vector3 position)
        {
            if (_navMeshAgent.enabled)
            {
                _navMeshAgent.Warp(position);
            }
        }

        protected virtual void HandleDeath(Character character)
        {
            _navMeshAgent.enabled = false;
            _capsuleCollider.enabled = false;

            DisableAttack();

            _characterAnimator.ChangeBaseState(CharacterAnimator.BaseState.Die);

            OnAnyCharacterDead?.Invoke(this, new OnCharacterDeadArgs
            {
                Character = character
            });

            Invoke(nameof(Despawn), 3.0f);
        }

        protected virtual void HandleInput(out Vector2 moveInput, out Vector2 lookInput)
        {
            moveInput = Vector2.zero;
            lookInput = Vector2.zero;
        }

        private void HandleMovement()
        {
            _moveDirection = new Vector3(_moveInput.x, 0.0f, _moveInput.y);

            if (_navMeshAgent.enabled)
            {
                _navMeshAgent.Move(_moveDirection * _moveSpeed * Time.deltaTime);
            }
        }

        private void HandleRotation()
        {
            if (_lookInput != Vector2.zero)
            {
                Vector3 lookDirection = new Vector3(_lookInput.x, 0.0f, _lookInput.y);
                Quaternion targetRotation = Quaternion.LookRotation(lookDirection);

                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, _rotateSpeed * Time.deltaTime);
            }
        }

        private void HandleMovementAnimation()
        {
            if (_moveDirection == Vector3.zero)
            {
                _characterAnimator.ChangeBaseState(CharacterAnimator.BaseState.Idle);
            }
            else
            {
                _characterAnimator.ChangeBaseState(CharacterAnimator.BaseState.Run);
            }

            Vector3 animationMoveDirection = Quaternion.Euler(0.0f, -transform.eulerAngles.y, 0.0f) * _moveDirection;

            _characterAnimator.SetHorizontalVertical(animationMoveDirection.x, animationMoveDirection.z);
        }

        private void HandleAttack()
        {
            if (!_isAttacking)
            {
                _attackSingleTimer = _attackSingleTimerMax;
                _attackAutoTimer = _attackAutoTimerMax;
            }
            else
            {
                if (_energy <= 0.0f)
                {
                    return;
                }
                else if (!_isAttackingAuto)
                {
                    _attackSingleTimer += Time.deltaTime;
                    if (_attackSingleTimer >= _attackSingleTimerMax)
                    {
                        _attackSingleTimer = 0.0f;

                        Attack();
                    }

                    _attackAutoTimer = _attackAutoTimerMax;
                }
                else
                {
                    _attackAutoTimer += Time.deltaTime;
                    if (_attackAutoTimer >= _attackAutoTimerMax)
                    {
                        _attackAutoTimer = 0.0f;

                        Attack();
                    }

                    _attackSingleTimer = _attackSingleTimerMax;
                }
            }
        }

        private void HandleAttackAnimation()
        {
            if (!_isAttacking)
            {
                _characterAnimator.SetAttackState(CharacterAnimator.AttackState.StopAttack);
            }
            else
            {
                if (_energy <= 0.0f)
                {
                    _characterAnimator.SetAttackState(CharacterAnimator.AttackState.StopAttack);
                }
                else if (!_isAttackingAuto)
                {
                    _characterAnimator.SetAttackState(CharacterAnimator.AttackState.AttackSingle);
                }
                else
                {
                    _characterAnimator.SetAttackState(CharacterAnimator.AttackState.AttackAuto);
                }
            }
        }

        private void Attack()
        {
            _weapon.Fire(this);

            ChangeEnergy(-1.0f);
        }

        private IEnumerator LoseHealthOverTimeCoroutine()
        {
            while (!IsDead())
            {
                yield return new WaitForSeconds(3.0f);

                GetHit(null, 10.0f);
            }

            _loseHealthOverTimeCoroutine = null;
        }

        private IEnumerator RestoreEnergyOverTimeCoroutine()
        {
            while (!IsDead())
            {
                yield return new WaitForSeconds(2.0f);

                ChangeEnergy(1.0f);
            }

            _loseHealthOverTimeCoroutine = null;
        }
    }
}
