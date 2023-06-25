using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StormDreams
{
    public class Weapon : MonoBehaviour
    {
        public enum Type
        {
            None = 0,
            Pistol = 1,
            AssaultRifle = 2,
        }

        [SerializeField]
        private Type _type;
        [SerializeField]
        private BulletPool _bulletPool;
        [SerializeField]
        private Transform _firePointTransform;

        private void OnDestroy()
        {
            _bulletPool.Dispose();
        }

        public Type GetWeaponType()
        {
            return _type;
        }

        public void Fire(Character character)
        {
            Bullet bullet = _bulletPool.GetPrefabInstance();
            bullet.transform.SetPositionAndRotation(_firePointTransform.position, _firePointTransform.rotation);
            bullet.Initialize(character, this);
        }

        public void Despawn()
        {
            _bulletPool.Dispose();

            Destroy(gameObject);
        }
    }
}
