using UnityEngine;

public class FoodSpawn : MonoBehaviour
{
    [SerializeField] private float _foodGenerateTimeMinDays = 7, _foodGenerateTimeMaxDays = 21;
    [SerializeField] private float _foodSpawnRadius = 2;

    [SerializeField] private GameObject _foodObj = null;
    private float _timeUntilSpawn = 0f;
    private float _foodGenerateTime;

    const int MAX_ATTEMPT = 10;

    /// <summary>
    /// Call this function to generate food
    /// </summary>
    [ContextMenu("Create food")]
    private void GenerateFood()
    {
        bool validPos = false;
        Vector3 foodPos = Vector3.zero;
        for (int attempt = 0; attempt < MAX_ATTEMPT; attempt++)
        {
            Vector2 randomPos = Random.insideUnitCircle * _foodSpawnRadius;
            foodPos = new Vector3(transform.position.x + randomPos.x, transform.position.y + .1f, transform.position.z + randomPos.y);
            if (Utils.IsOnGround(foodPos))
            {
                validPos = true;
                break;
            }
        }

        if (validPos)
        {
            Instantiate(_foodObj, foodPos, Quaternion.identity, EnvironmentMaker.Instance.FoodParent.transform);
            _foodGenerateTime = Random.Range(_foodGenerateTimeMinDays, _foodGenerateTimeMaxDays);
        }
        else
        {
            Debug.Log("Could not spawn food, because it is not on the ground");
        }
    }

    /// <summary>
    /// Call this function to increase the time until some new food can be spawned
    /// </summary>
    private void IncreaseTime()
    {
        _timeUntilSpawn += TimeSettings.Instance.DeltaTime;
    }

    void Start()
    {
        if (Random.Range(0, 10) % 3 == 1)
        {
            GenerateFood();
        }
        else
        {
            _foodGenerateTime = Random.Range(_foodGenerateTimeMinDays, _foodGenerateTimeMaxDays);
        }
    }

    private void FixedUpdate()
    {
        IncreaseTime();

        if (_timeUntilSpawn >= _foodGenerateTime * TimeCycle.Instance.SecondsInDay)
        {
            _timeUntilSpawn = 0;
            GenerateFood();

        }
    }
}
