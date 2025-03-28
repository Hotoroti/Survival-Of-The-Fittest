using UnityEngine;

public class LookingForFoodState : AnimalState
{
    private Vector3 _targetPos = Vector3.zero;
    private GameObject _foodObj;
    private bool _foundFood = false;
    public LookingForFoodState(AnimalController controller, AnimalDNA dna, GameObject animalObject) : base(controller, dna, animalObject)
    {
    }

    public override void OnEnter()
    {
        Debug.Log(controller._foodPositions.Count);
        if (controller._foodPositions.Count > 0)
        {
            _targetPos = controller._foodPositions.Pop();
        }
    }

    public override void OnExit()
    {
    }

    public override void OnUpdate()
    {
        if (Utils.IsOnGround(_targetPos))
        {
            if (ArrivedAtTarget(_targetPos))
            {
                if (_foundFood)
                {
                    controller.SwitchState(new EatingState(controller, controller._dna, controller.gameObject, _foodObj));
                }
                else
                {
                    GetNewPosition();
                }
            }
            else
            {
                WalkTowards(_targetPos);
                //controller.HungerConsumption(10);
            }

        }
        else
        {
            Debug.Log(Utils.IsOnGround(_targetPos));
            GetNewPosition();
        }
    }

    public override void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Food"))
        {
            _targetPos = other.transform.position;
            _foodObj = other.gameObject;
            _foundFood = true;
        }
    }

    /// <summary>
    /// Call this function to get a new position depending on if it knows a position where food was or in the sense radius
    /// </summary>
    private void GetNewPosition()
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
