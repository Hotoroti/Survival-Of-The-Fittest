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
        Debug.Log("Enter Eating State");
    }

    public override void OnExit()
    {
    }

    public override void OnTriggerEnter(Collider other)
    {
    }

    public override void OnUpdate()
    {
        if (!HasEating())
        {
            _eatTimer += TimeSettings.Instance.DeltaTime;
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

        controller.SwitchState(new RoamingState(controller, dna, animalOBJ));

        GameObject.Destroy(_foodObj);
    }
}
