using System.Collections.Generic;
using UnityEngine;

public class AnimalManager : MonoBehaviour
{
    [SerializeField] private GameObject _animalOBJ;
    [SerializeField] private int _animalSpawnCount;

    private GameObject parent;
    private bool _spawnedAllAnimals;

    private Dictionary<string, GameObject> _bestOfGenerations = new Dictionary<string, GameObject>();

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
            _bestOfGenerations.Add("gen" + Generation, Animals[0]);
            BestOfNewestGeneration = Animals[0].GetComponent<AnimalDNA>();
            Time.timeScale = 0;
        }
    }

    public void SpawnAnimals()
    {
        Generation++;
        for (int i = 0; i < _animalSpawnCount; i++)
        {
            Animals.Add(Instantiate(_animalOBJ, new Vector3(0, 0, 0), Quaternion.identity, parent.transform));
        }

        _spawnedAllAnimals = true;
    }
}
