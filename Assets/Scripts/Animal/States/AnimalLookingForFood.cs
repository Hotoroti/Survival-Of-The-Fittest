using UnityEngine;

public class AnimalLookingForFood : AnimalState
{
    private int _eatingTime = 5;

    private float timer;
    private Transform _target;

    public AnimalLookingForFood(AnimalController animalController) : base(animalController)
    {
    }

    private void RandomWalkTarget()
    {
        int iPoint = Random.Range(0, FoodManager.Instance.ActivateFoods.Count);
        _target = FoodManager.Instance.ActivateFoods[iPoint].transform;
        controller.Agent.SetDestination(_target.transform.position);
    }

    private void Eating()
    {
        timer += Time.fixedDeltaTime;

        if (timer >= _eatingTime)
        {
            _target.gameObject.GetComponent<FoodObject>().HasBeenEaten();
            RandomWalkTarget();
            timer = 0;
            controller.ResetLife();
            controller.ChangeState(new AnimalWalking(controller));
        }
    }

    public override void OnStateEnter()
    {
        RandomWalkTarget();
    }

    public override void OnStateUpdate()
    {
        if (controller.Agent.remainingDistance <= controller.Agent.stoppingDistance)
            Eating();
    }

    public override void OnStateExit()
    {
        _target = null;
    }
}
