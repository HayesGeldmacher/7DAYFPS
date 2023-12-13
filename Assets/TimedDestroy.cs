using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedDestroy : MonoBehaviour
{

    [SerializeField] private float _destroyTime;
    float _currentTime; 


    // Start is called before the first frame update
    void Start()
    {
        _currentTime = 0;  
    }

    // Update is called once per frame
    void Update()
    {
        _currentTime += Time.deltaTime;
        if(_currentTime > _destroyTime)
        {
            Destroy();
        }
    }

    private void Destroy()
    {
        Destroy(gameObject); 
    }
}
