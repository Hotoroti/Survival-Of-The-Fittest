using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewAnimalManager : MonoBehaviour
{
    [SerializeField] private int _initialAnimalCount;
    [SerializeField] public GameObject AnimalObj;
    [SerializeField] private GenerateFloor _floor;

    private float _timeActive;
    
    public List<GameObject> Animals = new List<GameObject>();

    public List<GameObject> AnimalsReadyToMate = new List<GameObject>();

    public GameObject Parent { get; private set; }
    public bool IsNotFirstGeneration {  get; private set; }

    public bool CanSpawnCarnivore { get; private set; }
    private void Start()
    {
        Parent = new GameObject("AnimalParent");
        Parent.transform.parent = transform;
        for(int i = 0; i <_initialAnimalCount; i++)
        {
            Animals.Add(Instantiate(AnimalObj, _floor.GridCells[Random.Range(0, _floor.GridCells.Count)].transform.position, Quaternion.identity, Parent.transform));
        }
        IsNotFirstGeneration = true;
    }

    private void FixedUpdate()
    {
        _timeActive += Time.deltaTime;

        CanSpawnCarnivore = _timeActive >= 1000;
    }
    public void AnimalDied(GameObject animal)
    {
        Animals.Remove(animal);

        Destroy(animal);
    }
}
