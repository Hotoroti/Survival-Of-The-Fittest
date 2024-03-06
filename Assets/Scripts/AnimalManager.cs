using System;
using System.Collections.Generic;
using UnityEngine;

public class AnimalManager : MonoBehaviour
{
    [SerializeField] private GameObject _animalOBJ;
    [SerializeField] private int _animalSpawnCount;

    private GameObject parent;
    private bool _spawnedAllAnimals;

    private List<AnimalData> _animalDatas = new List<AnimalData>();


    public int Generation;
    public AnimalDNA BestOfNewestGeneration;
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

    private void FixedUpdate()
    {
        Debug.Log(Animals.Count);
        if (_spawnedAllAnimals && Animals.Count <= 1)
        {
            BestOfNewestGeneration = Animals[0].GetComponent<AnimalDNA>();
            Destroy(Animals[0]);
            Animals.Clear();
            SafeToJSON();
            Generation++;
            Invoke("SpawnAnimals", 5f);
        }
    }

    public void SpawnAnimals()
    {
        _spawnedAllAnimals = false;
        FoodManager.Instance.GenerateFood();
        for (int i = 0; i < _animalSpawnCount; i++)
        {
            Animals.Add(Instantiate(_animalOBJ, new Vector3(0, 0, 0), Quaternion.identity, parent.transform));
        }

        _spawnedAllAnimals = true;
    }

    public void SafeToJSON()
    {
        var data = new AnimalData();
        data.Generation = Generation;
        data.Chromosomes = BestOfNewestGeneration.Chromosomes;
        string animalData = JsonUtility.ToJson(data, true);
        string filePath = Application.dataPath + "/animalData.json";
        Debug.Log(filePath);
        System.IO.File.AppendAllText(filePath, animalData);
        Debug.Log("Saved");
    }
}

[Serializable]
public class AnimalData
{
    public int Generation;
    public List<float> Chromosomes;
}

