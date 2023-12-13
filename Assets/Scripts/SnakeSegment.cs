using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

[RequireComponent(typeof(Health))]
public class SnakeSegment : MonoBehaviour
{
    public SnakeSegment ParentSegment = null;
    public SnakeSegment ChildSegment = null;
    public float DistanceFromChild = 1f;
    public float Speed = 10f;
    public int PathResolution = 10;
    [HideInInspector] public List<Vector3> Path = new List<Vector3>();
    private Vector3 _targetPosition;
    private Quaternion _targetRotation;

    private void OnEnable()
    {
        GetComponent<Health>().OnDeath += OnDeath;
    }

    private void OnDisable()
    {
        GetComponent<Health>().OnDeath -= OnDeath;
    }

    private void Start()
    {
        InitializePath();
    }

    private void Update()
    {
        UpdatePath();
        FollowParent();
    }

    public int Length()
    {
        if (ChildSegment == null)
            return 1;
        else
            return 1 + ChildSegment.Length();
    }

    protected void InitializePath()
    {
        for (int i = 0; i <= PathResolution; i++)
        {
            Path.Add(transform.position);
        }
    }

    protected void UpdatePath()
    {
        if (Vector3.Distance(transform.position, Path.Last()) > DistanceFromChild / PathResolution)
        {
            Path.Add(transform.position);
            Path.RemoveAt(0);
        }
    }

    protected void FollowParent()
    {
        _targetPosition = ParentSegment.Path.First();
        if (_targetPosition != transform.position)
            _targetRotation = Quaternion.LookRotation(_targetPosition - transform.position, Vector3.up);
        transform.position += (_targetPosition - transform.position).normalized * Speed * Time.deltaTime;
        transform.rotation = Quaternion.RotateTowards(transform.rotation, _targetRotation, 360f * Time.deltaTime);
    }

    protected void SetSpeed(float speed)
    {
        Speed = speed;
        if (ChildSegment != null)
            ChildSegment.SetSpeed(speed);
    }

    protected SnakeHead GetHead()
    {
        if (ParentSegment == null)
            return (SnakeHead)this;
        else
            return ParentSegment.GetHead();
    }

    private void OnDeath()
    {
        ResetHealth();
        if (ChildSegment != null)
        {
            ChildSegment.enabled = false;
            SnakeHead newHead = ChildSegment.gameObject.AddComponent<SnakeHead>();
            newHead.Target = GetHead().Target;
            newHead.Speed = GetHead().Speed;
            newHead.NormalSpeed = GetHead().NormalSpeed;
            newHead.AttackSpeed = GetHead().AttackSpeed;
            newHead.ChildSegment = ChildSegment.ChildSegment;
            newHead.DistanceFromChild = ChildSegment.DistanceFromChild;
            if (ChildSegment.ChildSegment != null)
                ChildSegment.ChildSegment.ParentSegment = newHead;
            if (GetHead().Attacking)
                newHead.StartCoroutine(newHead.Attack());
        }
        if (ParentSegment != null)
        {
            ParentSegment.ChildSegment = null;
            ParentSegment = null;
        }
        Destroy(gameObject);
    }

    private void ResetHealth()
    {
        if (ParentSegment == null)
            GetComponent<Health>().CurrentHealth = 1;
        else
            GetComponent<Health>().CurrentHealth = GetComponent<Health>().MaxHealth;
        Health childHealth = ChildSegment?.GetComponent<Health>();
        Health parentHealth = ParentSegment?.GetComponent<Health>();
        if (ChildSegment != null && childHealth.CurrentHealth != childHealth.MaxHealth)
            ChildSegment.ResetHealth();
        if (ParentSegment != null && parentHealth.CurrentHealth != parentHealth.MaxHealth)
            ParentSegment.ResetHealth();
    }
    
}
