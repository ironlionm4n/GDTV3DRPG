using RPG.Combat;
using RPG.Core;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.PlayerMovement
{
    public class NavMeshAgentMover : MonoBehaviour, IAction
    {
        #region Variables

        private Animator _playerAnimator;
        private NavMeshAgent _playerAgent;
        private static readonly int MovementSpeed = Animator.StringToHash("MoveSpeed");
        private ActionScheduler _actionScheduler;

        #endregion

        #region UnityMethods

        private void Awake()
        {
            _playerAgent = GetComponent<NavMeshAgent>();
            _actionScheduler = GetComponent<ActionScheduler>();
            _playerAnimator = GetComponent<Animator>();
        }

        private void Update()
        {
            UpdateAnimator();
        }

        private void UpdateAnimator()
        {
            // Convert from global to local because the NavMeshAgent velocity is in world space,
            // Animator parameter needs the local velocity to blend correctly
            var localVelocity = transform.InverseTransformDirection(_playerAgent.velocity);
            _playerAnimator.SetFloat(MovementSpeed, localVelocity.z);
        }

        #endregion

        #region CustomFunctions

        public void StartMoveAction(Vector3 destination)
        {
            _actionScheduler.StartAction(this);
            MoveTo(destination);
        }
        
        public void MoveTo(Vector3 targetPosition)
        {
            if (_playerAgent.SetDestination(targetPosition))
            {
                IsStoppedFalse();
            }
        }

        private void IsStoppedFalse()
        {
            _playerAgent.isStopped = false;
        }

        public void Cancel()
        {
            _playerAgent.isStopped = true;
        }
        
        
        #endregion
    }
}