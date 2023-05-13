using System;
using RPG.Core;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float projectileSpeed;
    private Health _target;
    private CapsuleCollider _targetCapsuleCollider;
    private float _damage;

    private void Update()
    {
        if (_target != null)
        {
            transform.LookAt(GetAimLocation());
            transform.Translate(Vector3.forward * projectileSpeed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Health>() != _target) return;
        
        _target.TakeDamage(_damage);
        Destroy(gameObject);
    }

    public void SetTarget(Health target, float damage)
    {
        _target = target;
        _targetCapsuleCollider = target.GetComponent<CapsuleCollider>();
        _damage = damage;
    }

    private Vector3 GetAimLocation()
    {
        return _target.transform.position + (Vector3.up * _targetCapsuleCollider.height / 1.75f);
    }
}