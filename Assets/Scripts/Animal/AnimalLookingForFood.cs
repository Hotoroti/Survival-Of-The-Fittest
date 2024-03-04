using UnityEngine;
using UnityEngine.AI;

public class AnimalLookingForFood : MonoBehaviour
{
    [SerializeField] private int _eatingTime;

    private float timer;
    private Transform _target;

    private NavMeshAgent _agent;

    private void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        RandomWalkTarget();
    }

    private void FixedUpdate()
    {
        if (_agent.remainingDistance <= _agent.stoppingDistance)
            Eating();


    }

    private void RandomWalkTarget()
    {
        int iPoint = Random.Range(0, FoodManager.Instance.ActivateFoods.Count);
        _target = FoodManager.Instance.ActivateFoods[iPoint].transform;
        _agent.SetDestination(_target.transform.position);
    }

    private void Eating()
    {
        timer += Time.fixedDeltaTime;

        if (timer >= _eatingTime)
        {
            _target.gameObject.GetComponent<FoodObject>().HasBeenEaten();
            RandomWalkTarget();
            timer = 0;
        }
    }
}
