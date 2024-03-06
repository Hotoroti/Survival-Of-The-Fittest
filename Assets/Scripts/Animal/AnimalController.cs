using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(AnimalDNA))]
public class AnimalController : MonoBehaviour
{
    private AnimalState _currentState;
    public float CurrentLife { get; private set; }
    public AnimalDNA AnimalDNA { get; private set; }

    [HideInInspector] public NavMeshAgent Agent;

    private void Start()
    {
        AnimalDNA = GetComponent<AnimalDNA>();
        Agent = GetComponent<NavMeshAgent>();

        Agent.speed = AnimalDNA.Chromosomes[0];
        transform.localScale *= AnimalDNA.Chromosomes[1];
        CurrentLife = AnimalDNA.Chromosomes[2];

        _currentState = new AnimalWalking(this);

        _currentState.OnStateEnter();
    }

    private void FixedUpdate()
    {
        _currentState.OnStateUpdate();
        CurrentLife -= Time.fixedDeltaTime;
        if (CurrentLife <= 0)
        {
            AnimalManager.Instance.Animals.Remove(gameObject);
            Destroy(gameObject);
        }
    }

    public void ChangeState(AnimalState state)
    {
        _currentState.OnStateExit();
        _currentState = state;
        _currentState.OnStateEnter();
    }

    public void ResetLife()
    {
        CurrentLife = AnimalDNA.Chromosomes[2];
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            ChangeState(new AnimalWalking(this));
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            ChangeState(new AnimalLookingForFood(this));
        }
    }
}
