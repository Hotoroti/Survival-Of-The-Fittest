using UnityEngine;

public class LookingForMateState : AnimalState
{
    private Vector3 _newPos = Vector3.zero;

    public LookingForMateState(AnimalController controller, AnimalDNA dna, GameObject animalObject) : base(controller, dna, animalObject) { }

    public override void OnEnter()
    {
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
                GetNewPosition();
            else
                WalkTowardsIdleTarget();
        }
        else
        {
            GetNewPosition();
        }
    }

    /// <summary>
    /// Walk towards the mate
    /// </summary>
    private void HandleMatingApproach()
    {
        Vector3 matePosition = controller.MateOBJ.transform.position;
        WalkTowards(matePosition);

        if (!SlowMovement)
            controller.HungerConsumption(10f);

        if (ArrivedAtTarget(matePosition))
            controller.SwitchState(new MatingState(controller, dna, animalOBJ, controller.MateOBJ));
    }

    /// <summary>
    /// Walk around looking for mate
    /// </summary>
    private void WalkTowardsIdleTarget()
    {
        WalkTowards(_newPos);
        if (!SlowMovement)
            controller.HungerConsumption(10f);
    }

    private void OnAnimalEnter(GameObject animal)
    {
        AnimalController other = animal?.GetComponent<AnimalController>();
        if (!CanMateWith(other)) return;

        controller.MateOBJ = other;
        other.MateOBJ = controller;
    }

    protected override void GetNewPosition()
    {
        _newPos = animalOBJ.transform.position + Random.insideUnitSphere * dna.Chromosomes["Sense"];
        _newPos = new Vector3(_newPos.x, 0.1f, _newPos.z);
    }
}
