using UnityEngine;

public class LookingForMateState : AnimalState
{
    private Vector3 _newPos = Vector3.zero;
    private bool _slowMove = false;
    public LookingForMateState(AnimalController controller, AnimalDNA dna, GameObject animalObject) : base(controller, dna, animalObject)
    {
    }

    public override void OnEnter()
    {
        Debug.Log("Looking for Mate");
        if (controller.MateOBJ == null)
        {
            GetNewPosition();
            if (dna.Carnivore >= 1)
                controller.SenseCollider.OnCarnivoreAnimalEnter += OnAnimalEnter;
            else
                controller.SenseCollider.OnHerbivoreAnimalEnter += OnAnimalEnter;


        }
    }

    public override void OnExit()
    {
        if (dna.Carnivore >= 1)
            controller.SenseCollider.OnCarnivoreAnimalEnter -= OnAnimalEnter;
        else
            controller.SenseCollider.OnHerbivoreAnimalEnter -= OnAnimalEnter;
    }

    public override void OnUpdate()
    {
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

        if (controller.MateOBJ != null)
        {
            WalkTowards(controller.MateOBJ.transform.position);
            if (!_slowMove)
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
                if (!_slowMove)
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

    private void OnAnimalEnter(GameObject animal)
    {
        AnimalController otherAnimalController = animal.GetComponent<AnimalController>();

        //Check if it has a controller
        if (otherAnimalController == null)
        {
            Debug.Log("Could not return AnimalController");
            return;
        }

        //Check if it is the same animal
        if (otherAnimalController.Dna.Carnivore != dna.Carnivore)
        {
            Debug.Log("Other type of animal as mate?");
            return;
        }

        //Check if it is a different gender
        if (otherAnimalController.Dna.Gender == dna.Gender)
        {
            Debug.Log("Same Gender");
            return;
        }

        //Check if the animal is matured
        if (!otherAnimalController.HasMatured)
        {
            Debug.Log("Was not yet mature");
            return;
        }

        //Check if the animal already has a mate
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
