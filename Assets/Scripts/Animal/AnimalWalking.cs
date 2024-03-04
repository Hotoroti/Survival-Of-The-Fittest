using UnityEngine;
using UnityEngine.AI;

public class AnimalWalking : MonoBehaviour
{
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
            RandomWalkTarget();
    }

    private void RandomWalkTarget()
    {
        int iPoint = Random.Range(0, GenerateGrid.Instance.GridCells.Count);
        _target = GenerateGrid.Instance.GridCells[iPoint].transform;
        _agent.SetDestination(_target.transform.position);
    }
}
