using Newtonsoft.Json.Linq;
using RPG.Combat;
using RPG.Core;
using RPG.SaveSystem;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.PlayerMovement
{
    public class NavMeshAgentMover : MonoBehaviour, IAction, IJsonSavable
    {
        #region Variables

        [SerializeField] private float maxSpeed;

        private Animator _playerAnimator;
        private NavMeshAgent _playerAgent;
        private Health _health;
        private static readonly int MovementSpeed = Animator.StringToHash("MoveSpeed");
        private ActionScheduler _actionScheduler;

        #endregion

        #region UnityMethods

        private void Awake()
        {
            _playerAgent = GetComponent<NavMeshAgent>();
            _actionScheduler = GetComponent<ActionScheduler>();
            _playerAnimator = GetComponent<Animator>();
            _health = GetComponent<Health>();
        }

        private void Update()
        {
            _playerAgent.enabled = !_health.HasDied;
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

        public void StartMoveAction(Vector3 destination, float speedFraction)
        {
            _actionScheduler.StartAction(this);
            MoveTo(destination, speedFraction);
        }
        
        public void MoveTo(Vector3 targetPosition, float speedFraction)
        {
            if (_playerAgent.SetDestination(targetPosition))
            {
                _playerAgent.speed = maxSpeed * Mathf.Clamp01(speedFraction);
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

        public JToken CaptureAsJToken()
        {
            return transform.position.ToToken();
        }

        public void RestoreFromJToken(JToken state)
        {
            _playerAgent.enabled = false;
            transform.position = state.ToVector3();
            _playerAgent.enabled = true;
            _actionScheduler.CancelCurrentAction();
        }
        
        #endregion
    }
}