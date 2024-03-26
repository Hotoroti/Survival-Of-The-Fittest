using System.Collections.Generic;
using UnityEngine;

public class AnimalManager : MonoBehaviour
{
    [SerializeField] private GameObject _animalOBJ;
    [SerializeField] private int _animalSpawnCount;

    private GameObject parent;

    public static AnimalManager Instance;

    public List<GameObject> Animals = new List<GameObject>();

    public List<GameObject> AllFemaleAnimals = new List<GameObject>();
    public List<GameObject> AllMaleAnimals = new List<GameObject>();

    public List<GameObject> FemaleAnimalsToMate = new List<GameObject>();

    public GameObject AnimalObject { get { return _animalOBJ; } }

    public bool FirstGenerationPast = false;

    private void Awake()
    {
        if (Instance != null)
            Destroy(Instance);
        else
            Instance = this;

        parent = new GameObject("AnimalParent");
    }

    public void SpawnAnimals()
    {
        for (int i = 0; i < _animalSpawnCount; i++)
        {
            Animals.Add(Instantiate(_animalOBJ, new Vector3(0, 0, 0), Quaternion.identity, parent.transform));
        }
        FirstGenerationPast = true;
    }
}



