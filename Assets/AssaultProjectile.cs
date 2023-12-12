using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssaultProjectile : Projectile
{
//     [SerializeField] private Transform bulletMesh;

//     // Start is called before the first frame update
//     void Start()
//     {

//         base._startPos = transform.position;
//     }

//     // Update is called once per frame
//     void Update()
//     {
//         transform.rotation = Quaternion.LookRotation(base._direction);
//         transform.position += transform.forward * base._speed * Time.deltaTime;
//         float _length = Vector3.Distance(base._startPos, transform.position);

//         if(_length > base._distance)
//         {
//             DestroyProjectile();
//         }

//     }


//     void DestroyProjectile()
//     {
//         //get some sort of fading aimation effect instead of just disappearing!
//         Destroy(gameObject);
//     }
}
