using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace RPG.Combat
{
    public class Health : MonoBehaviour
    {
        [SerializeField] private float healthPoints = 100f;
        private Animator _animator;
        private bool _hasDied;
        private static readonly int Die = Animator.StringToHash("Die");

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        public void TakeDamage(float damage)
        {
            healthPoints = Mathf.Max(healthPoints - damage, 0f);
            //Debug.Log(healthPoints);
            
            if (healthPoints != 0) return;
            if (_hasDied) return;
            
            _hasDied = true;
            _animator.SetTrigger(Die);
        }
    }
}