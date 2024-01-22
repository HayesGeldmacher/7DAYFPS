using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProximity : MonoBehaviour
{

    [SerializeField] private LayerMask _enemyMask;
    [SerializeField] private List<Transform> _enemiesNearby = new List<Transform>();
    [SerializeField] private float _radius;
    [SerializeField] private float _distance;
    [SerializeField] private float _behindTolerance;
    [SerializeField] private float lowestDistance = 100;
    [SerializeField] private Transform _currentDanger;
    [SerializeField] private bool _isDanger;
    [SerializeField] private Animator _spriteAnim;

    // Start is called before the first frame update
    void Start()
    {
        lowestDistance = 100;
    }

    // Update is called once per frame
    void Update()
    {
        //Raycast to every enemy 

        RaycastHit[] _hits = (Physics.SphereCastAll(transform.position, _radius, -transform.forward, _distance, _enemyMask));
        foreach (RaycastHit _hit in _hits)
        {
            if (_hit.transform != null)
            {
                if (!_enemiesNearby.Contains(_hit.transform))
                {
                    _enemiesNearby.Add(_hit.transform);

                }
            }
        }

        ListUpdate();
    }

    private void ListUpdate()
    {
        foreach (Transform _enemy in _enemiesNearby)
        {
            if (!_enemy)
            {
                _enemiesNearby.Remove(_enemy);

            }
            else
            {

                //first we check distance
                float _currentDistance = Vector3.Distance(_enemy.position, transform.position);
                if (_currentDistance > _distance)
                {
                    _enemiesNearby.Remove(_enemy);
                }
                else
                {

                    //then we check if enemy is actually behind the player!
                    Vector3 toTarget = (_enemy.transform.position - transform.position).normalized;

                    if (Vector3.Dot(toTarget, transform.forward) > _behindTolerance)
                    {
                        //target is in front of gameObject, removing from list
                        _enemiesNearby.Remove(_enemy);
                    }
                    else
                    {
                        //YAYYY!! enemy is behind player and in dangerous range!
                    }
                }


            }
        }

        if(_enemiesNearby.Count > 0)
        {
            _isDanger = true;
            DangerUpdate();
            _spriteAnim.SetBool("alert", true);
        }
        else
        {
            _isDanger = false;
            _currentDanger = null;
            _spriteAnim.SetBool("alert", false);
            
        }
    }

    private void DangerUpdate()
    {
        foreach (Transform _enemy in _enemiesNearby)
        {
            float _currentDistance = Vector3.Distance(_enemy.position, transform.position);
            if(_currentDistance < lowestDistance)
            {
               lowestDistance = _currentDistance;
                _currentDanger = _enemy.transform;
                
            }
        }

        SpriteUpdate();
    }

    private void SpriteUpdate()
    {
        //Get Some BIG Sprite to announce an enemy behind us or something!!!
        if(_isDanger)
        {
            _spriteAnim.SetBool("alert", true);
        }
        else
        {
            _spriteAnim.SetBool("alert", false);
        }
    }
}
