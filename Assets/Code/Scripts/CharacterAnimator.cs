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
            AttackSingle = 1,
            AttackAuto = 2,
        }

        private Animator animator;
        private BaseState baseState;

        private int horizontalHash;
        private int verticalHash;

        private void Awake()
        {
            animator = GetComponent<Animator>();

            horizontalHash = Animator.StringToHash("Horizontal");
            verticalHash = Animator.StringToHash("Vertical");
        }

        public void ChangeBaseState(BaseState baseState)
        {
            if (baseState == this.baseState)
            {
                return;
            }

            this.baseState = baseState;

            animator.SetTrigger(this.baseState.ToString());
        }

        public void SetAttackState(AttackState attackState)
        {
            animator.SetTrigger(attackState.ToString());
        }

        public void SetHorizontalVertical(float horizontal, float vertical)
        {
            float dampTime = 0.2f;

            animator.SetFloat(horizontalHash, horizontal, dampTime, Time.deltaTime);
            animator.SetFloat(verticalHash, vertical, dampTime, Time.deltaTime);
        }
    }
}
