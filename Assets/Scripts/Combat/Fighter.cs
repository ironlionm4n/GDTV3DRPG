using RPG.Core;
using UnityEngine;
using RPG.PlayerMovement;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction
    {
        #region Variables

        [SerializeField] private float weaponRange = 2f;
        [SerializeField] private float weaponDamage = 5f;
        [SerializeField] private float timeBetweenAttacks = 1f;

        private Health _target;
        private NavMeshAgentMover _navMeshMover;
        private ActionScheduler _actionScheduler;
        private Animator _playerAnimator;
        private float _timeSinceLastAttack = Mathf.Infinity;
        private static readonly int AttackL1 = Animator.StringToHash("AttackL1");
        private static readonly int StopAttack = Animator.StringToHash("StopAttack");

        #endregion

        #region UnityFunctions

        private void Awake()
        {
            _navMeshMover = GetComponent<NavMeshAgentMover>();
            _actionScheduler = GetComponent<ActionScheduler>();
            _playerAnimator = GetComponent<Animator>();
        }

        private void Update()
        {
            _timeSinceLastAttack += Time.deltaTime;
            if (_target != null)
            {
                if (_target.HasDied) return;
     
                if (Vector3.Distance(transform.position, _target.transform.position) > weaponRange)
                    _navMeshMover.MoveTo(_target.transform.position);
                else
                {
                    _navMeshMover.Cancel();
                    AttackState();
                }
            }
        }

        private void AttackState()
        {
            if (!(_timeSinceLastAttack >= timeBetweenAttacks)) return;
            
            transform.LookAt(_target.transform);
            TriggerAttack();

        }

        private void TriggerAttack()
        {
            _playerAnimator.ResetTrigger(StopAttack);
            _playerAnimator.SetTrigger(AttackL1);
            _timeSinceLastAttack = 0f;
        }

        #endregion

        #region CustomFunctions

        public bool CanAttack(GameObject combatTarget)
        {
            if (combatTarget == null) return false;
            
            var combatTargetHealth = combatTarget.GetComponent<Health>();
            return combatTargetHealth != null && !combatTargetHealth.HasDied;
        }
        
        public void Attack(GameObject combatTarget)
        {
            _actionScheduler.StartAction(this);
            _target = combatTarget.transform.GetComponent<Health>();
        }

        public void Cancel()
        {
            _playerAnimator.SetTrigger(StopAttack);
            _playerAnimator.ResetTrigger(AttackL1);
            _target = null;
        }

        // animation event
        private void Hit()
        {
            if (_target != null)
            {
                _target.TakeDamage(weaponDamage);
            }
        }
        
        #endregion
    }
}