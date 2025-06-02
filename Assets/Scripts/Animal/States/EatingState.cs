using UnityEngine;

public class EatingState : AnimalState
{
    private GameObject _foodObj;
    private float _eatTimer;

    private const float EATTIMEDAY = 0.5f;

    public EatingState(AnimalController controller, AnimalDNA dna, GameObject animalObject, GameObject foodObject) : base(controller, dna, animalObject)
    {
        _foodObj = foodObject;
    }

    public override void OnEnter()
    {
    }

    public override void OnExit()
    {
    }

    public override void OnUpdate()
    {
        if (!HasEating())
        {
            _eatTimer += TimeSettings.Instance.DeltaTime;
            controller.ReplenishEnergy(50f);
            return;
        }

        FinishedEating();
    }

    private bool HasEating()
    {
        if (_eatTimer > EATTIMEDAY * TimeCycle.Instance.SecondsInDay)
        {
            return true;
        }
        else
            return false;
    }

    private void FinishedEating()
    {
        controller.RechargeHunger();

        if (dna.Carnivore >= 1)
            controller.SwitchState(new HuntingState(controller, dna, animalOBJ));
        else
            controller.SwitchState(new RoamingState(controller, dna, animalOBJ));

        GameObject.Destroy(_foodObj);
    }

    protected override void GetNewPosition()
    {
    }
}
