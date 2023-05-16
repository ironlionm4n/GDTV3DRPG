using Newtonsoft.Json.Linq;
using RPG.Core;
using RPG.SaveSystem;
using RPG.Stats;
using UnityEngine;

namespace RPG.Attributes
{
    public class Health : MonoBehaviour, IJsonSavable
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

        private void Start()
        {
            healthPoints = GetComponent<BaseStats>().GetHealth();
        }

        public void TakeDamage(GameObject instigator, float damage)
        {
            healthPoints = Mathf.Max(healthPoints - damage, 0f);
            if (healthPoints != 0) return;
            if (_hasDied) return;
            
            Died();
            AwardExperience(instigator);
        }

        public float GetHealthPercentage()
        {
            return (healthPoints / GetComponent<BaseStats>().GetHealth()) * 100f;
        }

        private void AwardExperience(GameObject instigator)
        {
            var exp = instigator.GetComponent<Experience>();
            if (exp == null) return;
            
            exp.GainExperience(GetComponent<BaseStats>().GetExperienceReward());
        }
        private void Died()
        {
            _hasDied = true;
            _animator.SetTrigger(Die);
            _actionScheduler.CancelCurrentAction();
        }
        
        public JToken CaptureAsJToken()
        {
            return JToken.FromObject(healthPoints);
        }

        public void RestoreFromJToken(JToken state)
        {
            healthPoints = state.ToObject<float>();
            UpdateState();
        }

        private void UpdateState()
        {
            if (healthPoints <= 0)
            {
                Died();
            }
        }
    }
}