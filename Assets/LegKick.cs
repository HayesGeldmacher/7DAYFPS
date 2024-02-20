using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LegKick : MonoBehaviour
{
    [SerializeField] private BoxCollider _bc;
    [SerializeField] private float _damage;
    [SerializeField] private Animator _legAnimator; 
    
  

    private void OnTriggerEnter(Collider _other)
    {
       
               if (_other.transform.tag == "enemy")
                {
                    _other.gameObject.GetComponent<Health>()?.TakeDamage(500);
            _legAnimator.SetTrigger("stab");
                       
                }

       
            
        
    }

    private void OnCollisionEnter(Collision _other)
    {
        if (_other.transform.tag == "wall")
        {
            EthanPlayerMovement.instance.ExitKick();
        }
    }
}
