using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StormDreams
{
    public class Weapon : MonoBehaviour
    {
        [SerializeField]
        private Character _character;

        private BulletPool _bulletPool;

        private void Awake()
        {
            _bulletPool = GetComponent<BulletPool>();
        }

        public void SpawnBullet(Vector3 position, Quaternion rotation)
        {
            Bullet bullet = _bulletPool.GetPrefabInstance();
            bullet.transform.SetPositionAndRotation(position, rotation);
            bullet.Initialize(_character, this);
        }
    }
}
