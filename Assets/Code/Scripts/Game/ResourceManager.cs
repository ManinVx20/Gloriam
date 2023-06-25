using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StormDreams
{
    public class ResourceManager : MonoSingleton<ResourceManager>
    {
        [System.Serializable]
        private struct WeaponAnimData
        {
            public Weapon.Type Type;
            public AnimatorOverrideController CharAnimController;
        }

        [field: SerializeField]
        public CharacterInfoUIPool CharacterInfoUIPool { get; private set; }
        [field: SerializeField]
        public Match MatchPrefab { get; private set; }

        [SerializeField]
        private WeaponAnimData[] _weaponAnimDataArray;

        private Dictionary<Weapon.Type, AnimatorOverrideController> _weaponAnimDataDict;
        private Match _match;

        private void Awake()
        {
            _weaponAnimDataDict = new Dictionary<Weapon.Type, AnimatorOverrideController>();

            for (int i = 0; i < _weaponAnimDataArray.Length; i++)
            {
                _weaponAnimDataDict[_weaponAnimDataArray[i].Type] = _weaponAnimDataArray[i].CharAnimController;
            }
        }

        public AnimatorOverrideController GetCharAnimControllerByWeaponType(Weapon.Type type)
        {
            return _weaponAnimDataDict[type];
        }

        public void CreateMatch()
        {
            if (_match != null)
            {
                return;
            }

            _match = Instantiate(MatchPrefab);
        }

        public void RemoveMatch()
        {
            if (_match == null)
            {
                return;
            }

            _match.Despawn();
        }
    }
}
