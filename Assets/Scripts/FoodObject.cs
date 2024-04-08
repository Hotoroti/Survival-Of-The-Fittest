using UnityEngine;

public class FoodObject : MonoBehaviour
{
    [SerializeField] private GameObject _foodObj;

    private NewFoodManager _foodManager;
    public bool IsFood { get; set; }

    private void Start()
    {
        _foodManager = GetComponentInParent<NewFoodManager>();
        ShowFood();
    }

    public void ShowFood()
    {
        if (IsFood)
            _foodObj.SetActive(true);
        else
            _foodObj.SetActive(false);
    }

    [ContextMenu("FoodHasBeenEaten")]
    public void HasBeenEaten()
    {
        IsFood = false;
        ShowFood();
        _foodManager.GenerateNewFood(this);
    }
}
