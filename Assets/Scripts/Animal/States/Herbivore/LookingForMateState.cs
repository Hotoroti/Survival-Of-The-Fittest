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
        HandleLowEnergyMovement();
        ReplenishEnergyOnSlowMovement();

        if (controller.MateOBJ != null)
        {
            HandleMatingApproach();
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
                WalkTowardsIdleTarget();
            }
        }
        else
        {
            GetNewPosition();
        }
    }

    private void HandleMatingApproach()
    {
        Vector3 matePosition = controller.MateOBJ.transform.position;

        WalkTowards(matePosition);

        if (!SlowMovement)
        {
            controller.HungerConsumption(10f);
        }

        if (ArrivedAtTarget(matePosition))
        {
            controller.SwitchState(new MatingState(controller, dna, animalOBJ, controller.MateOBJ));
        }

        _newPos = matePosition;
    }

    void WalkTowardsIdleTarget()
    {
        WalkTowards(_newPos);

        if (!SlowMovement)
        {
            controller.HungerConsumption(10f);
        }
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
        // Null check
        if (animal == null)
        {
            Debug.LogWarning("Animal GameObject is null.");
            return;
        }

        // Get the controller
        AnimalController other = animal.GetComponent<AnimalController>();
        if (other == null)
        {
            Debug.LogWarning("No AnimalController found on target.");
            return;
        }

        //Don't mate with dead animal
        if (other.IsDead)
        {
            Debug.Log("Mating rejected: Animal is dead.");
            return;
        }

        // Don't mate with different species (e.g., herbivore vs carnivore)
        if (other.Dna.Carnivore != dna.Carnivore)
        {
            Debug.Log("Mating rejected: Different species type.");
            return;
        }

        // Don't mate with the same gender
        if (other.Dna.Gender == dna.Gender)
        {
            Debug.Log("Mating rejected: Same gender.");
            return;
        }

        // Must be mature
        if (!other.HasMatured)
        {
            Debug.Log("Mating rejected: Partner not mature.");
            return;
        }

        // Check if already has a mate
        if (other.MateOBJ != null)
        {
            // If it's not already paired with this animal, cancel this attempt
            if (other.MateOBJ != controller)
            {
                Debug.Log("Mating rejected: Partner already has another mate.");
                controller.MateOBJ = null;
            }
            return;
        }

        // Assign mates mutually
        controller.MateOBJ = other;
        other.MateOBJ = controller;

    }

}
