using System;
using RPG.Attributes;
using RPG.Core;
using UnityEngine;
using UnityEngine.Serialization;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float projectileSpeed;
    [SerializeField] private bool isHoming;
    [SerializeField] private GameObject hitEffect;
    [SerializeField] private float maxLifeTime = 10f;
    [SerializeField] private GameObject[] destroyOnHit;
    [SerializeField] private float lifeAfterImpact = 2f;
    private Health _target;
    private CapsuleCollider _targetCapsuleCollider;
    private float _damage;

    private void Update()
    {
        if (_target != null)
        {
            if (isHoming && !_target.HasDied) LookAtTargetAimLocation();
            transform.Translate(Vector3.forward * projectileSpeed * Time.deltaTime);
        }
    }

    private void LookAtTargetAimLocation()
    {
        transform.LookAt(GetAimLocation());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Health>() != _target) return;
        if (_target.HasDied) return;
        _target.TakeDamage(_damage);
        projectileSpeed = 0;
        if (hitEffect != null)
        {
            Instantiate(hitEffect, GetAimLocation(), transform.rotation);
        }

        foreach (var toDestroy in destroyOnHit)
        {
            Destroy(toDestroy);
        }
        
        Destroy(gameObject, lifeAfterImpact);
    }

    public void SetTarget(Health target, float damage)
    {
        _target = target;
        _targetCapsuleCollider = target.GetComponent<CapsuleCollider>();
        _damage = damage;
        LookAtTargetAimLocation();
        
        Destroy(gameObject, maxLifeTime);
    }

    private Vector3 GetAimLocation()
    {
        return _target.transform.position + Vector3.up * _targetCapsuleCollider.height / 1.75f;
    }
}