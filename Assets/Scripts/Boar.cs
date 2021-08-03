using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using MEC;
using System.Linq;

[RequireComponent(typeof(NavMeshAgent))]

public class Boar : MonoBehaviour
{
    [SerializeField] private CreatureProfile profile;
    [SerializeField] private CreatureSettings settings;

    private Entity thisEntity;
    private NavMeshAgent agent;
    private SphereCollider perceptionTrigger;
    private Transform target;
    private List<Entity> perceivedEntites;

    private bool isWandering;

    private float hunger;

    private CoroutineHandle decisionHandle;
    private CoroutineHandle needsHandle;

    private static float maxHunger;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        perceptionTrigger = GetComponent<SphereCollider>();
        perceptionTrigger.radius = profile.perceptionRange;
        perceivedEntites = new List<Entity>();
        thisEntity = GetComponent<Entity>();
    }

    private void OnEnable()
    {
        decisionHandle = Timing.RunCoroutine(DecisionCycle());
        needsHandle = Timing.RunCoroutine(NeedsCycle());
    }

    private void OnDisable()
    {
        Timing.PauseCoroutines(decisionHandle);
        Timing.PauseCoroutines(needsHandle);
    }

    private void Update()
    {
        isWandering = CheckIfWandering();
        TryEat();
    }

    private void OnTriggerEnter(Collider other)
    {
        Entity _entity = other.transform.GetComponent<Entity>();
        if (_entity == null) return;
        if (!perceivedEntites.Contains(_entity)) perceivedEntites.Add(_entity);
    }

    private void OnTriggerExit(Collider other)
    {
        Entity _entity = other.transform.GetComponent<Entity>();
        if (_entity == null) return;
        if (perceivedEntites.Contains(_entity)) perceivedEntites.Remove(_entity);
    }

    private bool CheckIfWandering()
    {
        return agent.remainingDistance > agent.stoppingDistance;
    }

    private void TryEat()
    {
        if (target == null) return;
        float _distance = Vector3.Distance(transform.position, target.position);
        if (_distance > profile.interactionRange) return;

        Entity entity = target.GetComponent<Entity>();

        if (entity.entityType == EntityTypes.Fruit)
        {
            Fruit food = entity.transform.GetComponent<Fruit>();
            hunger += food.Nutrition;
            perceivedEntites.Remove(entity);
            target = null;
            Destroy(food.gameObject);
        }
    }

    private Vector3 RandomDestination()
    {
        //Returns a random position for the navmesh agent within the creature's wander radius

        float _x, _z, _y;
        _y = 0;
        _x = transform.position.x;
        _z = transform.position.z;
        float _xMod = Random.Range(profile.wanderMinRange, profile.wanderMaxRange);
        float _zMod = Random.Range(profile.wanderMinRange, profile.wanderMaxRange);        
        if (Random.Range(0,2) == 0)
        {
            _xMod = -_xMod;
            _zMod = -_zMod;
        }
        _x += _xMod;
        _z += _zMod;

        Vector3 _newDestination = new Vector3(_x, _y, _z);
        return _newDestination;
    }

    IEnumerator<float> DecisionCycle()
    {
        while (true)
        {
            bool _decisionMade = false;

            //Get food
            if (hunger < maxHunger && !_decisionMade)

            {
                List<Entity> edibleEntities = new List<Entity>();

                perceivedEntites.FindAll(entity => entity.entityType == EntityTypes.Fruit && entity != null)
                    .ForEach(entity =>
                    {
                        Fruit food = entity.transform.GetComponent<Fruit>();
                        if (food.CheckEdibility(thisEntity.entityType) && food.isDetached) edibleEntities.Add(entity);
                    });

                float _minDistance = float.MaxValue;

                if (edibleEntities.Count != 0)
                {
                    edibleEntities.ForEach(entity =>
                    {
                        float _distance = Vector3.Distance(transform.position, entity.transform.position);
                        if (_distance < _minDistance)
                        {
                            _minDistance = _distance;
                            target = entity.transform;
                            agent.SetDestination(target.position);
                        }
                    });

                    _decisionMade = true;
                }

            }

            //Go wandering
            if (!isWandering && !_decisionMade)
            {
                agent.SetDestination(RandomDestination());

                target = null;
                isWandering = true;
            }

            yield return Timing.WaitForSeconds(settings.decisionCycleRate);
        }
    }

    IEnumerator<float> NeedsCycle()
    {
        while (true)
        {
            hunger -= profile.hungerRate;
            yield return Timing.WaitForSeconds(1);
        }
    }

}
