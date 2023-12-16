using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private List<SpawnInfo> _spawnables;
    private Dictionary<GameObject, int> _spawnableCounts = new Dictionary<GameObject, int>();
    [SerializeField] private float _intensityRate = 1.01f;
    private float _spawnTimer = 0f;
    private GameManager _gameManager;
    private Transform _player;

    private void Start()
    {
        _gameManager = FindObjectOfType<GameManager>();
        foreach (SpawnInfo spawnInfo in _spawnables)
        {
            _spawnableCounts.Add(spawnInfo.prefab, 0);
        }
        _player = FindObjectOfType<EthanPlayerMovement>().transform;
    }


    private void Update()
    {
        _spawnTimer -= Time.deltaTime;
        if (_spawnTimer < 0f)
            Spawn();
    }

    private SpawnInfo GetSpawnable()
    {
        float totalSpawnChance = 0f;
        foreach (SpawnInfo spawnInfo in _spawnables)
        {
            totalSpawnChance += spawnInfo.spawnChance;
        }

        float random = Random.Range(0f, totalSpawnChance);
        float current = 0f;
        foreach (SpawnInfo spawnInfo in _spawnables)
        {
            current += spawnInfo.spawnChance;
            if (random < current)
            {
                return spawnInfo;
            }
        }

        return null;
    }

    private Vector3 GetSpawnPosition()
    {
        Vector2 random = Random.insideUnitCircle.normalized * 80f;
        return new Vector3(random.x, 0f, random.y);
    }

    private void Spawn()
    {
        SpawnInfo spawnInfo = GetSpawnable();
        Vector3 spawnPosition = GetSpawnPosition();
        GameObject instance = Instantiate(spawnInfo.prefab, spawnPosition, Quaternion.LookRotation(_player.position - spawnPosition, Vector3.up));
        instance.transform.parent = transform;
        // _spawnableCounts[spawnInfo.prefab]++;
        // instance.GetComponent<Health>().OnDeath += () => _spawnableCounts[spawnInfo.prefab]--;
        _spawnTimer = spawnInfo.spawnInterval / Mathf.Pow(_intensityRate, _gameManager.TimeElapsed);
    }
}

[System.Serializable]
public class SpawnInfo
{
    public GameObject prefab;
    public float spawnChance;
    public float spawnInterval;
    public float maxSpawnCount;
}
