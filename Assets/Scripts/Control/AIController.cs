using System;
using RPG.Combat;
using RPG.Core;
using RPG.PlayerMovement;
using UnityEngine;

namespace RPG.Control
{
    public class AIController : MonoBehaviour
    {
        #region Variables

        [SerializeField] private float chaseDistance = 5f;
        [SerializeField] private float investigateTime = 3f;
        private Health _health;
        private GameObject _player;
        private Fighter _fighter;
        private Vector3 _guardPosition;
        private NavMeshAgentMover _navMeshAgentMover;
        private float _timeSinceLastSawPlayer;
        private ActionScheduler _actionScheduler;

        #endregion

        #region UnityFunctions

        private void Start()
        {
            _player = GameObject.FindGameObjectWithTag("Player");
            _fighter = GetComponent<Fighter>();
            _health = GetComponent<Health>();
            _guardPosition = transform.position;
            _navMeshAgentMover = GetComponent<NavMeshAgentMover>();
            _actionScheduler = GetComponent<ActionScheduler>();
            _timeSinceLastSawPlayer = Mathf.Infinity;
        }

        private void Update()
        {
            if (_health.HasDied) return;

            if (InAttackRange() && _fighter.CanAttack(_player))
            {
                _timeSinceLastSawPlayer = 0f;
                AttackBehaviour();
            }
            else if (_timeSinceLastSawPlayer < investigateTime)
            {
                InvestigateBehaviour();
            }
            else
            {
                ReturnToGuardBehaviour();
            }

            _timeSinceLastSawPlayer += Time.deltaTime;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, chaseDistance);
        }

        #endregion

        #region CustomFunctions

        private bool InAttackRange()
        {
            return DistanceToPlayer() <= chaseDistance;
        }

        private float DistanceToPlayer()
        {
            return Vector3.Distance(transform.position, _player.transform.position);
        }
        
        
        private void ReturnToGuardBehaviour()
        {
            _fighter.Cancel();
            _navMeshAgentMover.StartMoveAction(_guardPosition);
        }

        private void InvestigateBehaviour()
        {
            _actionScheduler.CancelCurrentAction();
        }

        private void AttackBehaviour()
        {
            _fighter.Attack(_player);
        }

        #endregion
    }
}