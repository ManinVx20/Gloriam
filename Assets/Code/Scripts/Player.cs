using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

namespace StormDreams
{
    public class Player : MonoBehaviour
    {
        [SerializeField]
        private PlayerAnimator playerAnimator;
        [SerializeField]
        private float moveSpeed = 4.0f;
        [SerializeField]
        private float rotateSpeed = 20.0f;

        private NavMeshAgent navMeshAgent;
        private PlayerInput playerInput;

        private Vector2 moveInput;
        private Vector2 lookInput;
        private Vector3 moveDirection;

        private Coroutine attackSingleCoroutine;
        private bool isAttackAutoOn;
        private bool isAttackOn;

        private void Awake()
        {
            navMeshAgent = GetComponent<NavMeshAgent>();
            playerInput = GetComponent<PlayerInput>();
        }

        private void Start()
        {
            isAttackOn = false;
        }

        private void Update()
        {
            HandleInput();
            HandleMovement();
            HandleRotation();
            HandleAnimation();
        }
        
        public void ToggleAttackAuto()
        {
            if (!isAttackOn)
            {
                return;
            }

            if (!isAttackAutoOn)
            {
                isAttackAutoOn = true;

                if (attackSingleCoroutine != null)
                {
                    StopCoroutine(attackSingleCoroutine);
                }

                HandleAttackAuto();
            }
            else
            {
                isAttackAutoOn = false;

                attackSingleCoroutine = StartCoroutine(AttackSingleCoroutine(1.0f));
            }
        }

        public void ToggleAttack()
        {
            if (!isAttackOn)
            {
                isAttackOn = true;

                if (isAttackAutoOn)
                {
                    HandleAttackAuto();
                }
                else
                {
                    attackSingleCoroutine = StartCoroutine(AttackSingleCoroutine(1.0f));
                }
            }
            else
            {
                isAttackOn = false;

                if (attackSingleCoroutine != null)
                {
                    StopCoroutine(attackSingleCoroutine);
                }

                playerAnimator.SetAttackState(PlayerAnimator.AttackState.StopAttack);
            }
        }

        private void HandleInput()
        {
            moveInput = playerInput.GetMoveInput();
            lookInput = playerInput.GetLookInput();
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

        private void HandleAnimation()
        {
            if (moveDirection == Vector3.zero)
            {
                playerAnimator.ChangeMoveState(PlayerAnimator.MoveState.Idle);
            }
            else
            {
                playerAnimator.ChangeMoveState(PlayerAnimator.MoveState.Run);
            }

            Vector3 animationMoveDirection = Quaternion.Euler(0.0f, -transform.eulerAngles.y, 0.0f) * moveDirection;

            playerAnimator.SetHorizontalVertical(animationMoveDirection.x, animationMoveDirection.z);
        }

        private void HandleAttackSingle()
        {
            playerAnimator.SetAttackState(PlayerAnimator.AttackState.AttackSingle);
        }

        private void HandleAttackAuto()
        {
            playerAnimator.SetAttackState(PlayerAnimator.AttackState.AttackAuto);
        }

        private IEnumerator AttackSingleCoroutine(float delayAttackTime)
        {
            while (true)
            {
                HandleAttackSingle();

                yield return new WaitForSeconds(delayAttackTime);
            }
        }
    }
}
