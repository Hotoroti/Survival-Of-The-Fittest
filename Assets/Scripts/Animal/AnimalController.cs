using System.Collections.Generic;
using UnityEngine;

public class AnimalController : MonoBehaviour
{
    [SerializeField] public AnimalDNA _dna;
    [SerializeField] private SphereCollider _senseCollider;

    private float _matureRate;
    private float _currentEnergy;
    private float _currentHunger;
    private float _baseEnergyConsumption;
    private bool _startMoving = false;

    private AnimalState _currentState = null;
    public float FitnessScore { get; private set; }
    public float HungerScore { get; private set; }

    public Stack<Vector3> _foodPositions { get; private set; } = new Stack<Vector3>();

    const float GROWTHFACTOR = 10f;

    private void Awake()
    {
        _dna.Initialise.AddListener(Initialise);
    }

    public void FixedUpdate()
    {
        if (!_startMoving)
            return;

        if (_currentState != null)
            _currentState.OnUpdate();

        FitnessScore += TimeSettings.Instance.DeltaTime;
    }

    /// <summary>
    /// Call this function when you want to switch the states of the Organism
    /// </summary>
    /// <param name="newState">The new state of the organism</param>
    public void SwitchState(AnimalState newState)
    {
        if (_currentState != null)
            _currentState.OnExit();

        _currentState = newState;
        _currentState.OnEnter();
    }

    /// <summary>
    /// Call this function when you want to remove energy from the organism
    /// </summary>
    /// <param name="consumption">The amount that will be removed from the energy</param>
    public void EnergyConsumption(float consumption)
    {
        _currentEnergy -= (_baseEnergyConsumption + consumption) * TimeSettings.Instance.DeltaTime;
    }

    /// <summary>
    /// Call this function when you want to remove hunger from the organism
    /// </summary>
    /// <param name="hungerConsumption">The amount to remove from the hunger</param>
    public void HungerConsumption(float hungerConsumption)
    {
        _currentHunger -= hungerConsumption * TimeSettings.Instance.DeltaTime;
        if (_currentHunger >= _dna.Hunger * .75f)
        {
            HungerScore = 0;
        }
        else if (_currentHunger >= _dna.Hunger * .5f)
        {
            HungerScore = .33f;
        }
        else if (_currentHunger >= _dna.Hunger * .25f)
        {
            HungerScore = .66f;
        }
        else
        {
            HungerScore = 1f;
        }
    }

    /// <summary>
    /// Call this function to initialise the controller values
    /// </summary>
    private void Initialise()
    {
        transform.localScale = new Vector3(_dna.Size * .25f, _dna.Size * .25f, _dna.Size * .25f);

        TimeCycle.Instance.DayFinished.AddListener(Mature);

        _matureRate = CalculateGrowthRate();
        _currentEnergy = _dna.Chromosomes["Energy"];
        _currentHunger = _dna.Hunger;
        _senseCollider.radius = _dna.Chromosomes["Sense"];

        _baseEnergyConsumption = _dna.Chromosomes["Sense"] / 10f;

        SwitchState(new RoamingState(this, _dna, gameObject));

        _startMoving = true;
        _dna.Initialise.RemoveListener(Initialise);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Food"))
        {
            _foodPositions.Push(other.transform.position);
            Debug.Log("Added food to memory. Food Count: " + _foodPositions.Count);
        }

        _currentState.OnTriggerEnter(other);
    }

    /// <summary>
    /// Call this function when you want to mature the organism
    /// </summary>
    private void Mature()
    {
        transform.localScale = new Vector3(transform.localScale.x + _matureRate, transform.localScale.y + +_matureRate, transform.localScale.z + +_matureRate);

        if (transform.localScale.x >= _dna.Size)
        {
            transform.localScale = new Vector3(_dna.Size, _dna.Size, _dna.Size);
            TimeCycle.Instance.DayFinished.RemoveListener(Mature);
        }
    }

    /// <summary>
    /// Call this function to calculate how big the mature steps are
    /// </summary>
    /// <returns>The size of the growth rate, the bigger the life the slower the rate</returns>
    private float CalculateGrowthRate()
    {
        return GROWTHFACTOR / _dna.Chromosomes["Life"];
    }

}
