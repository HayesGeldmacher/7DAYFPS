using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
     private float _timeElapsed;
    
    

    // Update is called once per frame
    void Update()
    {
        _timeElapsed += Time.deltaTime;
    }
}
