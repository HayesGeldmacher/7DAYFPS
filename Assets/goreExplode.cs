using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class goreExplode : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float lineLifeSpan = 2f;
    [SerializeField] private float lowerBounds = 200f;
    [SerializeField] private float upperBounds = 400f;
    [SerializeField] private float _fadeSpeed;
    private float _currentFade;


    private Material _material;

    // Start is called before the first frame update
    void Start()
    {
        _currentFade = 1;
        _material = GetComponent<MeshRenderer>().material;
        
        //Randomly adds a force to each gib and throws the around the arena

        float shootX = Random.Range(lowerBounds, upperBounds);
        float shootY = Random.Range(lowerBounds, upperBounds);
        float shootZ = Random.Range(lowerBounds, upperBounds);

        float finalShootX = (Random.Range(0, 2) * 2 - 1) * shootX;
        float finalShootY = (Random.Range(0, 2) * 2 - 1) * shootY;
        float finalShootZ = (Random.Range(0, 2) * 2 - 1) * shootZ;

        rb.AddForce(new Vector3(finalShootX, finalShootY, finalShootZ));
    }

    void Update()
    {
        _currentFade += (Time.deltaTime * -_fadeSpeed);
        _currentFade = Mathf.Clamp(_currentFade, 0,1);
        _material.SetFloat("Vector1_4fd90d0ce6d349c9a6400ec21704a612", _currentFade);


    }

}
