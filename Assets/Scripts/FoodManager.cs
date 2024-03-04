using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodManager : MonoBehaviour
{
    [SerializeField] private int _maxActivateFoodCount;

    public List<FoodObject> ActivateFoods = new List<FoodObject>();
    public List<FoodObject> DeactivatedFoods = new List<FoodObject>();

    public static FoodManager Instance;

    private void Awake()
    {
        if (Instance != null)
            Destroy(Instance);
        else
            Instance = this;
    }

    public IEnumerator GenerateFood()
    {
        yield return null;
        for (int i = 0; i < _maxActivateFoodCount; i++)
        {
            int iFood = Random.Range(0, DeactivatedFoods.Count);

            ActivateFoods.Add(DeactivatedFoods[iFood]);

            DeactivatedFoods.Remove(DeactivatedFoods[iFood]);
        }

        ActivateFood();
        DeactivateFood();
        StopCoroutine(GenerateFood());
    }

    public void FoodHasBeenEaten(FoodObject food)
    {
        ActivateFoods.Remove(food);
        DeactivatedFoods.Add(food);

        int iFood = Random.Range(0, DeactivatedFoods.Count);
        ActivateFoods.Add(DeactivatedFoods[iFood]);
        DeactivatedFoods.Remove(DeactivatedFoods[iFood]);

        ActivateFood();
        DeactivateFood();
    }

    public void ActivateFood()
    {
        foreach (var food in ActivateFoods)
        {
            food.IsFood = true;
        }
    }

    public void DeactivateFood()
    {
        foreach (var food in DeactivatedFoods)
        {
            food.IsFood = false;
        }
    }

    [ContextMenu("Show all food")]
    public void ShowAllFood()
    {
        foreach (var food in DeactivatedFoods)
        {
            food.IsFood = true;
        }
    }
}
