using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class audioRandomizer : MonoBehaviour
{
    private AudioSource _audio;

    [SerializeField] private List<AudioClip> _clips = new List<AudioClip>();

    [SerializeField] private float _minSoundVolume = 0.8f;
    [SerializeField] private float _maxSoundVolume = 0.12f;
    [SerializeField] private float _minPitch = 0.8f;
    [SerializeField] private float _maxPitch = 1.2f;

    // Start is called before the first frame update
    void Start()
    {
        SoundManager();
    }

    private void SoundManager()
    {
        //randomly play the shotgun audio at a different sound and pitch
        _audio = GetComponent<AudioSource>();
        _audio.clip = _clips[Random.Range(0, _clips.Count)];

        float randomPitch = Random.Range(_minPitch, _maxPitch);
        float randomVolume = Random.Range(_minSoundVolume, _maxSoundVolume);

        _audio.pitch = randomPitch;
        _audio.volume = randomVolume;
        _audio.Play();
    }
}
