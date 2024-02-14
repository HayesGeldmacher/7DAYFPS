using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DevilCoin : MonoBehaviour
{
    [SerializeField] private float _coinTime = 5;
    [SerializeField] private float _rageValue = 10;
    [SerializeField] private float _floatDistance;
    [SerializeField] private float _floatSpeed;
    private Transform _player;
    private Rigidbody _rb;
    [SerializeField] private float _gravity;
   
    
    
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SelfDestruct());
        _player = EthanPlayerMovement.instance.transform;
        _rb = transform.parent.GetComponent<Rigidbody>();
        Explode();
    }

    void Explode()
    {
        float _randomX = Random.Range(-50, 50);
        float _randomY = Random.Range(-10, 10);
        float _randomZ = Random.Range(-50, 50);
        _rb.AddForce(_randomX, _randomY, _randomZ);
    }

    // Update is called once per frame
    void Update()
    {
        float _distance = Vector3.Distance(_player.position, transform.position);
        if(_distance < _floatDistance)
        {
            Vector3 direction = _player.position - transform.position;
            transform.parent.transform.position += direction * _floatSpeed * Time.deltaTime;
           // _rb.AddForce(direction * _floatSpeed);

        }
        else
        {

            //Vector3 _gravityForce = new Vector3 (0, _gravity, 0);
          //  _rb.AddForce(_gravityForce);
           
        }
    }
    

    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.tag == "Player")
        {
            playerHealth _player = other.transform.GetComponent<playerHealth>();
            _player.AddRage(_rageValue);
            
            GameManager.instance.CallCoinGrabSound();
            GameManager.instance.CallSlowMotion(0.1f);
            Destroy(gameObject.transform.parent.gameObject);
        }
    }

    private IEnumerator SelfDestruct()
    {
        yield return new WaitForSeconds(_coinTime);
        //lets play an effect here!
        Destroy(gameObject.transform.parent.gameObject);
    }
}
