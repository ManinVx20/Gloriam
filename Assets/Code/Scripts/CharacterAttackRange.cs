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
            if (character.IsDead())
            {
                return;
            }

            targetCharacterList.RemoveAll(character => character.IsDead());

            if (targetCharacterList.Count > 0)
            {
                character.EnableAttack(targetCharacterList[0]);
            }
            else
            {
                character.DisableAttack();
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (this.character.IsDead())
            {
                return;
            }

            if (other.TryGetComponent<Character>(out Character character) && character != this.character && !character.IsDead())
            {
                targetCharacterList.Add(character);
            }

            if (other.TryGetComponent<Obstacle>(out Obstacle obstacle) && this.character is Player)
            {
                obstacle.SetTrasparentMaterial();
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (this.character.IsDead())
            {
                return;
            }

            if (other.TryGetComponent<Character>(out Character character) && targetCharacterList.Contains(character))
            {
                targetCharacterList.Remove(character);
            }

            if (other.TryGetComponent<Obstacle>(out Obstacle obstacle) && this.character is Player)
            {
                obstacle.SetNormalMaterial();
            }
        }
    }
}
