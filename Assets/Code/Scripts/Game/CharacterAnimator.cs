using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StormDreams
{
    public class CharacterAnimator : MonoBehaviour
    {
        public enum BaseState
        {
            Idle = 0,
            Run = 1,
            GetHit = 2,
            Die = 3,
        }

        public enum AttackState
        {
            StopAttack = 0,
            Attack = 1,
        }

        private Animator _animator;
        private BaseState _baseState;

        private int _horizontalHash;
        private int _verticalHash;

        private void Awake()
        {
            _animator = GetComponent<Animator>();

            _horizontalHash = Animator.StringToHash("Horizontal");
            _verticalHash = Animator.StringToHash("Vertical");
        }

        public void SetAnimController(AnimatorOverrideController animController)
        {
            _animator.runtimeAnimatorController = animController;
        }

        public void ChangeBaseState(BaseState baseState)
        {
            if (baseState == this._baseState)
            {
                return;
            }

            this._baseState = baseState;

            _animator.SetTrigger(this._baseState.ToString());
        }

        public void SetAttackState(AttackState attackState)
        {
            _animator.SetTrigger(attackState.ToString());
        }

        public void SetHorizontalVertical(float horizontal, float vertical)
        {
            float dampTime = 0.2f;

            _animator.SetFloat(_horizontalHash, horizontal, dampTime, Time.deltaTime);
            _animator.SetFloat(_verticalHash, vertical, dampTime, Time.deltaTime);
        }
    }
}
