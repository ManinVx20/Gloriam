using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StormDreams
{
    public class CharacterVisual : MonoBehaviour
    {
        [System.Serializable]
        private struct WeaponHolderData
        {
            public WeaponType WeaponType;
            public Transform Transform;
        }

        [SerializeField]
        private WeaponHolderData[] _weaponHolderDataArray;

        private Dictionary<WeaponType, Transform> _weaponHolderDataDict;

        private void Awake()
        {
            _weaponHolderDataDict = new Dictionary<WeaponType, Transform>();

            for (int i = 0; i  < _weaponHolderDataArray.Length; i++)
            {
                _weaponHolderDataDict[_weaponHolderDataArray[i].WeaponType] = _weaponHolderDataArray[i].Transform;
            }
        }

        public void SetWeapon(Weapon weapon, WeaponType weaponType)
        {
            foreach (Transform transform in _weaponHolderDataDict.Values)
            {
                transform.ClearChildren();
            }

            weapon.transform.SetParent(_weaponHolderDataDict[weaponType], false);
        }
    }
}
