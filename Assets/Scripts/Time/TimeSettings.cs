using UnityEngine;

public class TimeSettings : MonoBehaviour
{
    private float _timeStep = 1;
    public float DeltaTime { get; private set; }

    public static TimeSettings Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(Instance);
            Debug.LogError("More then one Instance of the TimeSettings, destroy all");
        }
        else
        {
            Instance = this;
        }
    }

    public void IncreaseSpeed(float speed)
    {
        _timeStep = speed;
    }

    private void FixedUpdate()
    {
        DeltaTime = _timeStep * Time.deltaTime;
    }
}
