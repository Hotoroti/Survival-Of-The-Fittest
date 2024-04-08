using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewAnimalManager : MonoBehaviour
{
    [SerializeField] private int _initialAnimalCount;
    [SerializeField] private GameObject _animalObj;
    [SerializeField] private GenerateFloor _floor;
    
    private List<GameObject> _animals = new List<GameObject>();
    private void Start()
    {
        var parent = new GameObject("AnimalParent");
        parent.transform.parent = transform;
        for(int i = 0; i <_initialAnimalCount; i++)
        {
            _animals.Add(Instantiate(_animalObj, _floor.GridCells[Random.Range(0, _floor.GridCells.Count)].transform.position, Quaternion.identity, parent.transform));
        }
    }
}
