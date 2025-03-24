using UnityEngine;

public class LookingForFoodState : AnimalState
{
    private Vector3 _targetPos = Vector3.zero;
    public LookingForFoodState(AnimalController controller, AnimalDNA dna, GameObject animalObject) : base(controller, dna, animalObject)
    {
    }

    public override void OnEnter()
    {
        Debug.Log("Food State");
    }

    public override void OnExit()
    {
    }

    public override void OnUpdate()
    {

    }

    public override void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Food"))
            Debug.Log("Found Food");
    }
}
