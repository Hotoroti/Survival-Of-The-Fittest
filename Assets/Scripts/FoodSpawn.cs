using UnityEngine;

public class FoodSpawn : MonoBehaviour
{
    [SerializeField] private float _foodGenerateTimeMin = 50, _foodGenerateTimeMax = 500;
    [SerializeField] private float _foodSpawnRadius = 2;

    [SerializeField] private GameObject _foodObj = null;
    private float _timeUntilSpawn = 0f;
    private float _foodGenerateTime;

    const int MAX_ATTEMPT = 10;

    [ContextMenu("Create food")]
    private void GenerateFood()
    {
        bool validPos = false;
        Vector3 foodPos = Vector3.zero;
        for (int attempt = 0; attempt < MAX_ATTEMPT; attempt++)
        {
            Vector2 randomPos = Random.insideUnitCircle * _foodSpawnRadius;
            foodPos = new Vector3(transform.position.x + randomPos.x, transform.position.y + .1f, transform.position.z + randomPos.y);
            if (Physics.Raycast(foodPos, Vector3.down, 1f, LayerMask.GetMask("Ground")))
            {
                validPos = true;
                break;
            }
        }

        if (validPos)
        {
            Instantiate(_foodObj, foodPos, Quaternion.identity, transform);
            _foodGenerateTime = Random.Range(_foodGenerateTimeMin, _foodGenerateTimeMax);
        }
        else
        {
            Debug.Log("Could not spawn food, because it is not on the ground");
        }
    }

    private void IncreaseTime()
    {
        _timeUntilSpawn += TimeSettings.Instance.DeltaTime;
    }

    void Start()
    {
        if (Random.Range(0, 10) % 3 == 1)
            GenerateFood();
    }

    private void FixedUpdate()
    {
        IncreaseTime();

        if (_timeUntilSpawn >= _foodGenerateTime)
        {
            _timeUntilSpawn = 0;
            GenerateFood();

        }
    }
}
