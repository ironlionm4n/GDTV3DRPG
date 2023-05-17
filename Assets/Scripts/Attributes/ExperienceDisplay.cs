using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace RPG.Attributes
{
    public class ExperienceDisplay : MonoBehaviour
    {
        [SerializeField] private TMP_Text healthValueText;
        private Experience _experience;
        

        private void Awake()
        {
            _experience = GameObject.FindWithTag("Player").GetComponent<Experience>();
        }

        private void Update()
        {
            healthValueText.text = $"{_experience.GetExperiencePoints:0}";
        }
    }
}
