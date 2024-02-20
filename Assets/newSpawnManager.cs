using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class newSpawnManager : MonoBehaviour
{
    [SerializeField] private List<SpawnClass> _spawnables;
    [SerializeField] private Transform _centerPoint;
    private Dictionary<GameObject, int> _spawnableCounts = new Dictionary<GameObject, int>();
    [SerializeField] private float _intensityRate = 1.01f;
    [SerializeField] private List<Transform> spawnPoints = new List<Transform>();
    private float _spawnTimer = 0f;
    private GameManager _gameManager;
    private Transform _player;
  
    public int _currentRound;

    private void Start()
    {
        _currentRound = 0;
        StartCoroutine(WaitForWave());
        
    }

    private void AntennaSlam()
    {

    }

    private void Spawn()
    {
        int _spawnRounds = 0;
        SpawnClass _currentSpawn = _spawnables[_currentRound];
        foreach(GameObject _enemy2Spawn in _currentSpawn._enemyGroups)
        {
            Transform _spawnPoint = _currentSpawn._spawnLocations[_spawnRounds];
            Antenna _antenna = _spawnPoint.GetChild(0).GetComponent<Antenna>();
            _spawnPoint.GetChild(0).GetComponent<Animator>().SetTrigger("slam");
            int _spawnCount = _currentSpawn._enemyCounts[_spawnRounds];
            for (int i = 0; _spawnCount > i; i++)
            {
               GameObject _currentEnemy =  Instantiate(_enemy2Spawn, _spawnPoint.position, Quaternion.identity);
                _antenna._enemies.Add(_currentEnemy);
            }

            _antenna.Deactivate();
            _spawnRounds++;
        }
        
        _currentRound += 1;
       if(_currentRound <= _spawnables.Count - 1)
        {
        StartCoroutine(WaitForWave());

        }
        else
        {
            Debug.Log("END of SpAWNS!");
        }
    }

    private IEnumerator WaitForWave()
    {
        SpawnClass _currentSpawn = _spawnables[_currentRound];
        yield return new WaitForSeconds(_currentSpawn._waitTime);
        Spawn();
    }
}

[System.Serializable]
public class SpawnClass
{
    public List<GameObject> _enemyGroups = new List<GameObject>();
    public List<int> _enemyCounts = new List<int>();
    public List<Transform> _spawnLocations = new List<Transform>();
    public float _waitTime;
}

