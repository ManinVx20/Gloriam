using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StormDreams
{
    public class CharacterAttackRange : MonoBehaviour
    {
        [SerializeField]
        private Character character;

        private List<Character> targetCharacterList = new List<Character>();

        private void Update()
        {
            if (targetCharacterList.Count > 0)
            {
                character.EnableAttack();
            }
            else
            {
                character.DisableAttack();
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<Character>(out Character character) && character != this.character)
            {
                targetCharacterList.Add(character);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent<Character>(out Character character) && character != this.character)
            {
                targetCharacterList.Remove(character);
            }
        }
    }
}
