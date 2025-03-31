using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AnimalController : MonoBehaviour
{
    [SerializeField] public AnimalDNA Dna;
    [SerializeField] private SphereCollider _senseCollider;
    [SerializeField] private MeshRenderer _renderer;

    private float _matureRate;
    private float _currentEnergy;
    private float _currentHunger;
    private float _baseEnergyConsumption;
    private float _currentMateRate;
    private bool _startMoving = false;

    private AnimalState _currentState = null;
    public float FitnessScore { get; private set; }
    public float HungerScore { get; private set; }
    public bool HasMatured { get; private set; } = false;
    public bool ReadyToMate { get; private set; } = false;


    public AnimalController MateOBJ = null;
    public SenseColliderScript SenseCollider;
    public Stack<Vector3> _foodPositions { get; private set; } = new Stack<Vector3>();

    const float GROWTHFACTOR = 10f;

    private void Awake()
    {
        Dna.Initialise.AddListener(Initialise);

        SenseCollider.OnFoodEnter += OnFoodEnter;
    }

    public void FixedUpdate()
    {
        if (!_startMoving)
            return;

        if (_currentState != null)
            _currentState.OnUpdate();

        FitnessScore += TimeSettings.Instance.DeltaTime;

        if (HasMatured)
        {
            _currentMateRate += TimeSettings.Instance.DeltaTime;
            ReadyToMate = _currentMateRate >= Dna.ReproductionRate;
        }
    }

    /// <summary>
    /// Call this function when you want to recharge the hunger stats
    /// </summary>
    public void RechargeHunger()
    {
        HungerScore = 0;
        _currentHunger = Dna.Hunger;
    }

    /// <summary>
    /// Call this function when you want to replenish energy
    /// </summary>
    /// <param name="replenishRate">The amount it needs to replenish per second</param>
    public void ReplenishEnergy(float replenishRate)
    {
        var currentRate = _currentEnergy / Dna.Chromosomes["Energy"] + replenishRate * TimeSettings.Instance.DeltaTime;
        _currentEnergy = Mathf.Lerp(_currentEnergy, Dna.Chromosomes["Energy"], replenishRate * TimeSettings.Instance.DeltaTime);
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
        if (_currentHunger >= Dna.Hunger * .75f)
        {
            HungerScore = 0;
        }
        else if (_currentHunger >= Dna.Hunger * .5f)
        {
            HungerScore = .33f;
        }
        else if (_currentHunger >= Dna.Hunger * .25f)
        {
            HungerScore = .66f;
        }
        else
        {
            HungerScore = 1f;
        }
    }

    public void HadMated()
    {
        _currentMateRate = 0f;
        SwitchState(new RoamingState(this, Dna, gameObject));
    }
    /// <summary>
    /// Call this function to initialise the controller values
    /// </summary>
    private void Initialise()
    {
        transform.localScale = new Vector3(Dna.Size * .25f, Dna.Size * .25f, Dna.Size * .25f);

        TimeCycle.Instance.DayFinished.AddListener(Mature);

        _matureRate = CalculateGrowthRate();
        _currentEnergy = Dna.Chromosomes["Energy"];
        _currentHunger = Dna.Hunger;
        _senseCollider.radius = Dna.Chromosomes["Sense"];

        _baseEnergyConsumption = Dna.Chromosomes["Sense"] / 10f;

        if (Dna.Chromosomes.Count == 0)
            return;

        var highestChromosome = Dna.Chromosomes.OrderByDescending(pair => pair.Value).First();
        string highestTrait = highestChromosome.Key;
        float highestValue = highestChromosome.Value;

        float normalizedValue = Mathf.Clamp01(highestValue / 1000f);

        Color genderTint = Dna.Gender switch
        {
            1 => new Color(0.3f, 0.3f, 1.0f), //More blue for males
            0 => new Color(1.0f, 0.3f, 0.3f) //More red for female
        };

        Color traitColor = highestTrait switch
        {
            "Life" => new Color(0, normalizedValue, 0),        // Green
            "WalkingSpeed" => new Color(0, 0, normalizedValue),       // Blue
            "Sense" => new Color(normalizedValue, 0, normalizedValue), // Purple
            "Energy" => new Color(0, normalizedValue, normalizedValue), // Cyan
            _ => _renderer.material.color // Default if trait is unknown
        };

        _renderer.material.color = traitColor * 0.7f + genderTint * 0.3f; // 70% trait, 30% gender influence

        SwitchState(new RoamingState(this, Dna, gameObject));

        _startMoving = true;
        Dna.Initialise.RemoveListener(Initialise);
    }

    private void OnFoodEnter(GameObject food)
    {
        _foodPositions.Push(food.transform.position);
        Debug.Log("Enterd food into stack");
    }


    /// <summary>
    /// Call this function when you want to mature the organism
    /// </summary>
    private void Mature()
    {
        transform.localScale = new Vector3(transform.localScale.x + _matureRate, transform.localScale.y + +_matureRate, transform.localScale.z + +_matureRate);

        if (transform.localScale.x >= Dna.Size)
        {
            transform.localScale = new Vector3(Dna.Size, Dna.Size, Dna.Size);
            HasMatured = true;
            TimeCycle.Instance.DayFinished.RemoveListener(Mature);
        }
    }

    /// <summary>
    /// Call this function to calculate how big the mature steps are
    /// </summary>
    /// <returns>The size of the growth rate, the bigger the life the slower the rate</returns>
    private float CalculateGrowthRate()
    {
        return GROWTHFACTOR / Dna.Chromosomes["Life"];
    }

}
