using UnityEngine;

public abstract class AnimalState : MonoBehaviour
{
    protected AnimalController controller;

    public AnimalState(AnimalController animalController)
    {
        controller = animalController;
    }
    abstract public void OnStateEnter();

    abstract public void OnStateUpdate();

    abstract public void OnStateExit();
}
