using UnityEngine;

public class FoodController : MonoBehaviour
{
    [SerializeField] private float _decayTimeDays;
    private float _timeAlive;

    private void FixedUpdate()
    {
        IncreaseTime();

        if (_timeAlive >= _decayTimeDays * TimeCycle.Instance.SecondsInDay)
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Call this function to increase the time the food has been alive
    /// </summary>
    private void IncreaseTime()
    {
        _timeAlive += TimeSettings.Instance.DeltaTime;
    }
}
