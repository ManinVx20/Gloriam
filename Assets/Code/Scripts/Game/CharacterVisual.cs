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
            public Weapon.Type Type;
            public Transform Transform;
        }

        [SerializeField]
        private WeaponHolderData[] _weaponHolderDataArray;

        private Dictionary<Weapon.Type, Transform> _weaponHolderDataDict;

        private void Awake()
        {
            _weaponHolderDataDict = new Dictionary<Weapon.Type, Transform>();

            for (int i = 0; i  < _weaponHolderDataArray.Length; i++)
            {
                _weaponHolderDataDict[_weaponHolderDataArray[i].Type] = _weaponHolderDataArray[i].Transform;
            }
        }

        public void SetWeapon(Weapon weapon)
        {
            foreach (Transform transform in _weaponHolderDataDict.Values)
            {
                transform.ClearChildren();
            }

            weapon.transform.SetParent(_weaponHolderDataDict[weapon.GetWeaponType()], false);
        }
    }
}
