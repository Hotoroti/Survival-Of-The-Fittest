using UnityEngine;

public class AnimalWalking : AnimalState
{
    private Transform _target;

    private void RandomWalkTarget()
    {
        int iPoint = Random.Range(0, GenerateGrid.Instance.GridCells.Count);
        _target = GenerateGrid.Instance.GridCells[iPoint].transform;
        controller.Agent.SetDestination(_target.transform.position);
    }

    public override void OnEnter()
    {
        CurrentState = "Walking";
        RandomWalkTarget();
    }

    public override void OnUpdate()
    {
        if (controller.Agent.remainingDistance <= controller.Agent.stoppingDistance)
            RandomWalkTarget();

        if (FoodManager.Instance.ActivateFoods.Count != 0 && controller.CurrentLife <= controller.AnimalDNA.Chromosomes[2] * .5f)
            controller.ChangeState(new AnimalLookingForFood());
    }

    public override void OnExit()
    {
        _target = null;
    }
}
