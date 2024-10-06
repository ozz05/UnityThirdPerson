using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponDamage : MonoBehaviour
{
    [SerializeField] private int _damage;
    [SerializeField] private float _knockback;
    [SerializeField] private Collider _myCollider;
    private List<Collider> _colliders = new List<Collider>();
    
    private void OnEnable() {
        ClearDamageList();
    }
    private void OnTriggerEnter(Collider other) {
        if (other == _myCollider || _colliders.Contains(other)) return;
        _colliders.Add(other);

        if (other.TryGetComponent<Health>(out Health health))
        {
            health.TakeDamage(_damage);
        }
        if (other.TryGetComponent<ForceReceiver>(out ForceReceiver forceReceiver))
        {
            Vector3 direction = (other.transform.position - _myCollider.transform.position).normalized;
            forceReceiver.AddForce(direction * _knockback);
        }
    }
    public void SetWeaponDamage(int damage, float knockback)
    {
        _damage = damage;
        _knockback = knockback;
    }
    private void ClearDamageList()
    {
        _colliders.Clear();
    }
}
