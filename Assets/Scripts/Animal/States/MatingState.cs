using UnityEngine;

public class MatingState : AnimalState
{
    private float _matingTimer;
    private AnimalController _mateController;

    public MatingState(AnimalController controller, AnimalDNA dna, GameObject animalObject, AnimalController mateController)
        : base(controller, dna, animalObject)
    {
        _mateController = mateController;
    }

    public override void OnEnter()
    {
        _matingTimer = 0f;
    }

    public override void OnExit() { }

    public override void OnUpdate()
    {
        ReplenishDuringMating();
        UpdateMatingTimer();

        if (IsMatingTimeComplete())
            CompleteMating();
    }

    /// <summary>
    /// Handles replenishing eneergy during mating
    /// </summary>
    private void ReplenishDuringMating()
    {
        controller.ReplenishEnergy(15f);
    }

    /// <summary>
    /// Update the timer during mating
    /// </summary>
    private void UpdateMatingTimer()
    {
        _matingTimer += TimeSettings.Instance.DeltaTime;
    }

    /// <summary>
    /// Checks if the mating is complete
    /// </summary>
    /// <returns>True or false</returns>
    private bool IsMatingTimeComplete()
    {
        return _matingTimer >= Settings.Instance.MateTime;
    }

    /// <summary>
    /// Handles the finishing of the mating
    /// </summary>
    private void CompleteMating()
    {
        if (IsFemale())
            SpawnChild(_mateController);

        _mateController.HadMated();
        controller.HadMated();
    }

    /// <summary>
    /// Spawn the child with the correct values
    /// </summary>
    /// <param name="otherParent">The other parent of the child</param>
    private void SpawnChild(AnimalController otherParent)
    {
        GameObject child = GameObject.Instantiate(
            Settings.Instance.AnimalObject,
            controller.transform.position,
            Quaternion.identity,
            Settings.Instance.AnimalParent.transform
        );

        AnimalDNA childDna = child.GetComponent<AnimalDNA>();
        if (childDna == null)
        {
            Debug.LogError("Child DNA component missing!");
            return;
        }

        float lifeValue = Utils.GetInheritedGene("Life", otherParent, controller);
        float speedValue = Utils.GetInheritedGene("WalkingSpeed", otherParent, controller);
        float senseValue = Utils.GetInheritedGene("Sense", otherParent, controller);
        float energyValue = Utils.GetInheritedGene("Energy", otherParent, controller);

        Utils.ApplyRandomMutation(ref lifeValue, ref speedValue, ref senseValue, ref energyValue);

        childDna.SetChromosomes(lifeValue, speedValue, senseValue, energyValue, dna.Carnivore);
    }

    protected override void GetNewPosition() { }
}
