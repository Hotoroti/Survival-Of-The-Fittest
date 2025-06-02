using UnityEngine;

public class DeadState : AnimalState
{
    const float DECAYDAYS = 10;
    float decayTimer = 0f;

    public DeadState(AnimalController controller, AnimalDNA dna, GameObject animalObject) : base(controller, dna, animalObject)
    {
    }

    public override void OnEnter()
    {
        Debug.Log("Entered deadstate");

        animalOBJ.transform.rotation = Quaternion.AngleAxis(90, Vector3.right) * animalOBJ.transform.rotation;

        decayTimer = 0f;
    }

    public override void OnExit()
    {
    }

    public override void OnUpdate()
    {
        // Increment timer
        decayTimer += TimeSettings.Instance.DeltaTime;

        if (decayTimer >= DECAYDAYS * TimeCycle.Instance.SecondsInDay)
        {
            GameObject.Destroy(animalOBJ);
        }
    }

    protected override void GetNewPosition()
    {
    }
}
