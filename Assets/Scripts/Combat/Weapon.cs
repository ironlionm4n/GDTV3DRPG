using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace RPG.Combat
{
    [CreateAssetMenu(menuName = "Items/Weapons", fileName = "Weapon", order = 0)]
    public class Weapon : ScriptableObject
    {
        [SerializeField] private AnimatorOverrideController weaponAnimatorOverride;
        [SerializeField] private GameObject weaponPrefab;
        [SerializeField] private float weaponRange = 2f;
        [SerializeField] private float weaponDamage = 5f;

        public float GetWeaponRange => weaponRange;
        public float GetWeaponDamage => weaponDamage;

        public void Spawn(Transform handTransform, Animator animator)
        {
            if (weaponPrefab != null) Instantiate(weaponPrefab, handTransform);
            if (weaponAnimatorOverride != null) animator.runtimeAnimatorController = weaponAnimatorOverride;
        }
    }
}