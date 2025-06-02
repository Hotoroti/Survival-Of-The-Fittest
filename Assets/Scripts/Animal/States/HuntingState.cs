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
        controller.SenseCollider.OnHerbivoreAnimalEnter += OnPreyEnter;
        controller.SenseCollider.OnHerbivoreAnimalExit += OnPreyExit;
        controller.BodyCollider.OnHerbivoreContact += OnBodyEnter;
        controller.BodyCollider.OnHerbivoreExit += OnBodyExit;

        Debug.Log("Hunting State");
    }

    public override void OnExit()
    {
        controller.SenseCollider.OnHerbivoreAnimalEnter -= OnPreyEnter;
        controller.SenseCollider.OnHerbivoreAnimalExit -= OnPreyExit;
        controller.BodyCollider.OnHerbivoreContact -= OnBodyEnter;
        controller.BodyCollider.OnHerbivoreExit -= OnBodyExit;
    }

    public override void OnUpdate()
    {
        HandleLowEnergyMovement();
        ReplenishEnergyOnSlowMovement();

        if (Utils.IsOnGround(_targetPos))
        {
            if (_preyController == null)
            {
                HandleWandering();
            }
            else
            {
                HandleHunting();
            }
        }
        else
        {
            GetNewPosition();
        }
    }

    private void HandleWandering()
    {
        if (ArrivedAtTarget(_targetPos))
        {
            GetNewPosition();
            return;
        }

        WalkTowards(_targetPos);
        if (!SlowMovement)
        {
            controller.HungerConsumption(10f);
        }
    }

    private void HandleHunting()
    {
        Vector3 preyPosition = _preyController.gameObject.transform.position;

        if (_preyController.IsDead)
        {
            WalkTowards(preyPosition);
            if (!_slowMove)
            {
                controller.HungerConsumption(10f);
            }

            if (ArrivedAtTarget(preyPosition))
            {
                controller.SwitchState(new EatingState(controller, dna, animalOBJ, _preyController.gameObject));
            }
        }
        else
        {
            WalkTowards(preyPosition);
            if (!_slowMove)
            {
                controller.HungerConsumption(10f);
            }
        }
    }

    private void OnPreyEnter(GameObject food)
    {
        AnimalController tempPreyController = food.GetComponentInParent<AnimalController>();

        //Check if the prey has a controller
        if (tempPreyController == null)
            return;
        //Check if it found food
        if (_foundFood)
        {
            //Check if it found a dead animal
            if (tempPreyController.IsDead)
            {
                //Check if the dead animal is bigger then the old prey
                if (_preyController.IsDead && _preyController.Dna.Size > tempPreyController.Dna.Size)
                {
                    _preyController = tempPreyController;
                    _targetPos = food.transform.position;
                    _foundFood = true;
                }
                else
                {
                    _preyController = tempPreyController;
                    _targetPos = food.transform.position;
                    _foundFood = true;
                }

                return;
            }
            return;
        }

        //Check if the prey is dead
        if (tempPreyController.IsDead)
        {
            _preyController = tempPreyController;
            _targetPos = food.transform.position;
            _foundFood = true;

            return;
        }

        //Check if the prey is an amount larger then itself
        if (tempPreyController.Dna.Size > dna.Size * 1.25f)
            return;

        _preyController = tempPreyController;
        _targetPos = food.transform.position;
        _foundFood = true;
    }

    private void OnPreyExit(GameObject prey)
    {
        if (prey == null)
            return;

        if (_preyController != null)
        {
            if (prey != _preyController.gameObject)
                return;
        }

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
