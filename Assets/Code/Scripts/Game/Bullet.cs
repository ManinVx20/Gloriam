using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StormDreams
{
    public class Bullet : PoolableObject
    {
        private Rigidbody _rb;
        private Character _character;

        private float _survivalTimer;
        private float _survivalTimerMax = 3.0f;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
        }

        private void Update()
        {
            _survivalTimer += Time.deltaTime;
            if (_survivalTimer >= _survivalTimerMax)
            {
                Despawn();
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<Character>(out Character character))
            {
                if (character != _character)
                {
                    character.GetHit(_character, 10.0f);

                    Despawn();
                }
            }

            if (other.TryGetComponent<Obstacle>(out _))
            {
                Despawn();
            }
        }

        public void Initialize(Character character, Weapon weapon)
        {
            _character = character;

            _rb.velocity = transform.forward * 10.0f;

            _survivalTimer = 0.0f;
        }

        private void Despawn()
        {
            ReturnToPool();
        }
    }
}
