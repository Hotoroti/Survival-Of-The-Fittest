using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewFoodManager : MonoBehaviour
{
    [SerializeField] private int _maxAvailableFood;
    [SerializeField] private GenerateFloor _generateFloor;
    [SerializeField] private GameObject _food;

    [HideInInspector] public List<GameObject> DeactivatedFoodList = new List<GameObject>();
    private List<GameObject> _activeFoodList = new List<GameObject>();
    private void Start()
    {
        foreach(GameObject obj in _generateFloor.GridCells)
        {
            var tempObj = Instantiate(_food, obj.transform.position, Quaternion.identity);
            DeactivatedFoodList.Add(tempObj);
        }

        for(int i = 0; i < _maxAvailableFood; i++)
        {
            int randValue = Random.Range(0, DeactivatedFoodList.Count);
            
            var tempObj = DeactivatedFoodList[randValue];
            DeactivatedFoodList.RemoveAt(randValue);

            tempObj.GetComponent<FoodObject>().IsFood = true;
            tempObj.GetComponent<FoodObject>().ShowFood();

            _activeFoodList.Add(tempObj);
        }
    }

    public void GenerateNewFood(FoodObject eatenFood)
    {
        int randValue = Random.Range(0, DeactivatedFoodList.Count);

        var tempObj = DeactivatedFoodList[randValue];
        DeactivatedFoodList.RemoveAt(randValue);

        tempObj.GetComponent<FoodObject>().IsFood = true;
        tempObj.GetComponent<FoodObject>().ShowFood();

        _activeFoodList.Remove(eatenFood.gameObject);
        DeactivatedFoodList.Add(eatenFood.gameObject);
        eatenFood.IsFood = false;
        eatenFood.ShowFood();


        _activeFoodList.Add(tempObj);
    }

}
