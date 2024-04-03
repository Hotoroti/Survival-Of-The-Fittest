using UnityEngine;

public class AnimalLookingForFood : AnimalState
{
    private int _eatingTime = 2;

    private float timer;
    private void Eating()
    {
        if (FoodManagerNew.Instance.ActivateFoods.Count != 0)
        {
            timer += Time.fixedDeltaTime;

            if (timer >= _eatingTime)
            {
                if (controller.Food == null) return;
                controller.Food.HasBeenEaten();
                timer = 0;
                controller.ResetLife();
                DetectedFood = false;
                controller.ChangeState(new AnimalWalking());
            }
        }
    }

    public override void OnEnter()
    {
        CurrentState = "LookingForFood";
        timer = 0;
        WalkTarget();
    }

    public override void OnUpdate()
    {
        if (!DetectedFood && controller.Agent.remainingDistance <= controller.Agent.stoppingDistance)
            WalkTarget();

        if (FoodManagerNew.Instance.ActivateFoods.Count != 0 && DetectedFood && controller.Agent.remainingDistance <= controller.Agent.stoppingDistance)
            Eating();

        if (FoodManagerNew.Instance.ActivateFoods.Count == 0)
            controller.ChangeState(new AnimalWalking());
    }

    public override void OnExit()
    {
        controller.Food = null;
        DetectedFood = false;
    }
}
