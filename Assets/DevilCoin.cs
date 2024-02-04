using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DevilCoin : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            GameManager.instance.CallCoinGrabSound();
            GameManager.instance.CallSlowMotion(0.1f);
            Destroy(gameObject);
        }
    }
}
