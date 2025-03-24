using UnityEngine;

public class RoamingState : AnimalState
{
    private Vector3 _newPos = Vector3.zero;

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
        if (Utils.IsOnGround(_newPos))
        {
            if (ArrivedAtTarget(_newPos))
            {
                GetNewPosition();

            }
            else
            {
                WalkTowards(_newPos);
                controller.HungerConsumption(10);
            }

        }
        else
        {
            Debug.Log(Utils.IsOnGround(_newPos));
            GetNewPosition();
        }

        if (controller.HungerScore >= .66f)
            controller.SwitchState(new LookingForFoodState(controller, controller._dna, controller.gameObject));
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

    public override void OnTriggerEnter(Collider other)
    {
    }
}
