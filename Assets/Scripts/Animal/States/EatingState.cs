using UnityEngine;

public class EatingState : AnimalState
{
    private GameObject _foodObj;

    public EatingState(AnimalController controller, AnimalDNA dna, GameObject animalObject, GameObject foodObject) : base(controller, dna, animalObject)
    {
        _foodObj = foodObject;
    }

    public override void OnEnter()
    {
        Debug.Log("Enter Eating State");
    }

    public override void OnExit()
    {
    }

    public override void OnTriggerEnter(Collider other)
    {
    }

    public override void OnUpdate()
    {
    }

    private void Eating()
    {

    }

    private void FinishedEating()
    {

    }
}
