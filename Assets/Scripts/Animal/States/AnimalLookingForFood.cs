using UnityEngine;

public class AnimalLookingForFood : AnimalState
{
    private int _eatingTime = 2;

    private float timer;
    private Transform _target;    

    private void RandomWalkTarget()
    {
        int iPoint = Random.Range(0, FoodManager.Instance.DeactivatedFoods.Count);
        _target = FoodManager.Instance.DeactivatedFoods[iPoint].transform;
        controller.Agent.SetDestination(_target.transform.position);
    }

    private void Eating()
    {
        if (FoodManager.Instance.ActivateFoods.Count != 0)
        {
            timer += Time.fixedDeltaTime;

            if (timer >= _eatingTime)
            {
                _target.gameObject.GetComponent<FoodObject>().HasBeenEaten();
                RandomWalkTarget();
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
        RandomWalkTarget();
    }

    public override void OnUpdate()
    {
        if (!DetectedFood && controller.Agent.remainingDistance <= controller.Agent.stoppingDistance)
            RandomWalkTarget();

        if (FoodManager.Instance.ActivateFoods.Count != 0 && DetectedFood && controller.Agent.remainingDistance <= controller.Agent.stoppingDistance)
            Eating();

        if (FoodManager.Instance.ActivateFoods.Count == 0)
            controller.ChangeState(new AnimalWalking());
    }

    public override void OnExit()
    {
        _target = null;
    }
}
