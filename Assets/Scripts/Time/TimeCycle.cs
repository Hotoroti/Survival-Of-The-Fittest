using UnityEngine;
using UnityEngine.Events;

public class TimeCycle : MonoBehaviour
{
    [SerializeField] private int _secondsInDay, _daysInWeek, _weeksInMonth, _monthsInYear;

    private float _seconds;

    public int Day { get; private set; }
    public int Week { get; private set; }
    public int Month { get; private set; }
    public int Year { get; private set; }

    public UnityEvent DayFinished { get; private set; } = new UnityEvent();
    public UnityEvent WeekFinished { get; private set; } = new UnityEvent();
    public UnityEvent MonthFinished { get; private set; } = new UnityEvent();
    public UnityEvent YearFinished { get; private set; } = new UnityEvent();

    public static TimeCycle Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(Instance);
            Debug.LogError("More then one Instance of the TimeCycle, destroy it");
        }
        else
        {
            Instance = this;
        }

        Day = 1;
        Week = 1;
        Month = 1;
        Year = 1;
    }

    private void FixedUpdate()
    {
        _seconds += TimeSettings.Instance.DeltaTime;

        if (_seconds > _secondsInDay)
        {
            Day++;
            _seconds = 0;
            DayFinished?.Invoke();
        }

        if (Day > _daysInWeek)
        {
            Week++;
            Day = 1;
            WeekFinished?.Invoke();
        }

        if (Week > _weeksInMonth)
        {
            Month++;
            Week = 1;
            MonthFinished?.Invoke();
        }

        if (Month > _monthsInYear)
        {
            Year++;
            Month = 1;
            YearFinished?.Invoke();
        }


    }
}
