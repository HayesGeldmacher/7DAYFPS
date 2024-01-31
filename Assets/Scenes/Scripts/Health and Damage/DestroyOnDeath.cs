using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Health))]
public class DestroyOnDeath : MonoBehaviour
{
    [SerializeField] private bool _spawnCorpse;
    [SerializeField] private GameObject _corpse;
    [SerializeField] private bool _playAudio;
    [SerializeField] private GameObject _audioEffect;
    [SerializeField] private bool _canSlow;
    [SerializeField] private float _slowTime;
    [SerializeField] private bool _canBloom;
    [SerializeField] private float _ragePoints;


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
        
        if( _playAudio)
        {
            Instantiate(_audioEffect, transform.position, Quaternion.identity);
        }

        if (_canSlow)
        {
            StartSlowMotion();
        }

        if(_canBloom)
        {
            GameManager.instance.CallBloomOut();
        }

        EthanPlayerMovement.instance.transform.GetComponent<playerHealth>().AddRage(_ragePoints);

        Destroy(gameObject);
    }

    private void StartSlowMotion()
    {
        GameManager slow = GameManager.instance;
        slow.CallSlowMotion(_slowTime);
        
    }
}
