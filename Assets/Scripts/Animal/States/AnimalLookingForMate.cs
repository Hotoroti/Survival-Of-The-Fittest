using UnityEngine;

public class AnimalLookingForMate : AnimalState
{
    private int _matingTime = 5;

    private float _timer;

    public override void OnEnter()
    {
        CurrentState = "LookingForMate";
        Debug.Log(CurrentState);        
    }

    public override void OnExit()
    {
        controller.MateTime = 5f;
        _timer = 0f;
    }

    public override void OnUpdate()
    {
        if (AnimalManager.Instance.FemaleAnimalsToMate != null && DetectedMate && controller.Agent.remainingDistance <= controller.Agent.stoppingDistance)
            Mate();

        /*if (!DetectedMate && controller.Agent.remainingDistance <= controller.Agent.stoppingDistance)
            WalkTarget();*/

        /*if (AnimalManager.Instance.FemaleAnimalsToMate == null)
            controller.ChangeState(new AnimalWalking());*/

        /*if (FoodManager.Instance.ActivateFoods.Count != 0 && controller.CurrentLife <= controller.AnimalDNA.Chromosomes[2] * .5f)
            controller.ChangeState(new AnimalLookingForFood());*/
    }

    private void Mate()
    {
        _timer += Time.fixedDeltaTime;

        if (_timer < _matingTime) return;

        Debug.Log("Mating");
    }
}
