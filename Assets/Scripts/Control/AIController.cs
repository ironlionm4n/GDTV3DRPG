using RPG.Combat;
using RPG.Core;
using UnityEngine;

namespace RPG.Control
{
    public class AIController : MonoBehaviour
    {
        #region Variables

        [SerializeField] private float chaseDistance = 5f;
        private Health _health;
        private GameObject _player;
        private Fighter _fighter;
        
        #endregion

        #region UnityFunctions

        private void Start()
        {
            _player = GameObject.FindGameObjectWithTag("Player");
            _fighter = GetComponent<Fighter>();
            _health = GetComponent<Health>();
        }

        private void Update()
        {
            if (_health.HasDied) return;
            
            if (InAttackRange() && _fighter.CanAttack(_player))
            {
                _fighter.Attack(_player);
            }
            else
            {
                _fighter.Cancel();
            }
        }

        private bool InAttackRange()
        {
            return DistanceToPlayer() <= chaseDistance;
        }

        private float DistanceToPlayer()
        {
            return Vector3.Distance(transform.position, _player.transform.position);
        }

        #endregion
    }
}