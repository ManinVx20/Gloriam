using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StormDreams
{
    public class CharacterAnimator : MonoBehaviour
    {
        public enum MoveState
        {
            Idle = 0,
            Run = 1,
        }

        public enum AttackState
        {
            StopAttack = 0,
            AttackSingle = 1,
            AttackAuto = 2,
        }

        private Animator animator;
        private MoveState moveState;

        private int horizontalHash;
        private int verticalHash;

        private void Awake()
        {
            animator = GetComponent<Animator>();

            horizontalHash = Animator.StringToHash("Horizontal");
            verticalHash = Animator.StringToHash("Vertical");
        }

        public void ChangeMoveState(MoveState moveState)
        {
            if (moveState == this.moveState)
            {
                return;
            }

            this.moveState = moveState;

            animator.SetTrigger(this.moveState.ToString());
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
