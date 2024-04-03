using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(AnimalDNA))]
public class AnimalController : MonoBehaviour
{
    private AnimalState _currentState;
    public float CurrentLife { get; private set; }
    public AnimalDNA AnimalDNA { get; private set; }

    public FoodObject Food { get; set; }
    public AnimalController Mate { get; set; }

    [HideInInspector] public NavMeshAgent Agent;

    public float MateTime = 10f;

    private void Start()
    {
        AnimalDNA = GetComponent<AnimalDNA>();
        Agent = GetComponent<NavMeshAgent>();

        Agent.speed = AnimalDNA.Chromosomes[0];
        transform.localScale *= AnimalDNA.Chromosomes[1];
        CurrentLife = AnimalDNA.Chromosomes[2];

        ResetValues();

        _currentState = new AnimalWalking();

        if (_currentState != null)
            _currentState.StateEnter(this);
    }

    private void FixedUpdate()
    {
        if (_currentState != null)
            _currentState.StateUpdate();

        //CurrentLife -= Time.fixedDeltaTime * ((AnimalDNA.Chromosomes[0] / 1) / 2) ;
        if (CurrentLife <= 0)
        {
            AnimalManager.Instance.Animals.Remove(gameObject);
            Destroy(gameObject);
        }

        if (AnimalDNA.Chromosomes[4] == 0 && !_currentState.DetectedMate)
        {
            MateTime -= Time.fixedDeltaTime;
            if (MateTime <= 0)
            {
                ChangeState(new AnimalLookingForMate());
            }
        }
    }

    public void ChangeState(AnimalState state)
    {
        if (_currentState != null)
            _currentState.StateExit();

        _currentState = state;
        _currentState.StateEnter(this);
    }

    public void ResetLife()
    {
        CurrentLife = AnimalDNA.Chromosomes[2];
    }

    private void ResetValues()
    {
        ResetLife();
        Food = null;
        Mate = null;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_currentState.CurrentState == "LookingForFood" && !_currentState.DetectedFood && other.CompareTag("Food"))
        {
            _currentState.DetectedFood = true;
            Food = other.transform.parent.gameObject.GetComponent<FoodObject>();
            Agent.SetDestination(Food.transform.position);
        }

        if (_currentState.CurrentState == "LookingForMate" && !_currentState.DetectedMate && other.CompareTag("Animal"))
        {
            if (AnimalManager.Instance.FemaleAnimalsToMate.Contains(other.gameObject))
            {
                _currentState.DetectedMate = true;
                AnimalManager.Instance.FemaleAnimalsToMate.Remove(other.gameObject);
                Mate = other.transform.gameObject.GetComponent<AnimalController>();
                Mate.Mate = this;
                Mate.Agent.isStopped = true;
                transform.position = Mate.transform.position;
                //Agent.SetDestination(other.transform.position);
                Invoke("InstantiateChild", 5f);
            }

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (_currentState.CurrentState == "LookingForFood" && _currentState.DetectedFood && other.CompareTag("Food"))
        {
            ChangeState(new AnimalWalking());
        }

        if (_currentState.CurrentState == "LookingForMate" && _currentState.DetectedMate && other.CompareTag("Animal"))
        {
            ChangeState(new AnimalLookingForMate());
        }
    }

    public void InstantiateChild()
    {
        Instantiate(AnimalManager.Instance.AnimalObject, transform.position, Quaternion.identity, transform);

        MateTime = 30f;
        _currentState.DetectedMate = false;

        Agent.isStopped = false;
        ChangeState(new AnimalWalking());

        if (Mate == null) return;
        Mate.Agent.isStopped = false;        
        Mate.ChangeState(new AnimalWalking());


        HasMated(Mate);   
        Mate = null;
        //AnimalManager.Instance.FemaleAnimalsToMate.Add(Mate.gameObject);

    }

    public void HasMated(AnimalController other)
    {
        other.Mate = null;
        AnimalManager.Instance.FemaleAnimalsToMate.Add(other.gameObject);
    }
}
