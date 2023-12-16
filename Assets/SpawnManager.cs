using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private List<SpawnInfo> _spawnables;
    private Dictionary<GameObject, int> _spawnableCounts = new Dictionary<GameObject, int>();
    private float _spawnTimer = 0f;
    private GameManager _gameManager;

    private void Start()
    {
        _gameManager = FindObjectOfType<GameManager>();
        foreach (SpawnInfo spawnInfo in _spawnables)
        {
            _spawnableCounts.Add(spawnInfo.prefab, 0);
        }
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
                if (_spawnableCounts[spawnInfo.prefab] < spawnInfo.maxSpawnCount)
                {
                    return spawnInfo;
                }
                else
                {
                    return GetSpawnable();
                }
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
        GameObject instance = Instantiate(spawnInfo.prefab, GetSpawnPosition(), Quaternion.identity);
        instance.transform.parent = transform;
        _spawnableCounts[spawnInfo.prefab]++;
        instance.GetComponent<Health>().OnDeath += () => _spawnableCounts[spawnInfo.prefab]--;
        _spawnTimer = spawnInfo.spawnInterval;
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
