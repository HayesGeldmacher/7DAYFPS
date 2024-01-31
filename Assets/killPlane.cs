using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class killPlane : MonoBehaviour
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
        Debug.Log("FUUUCCKCCC!!!OSS");
        other.GetComponent<Health>()?.TakeDamage(500);
    }

    private void OnCollisionEnter(Collision other)
    {
        Debug.Log("Collided!");
    }
}
