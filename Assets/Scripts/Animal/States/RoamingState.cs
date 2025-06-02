using UnityEngine;

public class RoamingState : AnimalState
{
    private Vector3 _newPos = Vector3.zero;
    private bool _slowMove = false;
    public RoamingState(AnimalController controller, AnimalDNA dna, GameObject animalObject) : base(controller, dna, animalObject)
    {
    }

    public override void OnEnter()
    {
        energyConsumption = .5f * dna.Size * Mathf.Pow(currentMovementSpeed, 2);
        GetNewPosition();
    }

    public override void OnUpdate()
    {
        if (controller.ReadyToMate)
            controller.SwitchState(new LookingForMateState(controller, dna, animalOBJ));
        else if (controller.HungerScore >= .66f)
        {
            if (controller.Dna.Carnivore >= 1)
                controller.SwitchState(new HuntingState(controller, dna, animalOBJ));
            else
                controller.SwitchState(new LookingForFoodState(controller, dna, animalOBJ));
        }

        if (controller.CurrentEnergy < dna.Chromosomes["Energy"] * .25f)
        {
            currentMovementSpeed = _baseMovementSpeed * 0.5f;
            _slowMove = true;
        }

        if (_slowMove)
        {
            controller.ReplenishEnergy(50f);
            controller.HungerConsumption(5f);
            if (controller.CurrentEnergy >= dna.Chromosomes["Energy"] * .85f)
            {
                _slowMove = false;
                currentMovementSpeed = _baseMovementSpeed;
            }
        }

        if (Utils.IsOnGround(_newPos))
        {
            if (ArrivedAtTarget(_newPos))
            {
                GetNewPosition();
            }
            else
            {

                if (!_slowMove)
                {
                    controller.HungerConsumption(10f);
                }

                WalkTowards(_newPos);

            }

        }
        else
            GetNewPosition();
    }

    public override void OnExit()
    {
    }

    /// <summary>
    /// Call this function to get a new position in the sense radius
    /// </summary>
    private void GetNewPosition()
    {
        _newPos = (animalOBJ.transform.position + Random.insideUnitSphere * dna.Chromosomes["Sense"]);
        _newPos = new Vector3(_newPos.x, 0.1f, _newPos.z);
    }

}
