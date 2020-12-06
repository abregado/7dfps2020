using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.Characters.ThirdPerson;
using Random = UnityEngine.Random;

public class EnemyAI : MonoBehaviour {
    public GameObject head;
    private NavMeshAgent _agent;
    protected Transform _player;
    private ThirdPersonCharacter _character;
    public LayerMask whatIsGround, whatIsPlayer;
    public NavMeshSurface navMeshSurface;

    public float _patrolSpeed = 0.3f;
    public float _chaseSpeed = 1f;
    public float _attackSpeed = 0.01f;
    
    public Vector3 walkPoint;
    private bool _walkPointSet;
    public Transform[] waypoints;
    private int _currentWaypoint;
    private bool _waiting;
    public float _waitTimeMin = 3f;
    public float _waitTimeMax = 10f;
    public float forgettingTime = 100f;
    private float _lastTimeSeen;

    public float timeBetweenAttacks;
    protected bool _alreadyAttacked;

    public float sightRange, attackRange;
    [SerializeField] public bool playerInSightRange, playerInAttackRange, playerSeen;
    
    
    void Awake() {
        _agent = GetComponent<NavMeshAgent>();
        _character = GetComponent<ThirdPersonCharacter>();
        _lastTimeSeen = -10000f;
    }
    
    void Update() {
        if (!_player)
        {
            _player = GameObject.FindWithTag("Player").transform;
            if (!_player)
                return;
        }

        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        CheckPlayerSeen();

        if (playerSeen && Time.time > _lastTimeSeen + forgettingTime) {
            playerSeen = false;
        }
        
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if (playerInAttackRange) {
            _agent.speed = _attackSpeed;
        }else if (playerSeen) {
            _agent.speed = _chaseSpeed;
        }
        else {
            _agent.speed = _patrolSpeed;
        }
        
        _character.Move(_agent.desiredVelocity, false, false);
        
        if (playerInAttackRange) {
            AttackPlayer();
        } else if (playerSeen && !playerInAttackRange) {
            ChasePlayer();
        } else if (!playerInSightRange && !playerInAttackRange) {
            Patrolling();
        }
    }

    private void CheckPlayerSeen() {

        if (playerInSightRange) {
            Vector3 direction = _player.transform.position - head.transform.position;
            Ray ray = new Ray(head.transform.position, direction);
            RaycastHit hit;
    
            
            if (Physics.Raycast(ray, out hit, sightRange) && LayerMask.LayerToName(hit.collider.gameObject.layer) == "Player") {
                //Debug.Log("Player spotted");
                playerSeen = true;
                _lastTimeSeen = Time.time;
            }
        }
    }
    
    private void Patrolling() {

        //Debug.Log("Patrolling");
        if (!_walkPointSet && !_waiting) {
            SearchWalkPoint();
        }

        if (_walkPointSet) {
            _agent.SetDestination(walkPoint);
        }

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        if (distanceToWalkPoint.magnitude < 1f) {
            _walkPointSet = false;
            ArrivedAtPoint();
        }
    }

    private void SearchWalkPoint() {
        int randomWaypoint = Random.Range(0, waypoints.Length);
        navMeshSurface.BuildNavMesh();
        walkPoint = waypoints[randomWaypoint].position;
        _walkPointSet = true;
    }

    private void ArrivedAtPoint() {
        _waiting = true;
        Invoke(nameof(ResetWaiting),Random.Range(_waitTimeMin,_waitTimeMax));
    }

    private void ResetWaiting() {
        _waiting = false;
    }
    
    
    private void ChasePlayer() {
        //Debug.Log("Chasing");
        _agent.SetDestination(_player.position);
    }

    protected virtual void AttackPlayer() {
        _agent.SetDestination(transform.position);

        if (!_alreadyAttacked) {
            _alreadyAttacked = true;
            Debug.Log("Bang!");
            Invoke(nameof(ResetAttack),timeBetweenAttacks);
        }
    }

    private void ResetAttack() {
        _alreadyAttacked = false;
    }

    public void DealDamage(int damage)
    {
        Destroy(gameObject); // ignore damage amount for now
    }
    
}
