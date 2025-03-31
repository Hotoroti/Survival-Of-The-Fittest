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
        /*if (controller.MateOBJ == null)
            GetNewPosition();*/
    }

    public override void OnExit()
    {
    }

    public override void OnUpdate()
    {
    }

    /// <summary>
    /// Call this function to get a new position in the sense radius
    /// </summary>
    private void GetNewPosition()
    {
        _newPos = (animalOBJ.transform.position + Random.insideUnitSphere * dna.Chromosomes["Sense"]);
        _newPos = new Vector3(_newPos.x, 0.1f, _newPos.z);
    }

}
