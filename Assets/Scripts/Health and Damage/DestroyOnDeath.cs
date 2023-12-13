using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Health))]
public class DestroyOnDeath : MonoBehaviour
{
    [SerializeField] private bool _spawnCorpse;
    [SerializeField] private GameObject _corpse;

    private void OnEnable()
    {
        GetComponent<Health>().OnDeath += HandleDeath;
    }

    private void OnDisable()
    {
        GetComponent<Health>().OnDeath -= HandleDeath;
    }

    private void HandleDeath()
    {
        if( _spawnCorpse)
        {
            Instantiate(_corpse, transform.position, Quaternion.identity);
        }
        
        Destroy(gameObject);
    }
}
