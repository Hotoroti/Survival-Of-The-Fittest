public class AnimalWalking : AnimalState
{
    public override void OnEnter()
    {
        CurrentState = "Walking";
        WalkTarget();
    }

    public override void OnUpdate()
    {
        if (controller.Agent.remainingDistance <= controller.Agent.stoppingDistance)
            WalkTarget();

        if (FoodManagerNew.Instance.ActivateFoods.Count != 0 && controller.CurrentLife <= controller.AnimalDNA.Chromosomes[2] * .5f)
            controller.ChangeState(new AnimalLookingForFood());
    }

    public override void OnExit()
    {
    }
}
