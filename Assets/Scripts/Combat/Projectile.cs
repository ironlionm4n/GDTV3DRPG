using System;
using RPG.Core;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float projectileSpeed;
    [SerializeField] private bool isHoming;
    [SerializeField] private GameObject hitEffect;
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
    
        if (hitEffect != null)
        {
            Instantiate(hitEffect, GetAimLocation(), transform.rotation);
        }
        
        Destroy(gameObject);
    }

    public void SetTarget(Health target, float damage)
    {
        _target = target;
        _targetCapsuleCollider = target.GetComponent<CapsuleCollider>();
        _damage = damage;
        LookAtTargetAimLocation();
    }

    private Vector3 GetAimLocation()
    {
        return _target.transform.position + Vector3.up * _targetCapsuleCollider.height / 1.75f;
    }
}