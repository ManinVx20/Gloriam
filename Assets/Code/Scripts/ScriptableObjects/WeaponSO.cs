using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StormDreams
{
    [CreateAssetMenu(menuName = "Data/WeaponSO")]
    public class WeaponSO : ScriptableObject
    {
        [Header("Components")]
        public string Name;
        public WeaponType WeaponType;
        public Weapon WeaponPrefab;
        public AnimatorOverrideController CharAnimController;

        [Header("Properties")]
        public float EnergyCost;
        public float Damage;
        public float SingleFireRate;
        public float AutoFireRate;
    }

    public enum WeaponType
    {
        Pistol = 0,
        AssaultRifle = 1,
    }
}
