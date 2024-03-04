using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(AnimalDNA))]
public class AnimalController : MonoBehaviour
{
    private AnimalDNA _animalDNA;

    private AnimalState _currentState;

    private float _life;

    [HideInInspector] public NavMeshAgent Agent;

    private void Start()
    {
        _animalDNA = GetComponent<AnimalDNA>();
        Agent = GetComponent<NavMeshAgent>();

        Agent.speed = _animalDNA.Chromosomes[0];
        transform.localScale *= _animalDNA.Chromosomes[1];
        _life = _animalDNA.Chromosomes[2];

        _currentState = new AnimalLookingForFood(this);

        _currentState.OnStateEnter();
    }

    private void FixedUpdate()
    {
        _currentState.OnStateUpdate();
    }

    public void ChangeState(AnimalState state)
    {
        _currentState.OnStateExit();
        _currentState = state;
        _currentState.OnStateEnter();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            ChangeState(new AnimalWalking(this));
        }
    }
}
