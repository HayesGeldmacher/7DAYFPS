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
    [SerializeField] private GameObject _devilCoin;
    [SerializeField] private float _coinSpawnChance;
    


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

        float _randomChance = Random.Range(0, 100);
        if(_randomChance <= _coinSpawnChance)
        {
            Vector3 SpawnPosition = new Vector3(transform.position.x, transform.position.y + 2, transform.position.z);
            GameObject _coin = Instantiate(_devilCoin, SpawnPosition, Quaternion.identity);
            Rigidbody _rb = _coin.GetComponent<Rigidbody>();
            float _randomX = Random.Range(30, 100);
            float _randomZ = Random.Range(30, 100);
            Vector3 velocity = new Vector3(_randomX, 10, _randomZ);
            _rb.AddForce(velocity);
        }
        
        //EthanPlayerMovement.instance.transform.GetComponent<playerHealth>().AddRage(_ragePoints);

        Destroy(gameObject);
    }

    private void StartSlowMotion()
    {
        GameManager slow = GameManager.instance;
        slow.CallSlowMotion(_slowTime);
        
    }
}
