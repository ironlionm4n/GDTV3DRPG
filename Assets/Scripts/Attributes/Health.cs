using Newtonsoft.Json.Linq;
using RPG.Core;
using RPG.SaveSystem;
using RPG.Stats;
using UnityEngine;

namespace RPG.Attributes
{
    public class Health : MonoBehaviour, IJsonSavable
    {
        private float _healthPoints = -1f;
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

        private void Start()
        {
            if (_healthPoints < 0)
            {
                _healthPoints = GetComponent<BaseStats>().GetStat(Stats.Stats.Health);
            }
        }

        public void TakeDamage(GameObject instigator, float damage)
        {
            _healthPoints = Mathf.Max(_healthPoints - damage, 0f);
            if (_healthPoints != 0) return;
            if (_hasDied) return;
            
            Died();
            AwardExperience(instigator);
        }

        public float GetHealthPercentage()
        {
            return (_healthPoints / GetComponent<BaseStats>().GetStat(Stats.Stats.Health)) * 100f;
        }

        private void AwardExperience(GameObject instigator)
        {
            var exp = instigator.GetComponent<Experience>();
            if (exp == null) return;
            
            exp.GainExperience(GetComponent<BaseStats>().GetStat(Stats.Stats.Experience));
        }
        private void Died()
        {
            _hasDied = true;
            _animator.SetTrigger(Die);
            _actionScheduler.CancelCurrentAction();
        }
        
        public JToken CaptureAsJToken()
        {
            return JToken.FromObject(_healthPoints);
        }

        public void RestoreFromJToken(JToken state)
        {
            _healthPoints = state.ToObject<float>();
            UpdateState();
        }

        private void UpdateState()
        {
            if (_healthPoints <= 0)
            {
                Died();
            }
        }
    }
}