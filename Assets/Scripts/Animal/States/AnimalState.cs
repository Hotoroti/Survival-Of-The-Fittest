using UnityEngine;

public abstract class AnimalState
{
    protected AnimalController controller;

    public string CurrentState { get; protected set; }
    public bool DetectedFood { get; set; }
    public FoodObject Food { get; set; }

    public void StateEnter(AnimalController ac)
    {
        controller = ac;
        OnEnter();
    }

    abstract public void OnEnter();

    public void StateUpdate()
    {
        OnUpdate();
    }
    abstract public void OnUpdate();

    public void StateExit()
    {
        OnExit();
    }
    abstract public void OnExit();

    protected void WalkTarget()
    {
        int iPoint = Random.Range(0, GenerateGrid.Instance.GridCells.Count);
        controller.Agent.SetDestination(GenerateGrid.Instance.GridCells[iPoint].transform.position);
    }
}
