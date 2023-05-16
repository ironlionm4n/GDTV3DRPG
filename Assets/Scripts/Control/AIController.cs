using System;
using RPG.Attributes;
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
        [SerializeField] private PatrolPath patrolPath;
        [SerializeField] private float waypointArrivalThreshold;
        [SerializeField] private float waypointDwellTime;
        [SerializeField, Range(0,1)] private float patrolSpeedRatio = 0.2f;

        private Health _health;
        private GameObject _player;
        private Fighter _fighter;
        private Vector3 _guardPosition;
        private int _targetWaypointIndex;
        private NavMeshAgentMover _navMeshAgentMover;
        private float _timeSinceLastSawPlayer;
        private float _timeSinceArrivedAtWaypoint;
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
            _timeSinceArrivedAtWaypoint = Mathf.Infinity;
            _targetWaypointIndex = 0;
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

            IncrementTimers();
        }

        private void IncrementTimers()
        {
            _timeSinceLastSawPlayer += Time.deltaTime;
            _timeSinceArrivedAtWaypoint += Time.deltaTime;
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

            var nextPosition = _guardPosition;

            if (patrolPath != null)
            {
                // have arrived at waypoint
                if (AtWaypoint())
                {
                    _timeSinceArrivedAtWaypoint = 0;
                    // find the next waypoint in path
                    CycleWaypoint();
                }

                nextPosition = GetCurrentWaypoint();
            }

            if (_timeSinceArrivedAtWaypoint > waypointDwellTime)
            {
                _navMeshAgentMover.StartMoveAction(nextPosition, patrolSpeedRatio);
            }
        }

        private Vector3 GetCurrentWaypoint()
        {
            return patrolPath.GetWaypoint(_targetWaypointIndex);
        }

        private void CycleWaypoint()
        {
            _targetWaypointIndex = patrolPath.GetNextIndex(_targetWaypointIndex);
        }

        private bool AtWaypoint()
        {
            return Vector3.Distance(transform.position, GetCurrentWaypoint()) < waypointArrivalThreshold;
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