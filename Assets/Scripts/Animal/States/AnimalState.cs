using UnityEngine;

public abstract class AnimalState
{
    protected AnimalController controller;
    protected AnimalDNA dna;
    protected GameObject animalOBJ;

    private float _baseMovementSpeed;
    private float _currentMovementSpeed;

    public AnimalState(AnimalController controller,
        AnimalDNA dna,
        GameObject animalObject) : base()
    {
        this.controller = controller;
        this.dna = dna;
        this.animalOBJ = animalObject;

        _baseMovementSpeed = dna.Chromosomes["WalkingSpeed"] / (dna.Size * 2);
        _currentMovementSpeed = _baseMovementSpeed;
    }

    protected void WalkTowards(Vector3 target)
    {
        animalOBJ.transform.position = Vector3.MoveTowards(animalOBJ.transform.position, target, _currentMovementSpeed * TimeSettings.Instance.DeltaTime);
        animalOBJ.transform.LookAt(target);
    }

    protected bool ArrivedAtTarget(Vector3 target)
    {
        return Vector3.Distance(animalOBJ.transform.position, target) <= .5f;
    }

    public abstract void OnEnter();

    public abstract void OnUpdate();

    public abstract void OnExit();
}
