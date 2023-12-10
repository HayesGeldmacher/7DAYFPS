using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

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
    
}
