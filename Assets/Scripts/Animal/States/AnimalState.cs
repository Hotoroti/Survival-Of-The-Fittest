using UnityEngine;

public abstract class AnimalState : MonoBehaviour
{
    protected AnimalController controller;

    public string CurrentState { get; protected set; }
    public bool DetectedFood { get; set; }

    public AnimalState(AnimalController animalController)
    {
        controller = animalController;
    }
    abstract public void OnStateEnter();

    abstract public void OnStateUpdate();

    abstract public void OnStateExit();
}
