using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StormDreams
{
    public class Weapon : MonoBehaviour
    {
        [SerializeField]
        private WeaponSO _weaponSO;
        [SerializeField]
        private BulletPool _bulletPool;
        [SerializeField]
        private Transform _firePointTransform;

        private void OnDestroy()
        {
            _bulletPool.Dispose();
        }

        public WeaponSO GetWeaponSO()
        {
            return _weaponSO;
        }

        public void Fire(Character character)
        {
            Bullet bullet = _bulletPool.GetPrefabInstance();
            bullet.transform.SetPositionAndRotation(_firePointTransform.position, _firePointTransform.rotation);
            bullet.Initialize(character, _weaponSO.Damage);
        }

        public void Despawn()
        {
            //_bulletPool.Dispose();

            Destroy(gameObject);
        }
    }
}
