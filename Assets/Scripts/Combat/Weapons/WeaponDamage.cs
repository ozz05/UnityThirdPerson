using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponDamage : MonoBehaviour
{
    [SerializeField] private int _damage;
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
    }
    public void ClearDamageList()
    {
        _colliders.Clear();
    }
}
