using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StormDreams
{
    public class CharacterAttackRange : MonoBehaviour
    {
        [SerializeField]
        private Character _character;

        private List<Character> _targetCharacterList = new List<Character>();

        private void Update()
        {
            if (_character.IsDead() || (!GameManager.Instance.IsGamePlaying() && !GameManager.Instance.IsGameOver()))
            {
                return;
            }

            _targetCharacterList.RemoveAll(character => character == null || character.IsDead());

            if (_targetCharacterList.Count > 0)
            {
                _character.SetTargetCharacterInRange(_targetCharacterList[0]);
                _character.EnableAttack();
            }
            else
            {
                _character.SetTargetCharacterInRange(null);
                _character.DisableAttack();
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (_character.IsDead())
            {
                return;
            }

            if (other.TryGetComponent<Character>(out Character character) && character != _character && !character.IsDead())
            {
                _targetCharacterList.Add(character);
            }

            if (other.TryGetComponent<Obstacle>(out Obstacle obstacle) && _character is Player)
            {
                obstacle.SetTrasparentMaterial();
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (_character.IsDead())
            {
                return;
            }

            if (other.TryGetComponent<Character>(out Character character) && _targetCharacterList.Contains(character))
            {
                _targetCharacterList.Remove(character);
            }

            if (other.TryGetComponent<Obstacle>(out Obstacle obstacle) && _character is Player)
            {
                obstacle.SetNormalMaterial();
            }
        }
    }
}
