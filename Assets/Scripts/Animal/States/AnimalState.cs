using UnityEngine;

public abstract class AnimalState
{
    protected float energyConsumption = 0f;
    protected AnimalController controller;
    protected AnimalDNA dna;
    protected GameObject animalOBJ;

    private float _baseMovementSpeed;
    protected float currentMovementSpeed;

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
    /// Call this function so that the organism will walk towards a point
    /// </summary>
    /// <param name="target">The point the organism needs to walk to</param>
    protected void WalkTowards(Vector3 target)
    {
        animalOBJ.transform.position = Vector3.MoveTowards(animalOBJ.transform.position, target, currentMovementSpeed * TimeSettings.Instance.DeltaTime);
        animalOBJ.transform.LookAt(target);
        EnergyConsumption();
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
}
