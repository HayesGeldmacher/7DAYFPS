using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private TMP_Text _timeText;
    public float TimeElapsed { get; private set; } = 0f;
    
    

    // Update is called once per frame
    void Update()
    {
        TimeElapsed += Time.deltaTime;
        _timeText.text = TimeElapsed.ToString("F2");
    }
}
