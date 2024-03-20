using UnityEngine;

public class FoodObject : MonoBehaviour
{
    [SerializeField] private GameObject _foodObj;

    public bool IsFood { get; set; }

    private void Start()
    {
        FoodManager.Instance.DeactivatedFoods.Add(this);
    }
    private void Update()
    {
        if (IsFood)
        {
            _foodObj.SetActive(true);            
        }
        else
        {
            _foodObj.SetActive(false);            
        }
    }

    [ContextMenu("FoodHasBeenEaten")]
    public void HasBeenEaten()
    {
        IsFood = false;
        FoodManager.Instance.FoodHasBeenEaten(this);
    }
}
