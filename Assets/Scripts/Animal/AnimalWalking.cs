using UnityEngine;

public class AnimalWalking : AnimalState
{
    private Transform _target;

    public AnimalWalking(AnimalController animalController) : base(animalController)
    {
    }

    private void RandomWalkTarget()
    {
        int iPoint = Random.Range(0, GenerateGrid.Instance.GridCells.Count);
        _target = GenerateGrid.Instance.GridCells[iPoint].transform;
        controller.Agent.SetDestination(_target.transform.position);
    }

    public override void OnStateEnter()
    {
        RandomWalkTarget();
    }

    public override void OnStateUpdate()
    {
        if (controller.Agent.remainingDistance <= controller.Agent.stoppingDistance)
            RandomWalkTarget();
    }

    public override void OnStateExit()
    {
        _target = null;
    }
}
