using System.Collections;
using System.Collections.Generic;
using RPG.Core;
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
        [SerializeField] private bool isRightHanded;
        [SerializeField] private Projectile projectile;

        public float GetWeaponRange => weaponRange;
        public float GetWeaponDamage => weaponDamage;

        private const string _weaponName = "Weapon";

        public void Spawn(Transform rightHandTransform, Transform leftHandTransform, Animator animator)
        {
            DestroyOldWeapon(rightHandTransform, leftHandTransform);
            if (weaponPrefab != null)
            {
                var weapon = Instantiate(weaponPrefab, CheckRightHanded(rightHandTransform, leftHandTransform));
                weapon.name = _weaponName;
            }

            if (weaponAnimatorOverride != null)
            {
                animator.runtimeAnimatorController = weaponAnimatorOverride;
            }
            else
            {
                var overrideController = animator.runtimeAnimatorController as AnimatorOverrideController;
                if (overrideController != null)
                    animator.runtimeAnimatorController = overrideController.runtimeAnimatorController;
            }
        }

        private Transform CheckRightHanded(Transform rightHandTransform, Transform leftHandTransform)
        {
            return isRightHanded ? rightHandTransform : leftHandTransform;
        }

        public bool HasProjectile()
        {
            return projectile != null;
        }

        public void LaunchProjectile(Transform rightHandTransform, Transform leftHandTransform, Health target)
        {
            var projectileInstance = Instantiate(projectile,
                CheckRightHanded(rightHandTransform, leftHandTransform).position, Quaternion.identity);

            projectileInstance.SetTarget(target, weaponDamage);
        }

        private void DestroyOldWeapon(Transform rightHandTransform, Transform leftHandTransform)
        {
            var oldWeapon = rightHandTransform.Find(_weaponName);
            if (oldWeapon == null) oldWeapon = leftHandTransform.Find(_weaponName);

            if (oldWeapon == null) return;

            oldWeapon.name = "DESTROYING";
            Destroy(oldWeapon.gameObject);
        }
    }
}