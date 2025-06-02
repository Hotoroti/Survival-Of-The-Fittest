using UnityEngine;

public abstract class AnimalState
{
    protected float energyConsumption = 0f;
    protected AnimalController controller;
    protected AnimalDNA dna;
    protected GameObject animalOBJ;

    protected readonly float _baseMovementSpeed;
    protected float currentMovementSpeed;

    protected bool SlowMovement { private set; get; }

    public AnimalState(AnimalController controller,
        AnimalDNA dna,
        GameObject animalObject) : base()
    {
        this.controller = controller;
        this.dna = dna;
        this.animalOBJ = animalObject;

        _baseMovementSpeed = dna.Chromosomes["WalkingSpeed"] / (dna.Size * 2);
        currentMovementSpeed = _baseMovementSpeed;
    }

    /// <summary>
    /// Moves the animal towards the target position.
    /// </summary>
    protected void WalkTowards(Vector3 target)
    {
        animalOBJ.transform.position = Vector3.MoveTowards(animalOBJ.transform.position, target, currentMovementSpeed * TimeSettings.Instance.DeltaTime);
        animalOBJ.transform.LookAt(target);
        EnergyConsumption();
    }


    protected abstract void GetNewPosition();


    /// <summary>
    /// Slows movement if energy is low.
    /// </summary>
    protected void HandleLowEnergyMovement()
    {
        if (controller.CurrentEnergy < dna.Chromosomes["Energy"] * 0.25f)
        {
            currentMovementSpeed = _baseMovementSpeed * 0.5f;
            SlowMovement = true;
        }
    }

    /// <summary>
    /// Replenishes energy when moving slowly.
    /// </summary>
    protected void ReplenishEnergyOnSlowMovement()
    {
        if (SlowMovement)
        {
            controller.ReplenishEnergy(50f);
            controller.HungerConsumption(5f);
            if (controller.CurrentEnergy >= dna.Chromosomes["Energy"] * 0.85f)
            {
                SlowMovement = false;
                currentMovementSpeed = _baseMovementSpeed;
            }
        }
    }

    /// <summary>
    /// Moves toward target and consumes hunger.
    /// </summary>
    protected void WalkTowardsWithHungerConsumption(Vector3 target, float hungerAmount = 10f)
    {
        WalkTowards(target);
        if (!SlowMovement)
            controller.HungerConsumption(hungerAmount);
    }


    /// <summary>
    /// Move towards target or get a new one if invalid or reached.
    /// </summary>
    protected void MoveTowardsTargetOrGetNewPosition(Vector3 targetPos)
    {
        if (Utils.IsOnGround(targetPos))
        {
            if (ArrivedAtTarget(targetPos))
                GetNewPosition();
            else
                WalkTowardsWithHungerConsumption(targetPos);
        }
        else
        {
            GetNewPosition();
        }
    }


    /// <summary>
    /// Call this function to check if the organism has arrive at the target
    /// </summary>
    /// <param name="target">The target of the organism</param>
    /// <returns>If the organims has arrive the target</returns>
    protected bool ArrivedAtTarget(Vector3 target)
    {
        return Vector3.Distance(animalOBJ.transform.position, target) <= .5f;
    }

    /// <summary>
    /// Call this function to consump energy
    /// </summary>
    protected void EnergyConsumption()
    {
        controller.EnergyConsumption(energyConsumption);
    }

    protected bool IsMale() => dna.Gender == 0;
    protected bool IsFemale() => dna.Gender == 1;

    public virtual void OnTriggerEnter(Collider other)
    {
    }

    /// <summary>
    /// Call this function when entering a State
    /// </summary>
    public abstract void OnEnter();

    /// <summary>
    /// Call this function to Update the state
    /// </summary>
    public abstract void OnUpdate();

    /// <summary>
    /// Call this function when Exiting the state
    /// </summary>
    public abstract void OnExit();

    /// <summary>
    /// Checks if this animal can mate with the other.
    /// </summary>
    protected bool CanMateWith(AnimalController other)
    {
        if (other == null)
        {
            Debug.LogWarning("Mate candidate is null.");
            return false;
        }
        if (other.IsDead)
        {
            Debug.Log("Mating rejected: Animal is dead.");
            return false;
        }
        if (other.Dna.Carnivore != dna.Carnivore)
        {
            Debug.Log("Mating rejected: Different species type.");
            return false;
        }
        if (other.Dna.Gender == dna.Gender)
        {
            Debug.Log("Mating rejected: Same gender.");
            return false;
        }
        if (!other.HasMatured)
        {
            Debug.Log("Mating rejected: Partner not mature.");
            return false;
        }
        if (other.MateOBJ != null && other.MateOBJ != controller)
        {
            Debug.Log("Mating rejected: Partner already has another mate.");
            controller.MateOBJ = null;
            return false;
        }
        return true;
    }
}
