using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Antenna : MonoBehaviour
{
    public List<GameObject> _enemies = new List<GameObject>();
    [SerializeField] AudioSource _audio;
    public void OnSlam()
    {
        foreach(GameObject enemy in _enemies)
        {
            enemy.SetActive(true);
        }
        _enemies.Clear();
    }

    public void Deactivate()
    {
        foreach (GameObject enemy in _enemies)
        {
            enemy.SetActive(false);
        }
    }

    public void PlayAlarmSound()
    {
        _audio.Play();
    }
}
