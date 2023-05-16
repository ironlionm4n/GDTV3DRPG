using RPG.Attributes;
using TMPro;
using UnityEngine;

namespace RPG.Combat
{
    public class EnemyHealthDisplay : MonoBehaviour
    {
        [SerializeField] private TMP_Text healthValueText;
        private Fighter _fighter;
        

        private void Awake()
        {
            _fighter = GameObject.FindWithTag("Player").GetComponent<Fighter>();
        }

        private void Update()
        {
            if (_fighter.GetTarget() == null)
            {
                healthValueText.text = "N/A";
            }
            else
            {
                healthValueText.text = $"{_fighter.GetTarget().GetHealthPercentage():F2}";
            }
        }
    }
}