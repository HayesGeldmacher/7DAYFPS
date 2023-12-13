using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class goreExplode : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float lineLifeSpan = 2f;
    [SerializeField] private float lowerBounds = 200f;
    [SerializeField] private float upperBounds = 400f;

    // Start is called before the first frame update
    void Start()
    {
        //Randomly adds a force to each gib and throws the around the arena

        float shootX = Random.Range(lowerBounds, upperBounds);
        float shootY = Random.Range(lowerBounds, upperBounds);
        float shootZ = Random.Range(lowerBounds, upperBounds);

        float finalShootX = (Random.Range(0, 2) * 2 - 1) * shootX;
        float finalShootY = (Random.Range(0, 2) * 2 - 1) * shootY;
        float finalShootZ = (Random.Range(0, 2) * 2 - 1) * shootZ;

        rb.AddForce(new Vector3(finalShootX, finalShootY, finalShootZ));
    }


}
