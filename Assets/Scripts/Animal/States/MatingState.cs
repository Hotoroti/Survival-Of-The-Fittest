using UnityEngine;

public class MatingState : AnimalState
{
    private float _matingTimer;
    private AnimalController _mateController;
    public MatingState(AnimalController controller, AnimalDNA dna, GameObject animalObject, AnimalController mateController) : base(controller, dna, animalObject)
    {
        _mateController = mateController;
    }

    public override void OnEnter()
    {

    }

    public override void OnExit()
    {
    }

    public override void OnUpdate()
    {
        if (dna.Gender != 0)
            return;
        controller.ReplenishEnergy(15f);

        _matingTimer += TimeSettings.Instance.DeltaTime;

        if (_matingTimer < Settings.Instance.MateTime)
            return;

        SpawnChild(_mateController);

        _mateController.HadMated();
        controller.HadMated();
    }

    private void SpawnChild(AnimalController otherParent)
    {
        GameObject child = GameObject.Instantiate(Settings.Instance.AnimalObject, controller.transform.position, Quaternion.identity, Settings.Instance.AnimalParent.transform);

        AnimalDNA childDna = child.GetComponent<AnimalDNA>();

        if (childDna == null)
        {
            Debug.LogError("Child does not have DNA");
            return;
        }

        float lifeValue = Random.Range(0, 100) < 50 ? dna.Chromosomes["Life"] : otherParent.Dna.Chromosomes["Life"];
        float speedValue = Random.Range(0, 100) < 50 ? dna.Chromosomes["WalkingSpeed"] : otherParent.Dna.Chromosomes["WalkingSpeed"];
        float senseValue = Random.Range(0, 100) < 50 ? dna.Chromosomes["Sense"] : otherParent.Dna.Chromosomes["Sense"];
        float energyValue = Random.Range(0, 100) < 50 ? dna.Chromosomes["Energy"] : otherParent.Dna.Chromosomes["Energy"];

        if (Random.Range(0, 100) < Settings.Instance.MutationChangeMax)
        {
            float mutationValue = Random.Range(0, 2) < 1 ? Settings.Instance.MutationMultiplier.x : Settings.Instance.MutationMultiplier.y;
            switch (Random.Range(0, 4))
            {
                case 0:
                    lifeValue *= mutationValue;
                    break;
                case 1:
                    speedValue *= mutationValue;
                    break;
                case 2:
                    senseValue *= mutationValue;
                    break;
                case 3:
                    energyValue *= mutationValue;
                    break;
            }
        }

        childDna.SetChromosomes(lifeValue, speedValue, senseValue, energyValue, dna.Carnivore);
    }
}
