using TMPro;
using UnityEngine;

public class TimeCycle_UI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;

    private void Start()
    {
        TimeCycle.Instance.DayFinished.AddListener(UpdateText);
        TimeCycle.Instance.WeekFinished.AddListener(UpdateText);
        TimeCycle.Instance.MonthFinished.AddListener(UpdateText);
        TimeCycle.Instance.YearFinished.AddListener(UpdateText);
        UpdateText();
    }

    void UpdateText()
    {
        _text.text = $"Day: {TimeCycle.Instance.Day} Week: {TimeCycle.Instance.Week} Month: {TimeCycle.Instance.Month} Year: {TimeCycle.Instance.Year} ";
    }
}
