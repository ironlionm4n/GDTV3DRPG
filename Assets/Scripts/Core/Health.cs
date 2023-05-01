using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace RPG.Core
{
    public class Health : MonoBehaviour
    {
        [SerializeField] private float healthPoints = 100f;
        private Animator _animator;
        private bool _hasDied;
        public bool HasDied => _hasDied;
        private static readonly int Die = Animator.StringToHash("Die");
        private ActionScheduler _actionScheduler;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _actionScheduler = GetComponent<ActionScheduler>();
        }

        public void TakeDamage(float damage)
        {
            healthPoints = Mathf.Max(healthPoints - damage, 0f);
            if (healthPoints != 0) return;
            if (_hasDied) return;
            
            Died();
        }

        private void Died()
        {
            _hasDied = true;
            _animator.SetTrigger(Die);
            _actionScheduler.CancelCurrentAction();
        }
    }
}