using UnityEngine;

public class HuntingState : AnimalState
{
    private Vector3 _targetPos = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
    private AnimalController _preyController;
    private bool _foundFood = false;
    private bool _slowMove = false;

    public HuntingState(AnimalController controller, AnimalDNA dna, GameObject animalObject) : base(controller, dna, animalObject)
    {
    }

    public override void OnEnter()
    {
        controller.SenseCollider.OnHerbivoreAnimalEnter += OnFoodEnter;
        controller.SenseCollider.OnHerbivoreAnimalExit += OnFoodExit;
        controller.BodyCollider.OnHerbivoreContact += OnBodyEnter;
        controller.BodyCollider.OnHerbivoreExit += OnBodyExit;

        Debug.Log("Hunting State");
    }

    public override void OnExit()
    {
        controller.SenseCollider.OnHerbivoreAnimalEnter -= OnFoodEnter;
        controller.SenseCollider.OnHerbivoreAnimalExit -= OnFoodExit;
        controller.BodyCollider.OnHerbivoreContact -= OnBodyEnter;
        controller.BodyCollider.OnHerbivoreExit -= OnBodyExit;
    }

    public override void OnUpdate()
    {
        if (controller.CurrentEnergy < dna.Chromosomes["Energy"] * .25f)
        {
            currentMovementSpeed = _baseMovementSpeed * 0.5f;
            _slowMove = true;
        }

        if (_slowMove)
        {
            controller.ReplenishEnergy(50f);
            controller.HungerConsumption(5f);
            if (controller.CurrentEnergy >= dna.Chromosomes["Energy"] * .85f)
            {
                _slowMove = false;
                currentMovementSpeed = _baseMovementSpeed;
            }
        }

        if (Utils.IsOnGround(_targetPos))
        {
            if (_preyController == null)
            {
                if (ArrivedAtTarget(_targetPos))
                {
                    GetNewPosition();
                    Debug.Log("New Random Pos");
                    return;
                }
                else
                {
                    WalkTowards(_targetPos);
                    if (!_slowMove)
                        controller.HungerConsumption(10);
                }
            }
            else
            {
                if (_preyController.IsDead)
                {
                    WalkTowards(_preyController.gameObject.transform.position);
                    if (!_slowMove)
                        controller.HungerConsumption(10);

                    if (ArrivedAtTarget(_preyController.gameObject.transform.position))
                    {
                        controller.SwitchState(new EatingState(controller, dna, animalOBJ, _preyController.gameObject));
                    }
                }
                else
                {
                    WalkTowards(_preyController.gameObject.transform.position);
                    Debug.Log("Hunting");
                    if (!_slowMove)
                        controller.HungerConsumption(10f);
                }
            }

        }
        else
        {
            GetNewPosition();
        }
    }

    private void OnFoodEnter(GameObject food)
    {
        if (_foundFood)
            return;

        AnimalController foodController = food.GetComponentInParent<AnimalController>();

        if (foodController == null)
            return;

        if (foodController.Dna.Size > dna.Size * 1.25f)
            return;

        _preyController = foodController;
        _targetPos = food.transform.position;
        _foundFood = true;
    }

    private void OnFoodExit(GameObject food)
    {
        if (food == null)
            return;


        if (food != _preyController?.gameObject)
            return;

        _preyController = null;
        _foundFood = false;
    }

    private void OnBodyEnter(AnimalController animal)
    {
        if (animal == null)
            return;

        animal.RemoveLifeValue(Settings.Instance.BaseDamage * (dna.Size * 1.5f));

        currentMovementSpeed = 0;
    }

    private void OnBodyExit(AnimalController animal)
    {
        currentMovementSpeed = _baseMovementSpeed;
    }

    /// <summary>
    /// Call this function to get a new position depending on if it knows a position where food was or in the sense radius
    /// </summary>
    private void GetNewPosition()
    {
        if (!_foundFood)
        {
            _targetPos = (animalOBJ.transform.position + Random.insideUnitSphere * dna.Chromosomes["Sense"]);
            _targetPos = new Vector3(_targetPos.x, 0.1f, _targetPos.z);
        }

    }
}
