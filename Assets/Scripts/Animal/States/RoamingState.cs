using UnityEngine;

public class RoamingState : AnimalState
{
    private Vector3 _newPos = Vector3.zero;

    public RoamingState(AnimalController controller, AnimalDNA dna, GameObject animalObject) : base(controller, dna, animalObject) { }

    public override void OnEnter()
    {
        energyConsumption = 0.5f * dna.Size * Mathf.Pow(currentMovementSpeed, 2);
        GetNewPosition();
    }

    public override void OnUpdate()
    {
        HandleBehaviorSwitching();
        HandleLowEnergyMovement();
        ReplenishEnergyOnSlowMovement();

        if (Utils.IsOnGround(_newPos))
            HandleIdleGroundMovement();
        else
            GetNewPosition();
    }

    public override void OnExit() { }

    /// <summary>
    /// Handles the switching of the states when necessary
    /// </summary>
    private void HandleBehaviorSwitching()
    {
        if (controller.ReadyToMate)
        {
            controller.SwitchState(new LookingForMateState(controller, dna, animalOBJ));
            return;
        }

        if (controller.HungerScore >= 0.66f)
        {
            if (dna.Carnivore >= 1)
                controller.SwitchState(new HuntingState(controller, dna, animalOBJ));
            else
                controller.SwitchState(new LookingForFoodState(controller, dna, animalOBJ));
        }
    }

    /// <summary>
    /// Handles the movement when on the ground
    /// </summary>
    private void HandleIdleGroundMovement()
    {
        if (ArrivedAtTarget(_newPos))
        {
            GetNewPosition();
            return;
        }

        if (SlowMovement)
            controller.HungerConsumption(10f);

        WalkTowards(_newPos);
    }

    protected override void GetNewPosition()
    {
        _newPos = animalOBJ.transform.position + Random.insideUnitSphere * dna.Chromosomes["Sense"];
        _newPos = new Vector3(_newPos.x, 0.1f, _newPos.z);
    }
}
