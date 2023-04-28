using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StormDreams
{
    public class Bullet : PoolableObject
    {
        private Rigidbody rb;
        private Character character;

        private float survivalTimer;
        private float survivalTimerMax = 3.0f;

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
        }

        private void Update()
        {
            survivalTimer += Time.deltaTime;
            if (survivalTimer >= survivalTimerMax)
            {
                Despawn();
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<Character>(out Character character))
            {
                if (character != this.character)
                {
                    character.GetHit(this.character, 15.0f);

                    Despawn();
                }
            }
        }

        public void Initialize(Character character, Weapon weapon)
        {
            this.character = character;

            rb.velocity = transform.forward * 10.0f;

            survivalTimer = 0.0f;
        }

        private void Despawn()
        {
            ReturnToPool();
        }
    }
}
