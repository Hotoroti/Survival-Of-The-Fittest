using UnityEngine;

public class SpeedUpGame : MonoBehaviour
{
    [SerializeField] private bool _speedUp;
    [SerializeField] private int _speed;
    private void FixedUpdate()
    {
        if (!_speedUp) return;

        Time.timeScale = _speed;
    }
}
