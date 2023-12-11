using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssaultProjectile : Projectiles
{
    
    // Start is called before the first frame update
    void Start()
    {

        base._startPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += base._direction * base._speed * Time.deltaTime;
        float length = Vector3.Distance(base._startPos, transform.position);

        if(length > base._distance)
        {
            DestroyProjectile();
        }
    }

    void DestroyProjectile()
    {
        //get some sort of fading aimation effect instead of just disappearing!
        Destroy(gameObject);
    }
}
