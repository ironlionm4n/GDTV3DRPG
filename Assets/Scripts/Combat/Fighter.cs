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

        private Transform _target;
        private NavMeshAgentMover _navMeshMover;
        private ActionScheduler _actionScheduler;
        private Animator _playerAnimator;
        private float _timeSinceLastAttack = 0;
        private static readonly int AttackL1 = Animator.StringToHash("AttackL1");

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
                if (Vector3.Distance(transform.position, _target.position) > weaponRange)
                    _navMeshMover.MoveTo(_target.position);
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
            
            _playerAnimator.SetTrigger(AttackL1);
            _timeSinceLastAttack = 0f;
        }

        #endregion

        #region CustomFunctions

        public void Attack(CombatTarget combatTarget)
        {
            _actionScheduler.StartAction(this);
            _target = combatTarget.transform;
        }

        public void Cancel()
        {
            _target = null;
        }

        // animation event
        private void Hit()
        {
            print("Hit");
            _target.GetComponent<Health>().TakeDamage(weaponDamage);
        }
        
        #endregion
    }
}