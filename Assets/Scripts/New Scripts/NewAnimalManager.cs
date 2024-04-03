using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewAnimalManager : MonoBehaviour
{
    [SerializeField] private int _initialAnimalCount;
    [SerializeField] private GameObject _animalObj;
    [SerializeField] private Transform _spawnPoint;
    
    private List<GameObject> _animals = new List<GameObject>();
    private void Start()
    {
        for(int i = 0; i <_initialAnimalCount; i++)
        {
            _animals.Add(Instantiate(_animalObj, _spawnPoint.position, Quaternion.identity));
        }
    }
}
