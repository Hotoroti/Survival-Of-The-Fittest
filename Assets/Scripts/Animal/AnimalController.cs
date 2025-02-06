using UnityEngine;

public class AnimalController : MonoBehaviour
{
    [SerializeField] private AnimalDNA _dna;

    private float _matureRate;
    private bool _startMoving = false;

    private AnimalState _currentState = null;
    public float FitnessScore { get; private set; }

    const float GROWTHFACTOR = 10f;

    private void Awake()
    {
        _dna.Initialise.AddListener(Initialise);
    }

    private void Initialise()
    {
        transform.localScale = new Vector3(_dna.Size * .25f, _dna.Size * .25f, _dna.Size * .25f);

        TimeCycle.Instance.DayFinished.AddListener(Mature);

        _matureRate = CalculateGrowthRate();

        SwitchState(new RoamingState(this, _dna, gameObject));

        _startMoving = true;
        _dna.Initialise.RemoveListener(Initialise);
    }

    public void FixedUpdate()
    {
        //Do not start anything until Values are initialised
        if (!_startMoving) { return; }

        if (_currentState != null)
            _currentState.OnUpdate();

        FitnessScore += TimeSettings.Instance.DeltaTime;
    }

    public void SwitchState(AnimalState newState)
    {
        if (_currentState != null)
            _currentState.OnExit();

        _currentState = newState;
        _currentState.OnEnter();
    }

    private void Mature()
    {
        transform.localScale = new Vector3(transform.localScale.x + _matureRate, transform.localScale.y + +_matureRate, transform.localScale.z + +_matureRate);

        if (transform.localScale.x >= _dna.Size)
        {
            transform.localScale = new Vector3(_dna.Size, _dna.Size, _dna.Size);
            TimeCycle.Instance.DayFinished.RemoveListener(Mature);
        }
    }

    private float CalculateGrowthRate()
    {
        return GROWTHFACTOR / _dna.Chromosomes["Life"];
    }

}
