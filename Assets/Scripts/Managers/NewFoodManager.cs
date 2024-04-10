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
        var parentObject = new GameObject("FoodParent");
        parentObject.transform.parent = transform;
        foreach(GameObject obj in _generateFloor.GridCells)
        {
            var tempObj = Instantiate(_food, obj.transform.position, Quaternion.identity);
            tempObj.transform.parent = parentObject.transform;
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

    private void FixedUpdate()
    {
        if (_activeFoodList.Count >= _maxAvailableFood) return;

        if(_activeFoodList.Count <= _maxAvailableFood * .25f)
        {
            for(int i = 0; i < _maxAvailableFood - _activeFoodList.Count; i++)
            {
                int randValue = Random.Range(0, DeactivatedFoodList.Count);
                var tempObj = DeactivatedFoodList[randValue];
                DeactivatedFoodList.RemoveAt(randValue);

                tempObj.GetComponent<FoodObject>().IsFood = true;
                tempObj.GetComponent<FoodObject>().ShowFood();

                _activeFoodList.Add(tempObj);
            }
        }
       
    }

    public void GenerateNewFood(FoodObject eatenFood)
    {      
        _activeFoodList.Remove(eatenFood.gameObject);
        DeactivatedFoodList.Add(eatenFood.gameObject);
    }

}
