using System;
using RPG.Stats;
using TMPro;
using UnityEngine;

public class LevelDisplay : MonoBehaviour
{
    [SerializeField] private TMP_Text levelText;

    private BaseStats _baseStats;

    private void Start()
    {
        _baseStats = GameObject.FindGameObjectWithTag("Player").GetComponent<BaseStats>();
    }

    private void Update()
    {
        levelText.text = _baseStats.GetLevel().ToString();
    }
}
