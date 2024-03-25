using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(AnimalDNA))]
public class AnimalController : MonoBehaviour
{
    private AnimalState _currentState;
    public float CurrentLife { get; private set; }
    public AnimalDNA AnimalDNA { get; private set; }

    public FoodObject Food { get; set; }

    [HideInInspector] public NavMeshAgent Agent;

    private void Start()
    {
        AnimalDNA = GetComponent<AnimalDNA>();
        Agent = GetComponent<NavMeshAgent>();

        Agent.speed = AnimalDNA.Chromosomes[0];
        transform.localScale *= AnimalDNA.Chromosomes[1];
        CurrentLife = AnimalDNA.Chromosomes[2];

        _currentState = new AnimalWalking();

        if (_currentState != null)
            _currentState.StateEnter(this);
    }

    private void FixedUpdate()
    {
        if (_currentState != null)
            _currentState.StateUpdate();

        CurrentLife -= Time.fixedDeltaTime;
        if (CurrentLife <= 0)
        {
            AnimalManager.Instance.Animals.Remove(gameObject);
            Destroy(gameObject);
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

    private void OnTriggerEnter(Collider other)
    {
        if (_currentState.CurrentState == "LookingForFood" && !_currentState.DetectedFood && other.CompareTag("Food"))
        {
            _currentState.DetectedFood = true;
            Food = other.transform.parent.gameObject.GetComponent<FoodObject>();
            Agent.SetDestination(Food.transform.position);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (_currentState.CurrentState == "LookingForFood" && _currentState.DetectedFood && other.CompareTag("Food"))
        {

            ChangeState(new AnimalWalking());
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            ChangeState(new AnimalWalking());
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            ChangeState(new AnimalLookingForFood());
        }
    }
}
