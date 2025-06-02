using UnityEngine;

public class LookingForFoodState : AnimalState
{
    private Vector3 _targetPos = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
    private GameObject _foodObj;
    private bool _foundFood = false;
    private bool _slowMove = false;
    public LookingForFoodState(AnimalController controller, AnimalDNA dna, GameObject animalObject) : base(controller, dna, animalObject)
    {
    }

    public override void OnEnter()
    {
        if (controller._foodPositions.Count > 0)
        {
            _targetPos = controller._foodPositions.Pop();
        }

        controller.SenseCollider.OnFoodEnter += OnFoodEnter;
    }

    public override void OnExit()
    {
        controller.SenseCollider.OnFoodEnter -= OnFoodEnter;
    }

    public override void OnUpdate()
    {
        HandleLowEnergyMovement();
        ReplenishEnergyOnSlowMovement();

        if (Utils.IsOnGround(_targetPos))
        {
            if (ArrivedAtTarget(_targetPos))
            {
                HandleFoodArrival();
            }
            else
            {
                WalkTowardsWithHungerConsumption(_targetPos);
            }
        }
        else
        {
            HandleInvalidGroundTarget();
        }
    }

    private void HandleFoodArrival()
    {
        if (_foundFood)
        {
            if (_foodObj != null && _foodObj.gameObject != null)
            {
                controller.SwitchState(new EatingState(controller, dna, animalOBJ, _foodObj));
            }
            else
            {
                GetNewPosition();
            }
        }
        else
        {
            GetNewPosition();
        }
    }

    private void HandleInvalidGroundTarget()
    {
        Debug.Log(Utils.IsOnGround(_targetPos));
        GetNewPosition();
    }

    public void OnFoodEnter(GameObject food)
    {
        float distanceNewFood = (controller.transform.position - food.transform.position).sqrMagnitude;
        float oldFood = _foundFood ? (controller.transform.position - _targetPos).sqrMagnitude : float.MaxValue;

        if (distanceNewFood < oldFood)
        {
            _targetPos = food.transform.position;
            _foodObj = food.gameObject;
            _foundFood = true;
        }

    }

    protected override void GetNewPosition()
    {
        if (controller._foodPositions.Count <= 0)
        {
            _targetPos = (animalOBJ.transform.position + Random.insideUnitSphere * dna.Chromosomes["Sense"]);
            _targetPos = new Vector3(_targetPos.x, 0.1f, _targetPos.z);
        }
        else
            _targetPos = controller._foodPositions.Pop();
    }
}
