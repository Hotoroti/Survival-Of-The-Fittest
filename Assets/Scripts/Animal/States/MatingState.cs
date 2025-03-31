using UnityEngine;

public class MatingState : AnimalState
{
    public MatingState(AnimalController controller, AnimalDNA dna, GameObject animalObject) : base(controller, dna, animalObject)
    {
    }

    public override void OnEnter()
    {
        Debug.Log("Entered Mating State Stop Moving");
    }

    public override void OnExit()
    {
    }

    public override void OnUpdate()
    {
    }
}
