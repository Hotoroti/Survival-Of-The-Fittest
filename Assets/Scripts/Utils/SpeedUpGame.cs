using UnityEngine;

public class SpeedUpGame : MonoBehaviour
{
    [SerializeField] private bool _speedUp;
    [SerializeField] private int _speed;

    private int _currentSpeed;
    private void Update()
    {
        if (!_speedUp)
            _currentSpeed = 1;
        else
            _currentSpeed = _speed;

        if (_currentSpeed <= 0)
            _currentSpeed = 1;
        

        Time.timeScale = _currentSpeed;
    }
}
