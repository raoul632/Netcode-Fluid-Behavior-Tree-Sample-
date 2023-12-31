using System.Collections;
using CleverCrow.Fluid.BTs.Tasks;
using CleverCrow.Fluid.BTs.Tasks.Actions;
using CleverCrow.Fluid.BTs.Trees;
using System.Collections.Generic;
using UnityEngine;
using Animancer;
using UnityEngine.AI;
using Unity.Netcode;
using Unity.Netcode.Components;

public static class BehaviorTreeBuilderExtensions {
    public static BehaviorTreeBuilder TaskPatrolAction(this BehaviorTreeBuilder builder, string name = "Task Patrol") {
        return builder.AddNode(new TaskPatrolAction() { Name = name });
    }
}

public class GuardBehavior : NetworkBehaviour
{
   

    [SerializeField]
    private BehaviorTree _tree;

    [SerializeField]
    public Transform[] _waypoints;

    
    //bad naming convention _idle with public accessor 
    //find another way to get access to network animancer 

    [SerializeField]
    public ClipTransition _idle;

    [SerializeField]
    public ClipTransition _attack;

    [SerializeField]
    public ClipTransition _walk;

    [SerializeField]
    public AnimancerComponent _Animancer;

    [SerializeField]
    public NavMeshAgent _agent;

    [SerializeField]
    public AudioClip _walking;

    [SerializeField]
    private GameObject _enemy;

    [SerializeField]
    bool HasEnemyInRange = false;
    [SerializeField]
    bool IsAttacking = false;
    Collider[] hitColliders;

    public NetworkAnimancer _networkAnimancer; 

    

    private void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _networkAnimancer = GetComponent<NetworkAnimancer>();
      

        _networkAnimancer.AddAnimationToDictionary("Idle", _idle);
        _networkAnimancer.AddAnimationToDictionary("Walk", _walk);
        _networkAnimancer.AddAnimationToDictionary("Attack", _attack);



        GameObject parcours = GameObject.Find("Parcours");
        int childCount = parcours.transform.childCount;
        for (int i = 0; i < childCount; i++)
        {
            Transform child = parcours.transform.GetChild(i);
            _waypoints[i] = child; 
            // Fais quelque chose avec chaque enfant
            Debug.Log("Enfant : " + child.name);
        }
        
    }



    private void Awake()
    {
        

    }

   public override void OnNetworkSpawn()
    {
        InitBehaviorTree();
    }

    private void InitBehaviorTree()
    {
        _tree = new BehaviorTreeBuilder(gameObject)
           .Selector()
            .Sequence("Patrol")
            .Condition("Enemy In Range", () => !HasEnemyInRange)
            .Condition("Is Server Authoritative", () => IsServer)
                 .Do("See Enemy", () =>
                 {
                     hitColliders = Physics.OverlapSphere(transform.position, 4);
                     foreach (var hitCollider in hitColliders)
                     {
                         //Debug.Log(hitCollider.name); 
                         //hitCollider.SendMessage("AddDamage");
                         if (hitCollider.gameObject.TryGetComponent(out EnemyManager myClass))
                         {

                             _enemy = myClass.gameObject;
                             HasEnemyInRange = true;
                             Debug.Log("I've watched an enemy");
                             return TaskStatus.Failure;
                         }

                         
                     }


                     
                     return TaskStatus.Success;
                 })
                .TaskPatrolAction()
            .End()
            .Sequence("Go to enemy")
                .Condition("Enemy In Range", () => HasEnemyInRange && !IsAttacking)
                .Condition("Is Server Authoritative", () => IsServer)
                 .Do("Reach Enemy", () =>
                 {
                     if (_enemy == null) { return TaskStatus.Failure; }

                     float distance = Vector2.Distance(new Vector2(transform.position.x, transform.position.z), new Vector2(_enemy.transform.position.x, _enemy.transform.position.z));
                     // Debug.Log("I have the task to attack an enemy the distance is " + distance);
                     _agent.SetDestination(_enemy.transform.position);
                     if (distance < 1.3f)
                     {
                         _agent.isStopped = false;
                         _agent.ResetPath();
                         IsAttacking = true;
                         Debug.Log("I've reached an enemy");
                         _Animancer.Play(_attack);
                         
                         _networkAnimancer.SendAnimancerStateClientRpc("Attack"); 
                         
                         

                         return TaskStatus.Failure;
                     }
                     return TaskStatus.Success;
                 }).End()
            .Sequence("Attacking")
                .Condition("Enemy In Range", () => IsAttacking)
                .Condition("Is Server Authoritative", () => IsServer)
                 .Do("Reach Enemy", () =>
                 {
                     float distance = 999.9f;
                     if (_enemy != null)
                     {
                         distance = Vector2.Distance(new Vector2(transform.position.x, transform.position.z), new Vector2(_enemy.transform.position.x, _enemy.transform.position.z));
                     }
                     else if (distance > 1.3f)
                     {
                         HasEnemyInRange = false;
                         IsAttacking = false;
                         Debug.Log("Attacking enenmy");
                         _Animancer.Play(_idle);
                         return TaskStatus.Failure;
                     }



                     //if(_enemy == null)
                     //{
                     //    IsAttacking = false;
                     //    HasEnemyInRange = false; 
                     //}

                     return TaskStatus.Success;
                 }).End()

            .Build();
    }

    private void Update()
    {
        if (!IsServer) return; 
            // Update our tree every frame
            _tree.Tick();
        
    }

    public void PlayWalkingSound()
    {
       
           SoundManager.Instance.PlayAudioClip(_walking, 0.25f);
        
    }

    public void LaunchAttack()
    {
       
            if (_enemy != null)
                _enemy.GetComponent<EnemyManager>().TakeHit();
        
    }
}
