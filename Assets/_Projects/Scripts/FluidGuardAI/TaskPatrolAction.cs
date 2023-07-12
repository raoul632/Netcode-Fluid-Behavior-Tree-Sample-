using System.Collections;
using CleverCrow.Fluid.BTs.Tasks;
using CleverCrow.Fluid.BTs.Tasks.Actions;
using CleverCrow.Fluid.BTs.Trees;
using System.Collections.Generic;
using UnityEngine;
using Animancer;
using UnityEngine.AI; 

public class TaskPatrolAction : ActionBase
{
    private Transform _transform;
    private Transform[] _waypoints;

    private int _currentWaypointIndex = 0;

    private float _waitTime = 1f; // in seconds
    private float _waitCounter = 0f;
    private bool _waiting = false;

    GuardBehavior _guardBehavior;

    private AnimancerComponent _animancer;


    private ClipTransition _idle;
    private ClipTransition _attack;
    private ClipTransition _walk;
    private NavMeshAgent _agent;



    protected override void OnInit()
    {
        _guardBehavior = Owner.GetComponent<GuardBehavior>();
        
        _waypoints = _guardBehavior._waypoints;
        _transform = _guardBehavior.transform;
        _idle = _guardBehavior._idle;
        _attack = _guardBehavior._attack;
        _walk = _guardBehavior._walk;
        _animancer = _guardBehavior._Animancer;
        _agent = _guardBehavior._agent; 
        _animancer.Play(_idle); 



    }

    public TaskPatrolAction()
    {
       
    }
    protected override TaskStatus OnUpdate()
    {
        if (_waiting)
        {
            _animancer.Play(_idle);
            _waitCounter += Time.deltaTime;
            if (_waitCounter >= _waitTime)
            {
                _waiting = false;
            }
        }
        else
        {
            Transform wp = _waypoints[_currentWaypointIndex];
            if (Vector2.Distance(new Vector2(_transform.position.x, _transform.position.z), new Vector2(wp.position.x,wp.position.z)) < 0.025f)
            {
               // _transform.position = wp.position;
                _waitCounter = 0f;
                _waiting = true;

                _currentWaypointIndex = (_currentWaypointIndex + 1) % _waypoints.Length;
            }
            else
            {
                // _transform.position = Vector3.MoveTowards(_transform.position, wp.position, GuardBT.speed * Time.deltaTime);
                //_transform.LookAt(wp.position);
                _animancer.Play(_walk);
                _agent.SetDestination(wp.position); 
            }
        }
        return TaskStatus.Success;
    }


}