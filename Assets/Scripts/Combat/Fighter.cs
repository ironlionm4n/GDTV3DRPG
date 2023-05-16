using Newtonsoft.Json.Linq;
using RPG.Core;
using UnityEngine;
using RPG.PlayerMovement;
using RPG.SaveSystem;
using RPG.Attributes;
using RPGCharacterAnims.Lookups;
using UnityEngine.Serialization;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction, IJsonSavable
    {
        #region Variables

        [SerializeField] private float timeBetweenAttacks = 1f;
        [SerializeField] private Transform rightHandTransform;
        [SerializeField] private Transform lefttHandTransform;
        [SerializeField] private Weapon defaultWeapon;
        [SerializeField] private string defaultWeaponName = "Unarmed";

        private Health _target;
        private NavMeshAgentMover _navMeshMover;
        private ActionScheduler _actionScheduler;
        private Animator _playerAnimator;
        private float _timeSinceLastAttack = Mathf.Infinity;
        private Weapon _currentEquippedWeapon;
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

        private void OnEnable()
        {
            var weapon = Resources.Load<Weapon>(defaultWeaponName);
            EquipWeapon(weapon);
        }

        private void Update()
        {
            _timeSinceLastAttack += Time.deltaTime;
            if (_target != null)
            {
                if (_target.HasDied) return;

                if (Vector3.Distance(transform.position, _target.transform.position) >
                    _currentEquippedWeapon.GetWeaponRange)
                {
                    _navMeshMover.MoveTo(_target.transform.position, 1f);
                }
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
            _navMeshMover.Cancel();
        }

        // animation event
        private void Hit()
        {
            if (_currentEquippedWeapon.HasProjectile() && _target != null)
            {
                _currentEquippedWeapon.LaunchProjectile(rightHandTransform, lefttHandTransform,
                    _target);
            }
            else
            {
                if (_target != null) _target.TakeDamage(_currentEquippedWeapon.GetWeaponDamage);
            }
        }

        private void Shoot()
        {
            Hit();
        }

        public void EquipWeapon(Weapon weapon)
        {
            _currentEquippedWeapon = weapon;
            _currentEquippedWeapon.Spawn(rightHandTransform, lefttHandTransform, _playerAnimator);
        }

        #endregion

        public JToken CaptureAsJToken()
        {
            return JToken.FromObject(_currentEquippedWeapon.name);
        }

        public void RestoreFromJToken(JToken state)
        {
            var weaponName = state.ToObject<string>();
            var weapon = Resources.Load<Weapon>(weaponName);
            EquipWeapon(weapon);
        }
    }
}