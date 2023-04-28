using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StormDreams
{
    public class ResourceManager : MonoSingleton<ResourceManager>
    {
        [field: SerializeField]
        public CharacterInfoUIPool CharacterInfoUIPool { get; private set; }
    }
}
