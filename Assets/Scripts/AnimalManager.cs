using System.Collections.Generic;
using UnityEngine;

public class AnimalManager : MonoBehaviour
{
    [SerializeField] private GameObject _animalOBJ;
    [SerializeField] private int _animalSpawnCount;

    private GameObject parent;

    public static AnimalManager Instance;

    public List<GameObject> Animals = new List<GameObject>();

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
        FoodManager.Instance.GenerateFood();
        for (int i = 0; i < _animalSpawnCount; i++)
        {
            Animals.Add(Instantiate(_animalOBJ, new Vector3(0, 0, 0), Quaternion.identity, parent.transform));
        }
    }
}



