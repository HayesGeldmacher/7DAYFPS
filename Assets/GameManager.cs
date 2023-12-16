using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private TMP_Text _timeText;
    private bool _isSlowedDown = false;
    private float _pauseEndTime = 0;
    public float TimeElapsed { get; private set; } = 0f;

    #region Singleton

    public static GameManager instance;

    void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("More than one instance of inventory present!! NOT GOOD!");
            return;
        }

        instance = this;
    }

    #endregion


    // Update is called once per frame
    void Update()
    {
        TimeElapsed += Time.deltaTime;
        _timeText.text = TimeElapsed.ToString("F2");

        
        //counts real time to check when to stop slow motion
        if (_isSlowedDown)
        {
            if (Time.realtimeSinceStartup > _pauseEndTime)
            {
                Debug.Log("BackToNormal");
                Time.timeScale = 1f;
                _isSlowedDown = false;
            }
        }
    }

    public IEnumerator SlowMotion(float time)
    {

        Time.timeScale = 0.2f;
        _isSlowedDown = true;
        _pauseEndTime = Time.realtimeSinceStartup + time;


        yield return new WaitForSeconds(0.1f);

    }
}
