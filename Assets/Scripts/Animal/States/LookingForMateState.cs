using UnityEngine;

public class LookingForMateState : AnimalState
{
    private Vector3 _newPos = Vector3.zero;
    public LookingForMateState(AnimalController controller, AnimalDNA dna, GameObject animalObject) : base(controller, dna, animalObject)
    {
    }

    public override void OnEnter()
    {
        Debug.Log("Looking for Mate");
        if (controller.MateOBJ == null)
        {
            GetNewPosition();
            controller.SenseCollider.OnHerbivoreAnimalEnter += OnHerbivalAnimalEnter;
        }
    }

    public override void OnExit()
    {
        controller.SenseCollider.OnHerbivoreAnimalEnter -= OnHerbivalAnimalEnter;
    }

    public override void OnUpdate()
    {
        if (controller.MateOBJ != null)
        {
            WalkTowards(controller.MateOBJ.transform.position);
            controller.HungerConsumption(10);
            if (ArrivedAtTarget(controller.MateOBJ.transform.position))
            {
                controller.SwitchState(new MatingState(controller, dna, animalOBJ, controller.MateOBJ));
            }

            _newPos = controller.MateOBJ.transform.position;
            return;
        }

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
            GetNewPosition();
    }

    /// <summary>
    /// Call this function to get a new position in the sense radius
    /// </summary>
    private void GetNewPosition()
    {
        _newPos = (animalOBJ.transform.position + Random.insideUnitSphere * dna.Chromosomes["Sense"]);
        _newPos = new Vector3(_newPos.x, 0.1f, _newPos.z);
    }

    private void OnHerbivalAnimalEnter(GameObject animal)
    {
        AnimalController otherAnimalController = animal.GetComponent<AnimalController>();

        if (otherAnimalController == null)
        {
            Debug.Log("Could not return AnimalController");
            return;
        }

        if (otherAnimalController.Dna.Gender == dna.Gender)
        {
            Debug.Log("Same Gender");
            return;
        }

        if (!otherAnimalController.HasMatured)
        {
            Debug.Log("Was not yet mature");
            return;
        }

        if (otherAnimalController.MateOBJ != null)
        {
            Debug.Log("Already has a Mate");
            if (otherAnimalController.MateOBJ != animalOBJ)
            {
                controller.MateOBJ = null;
            }
            return;
        }

        controller.MateOBJ = otherAnimalController;
        otherAnimalController.MateOBJ = controller;
        Debug.Log("Found Mate");
    }

}
