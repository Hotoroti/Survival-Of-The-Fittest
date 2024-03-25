using UnityEngine;

public class FoodObject : MonoBehaviour
{
    [SerializeField] private GameObject _foodObj;

    public bool IsFood { get; set; }

    private void Start()
    {
        FoodManager.Instance.DeactivatedFoods.Add(this);
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
        FoodManager.Instance.FoodHasBeenEaten(this);
    }
}
