using UnityEngine;

public class FoodController : MonoBehaviour
{
    [SerializeField] private float _decayTime;
    private float _timeAlive;

    private void FixedUpdate()
    {
        IncreaseTime();

        if (_timeAlive >= _decayTime)
        {
            Destroy(gameObject);
        }
    }

    private void IncreaseTime()
    {
        _timeAlive += TimeSettings.Instance.DeltaTime;
    }
}
