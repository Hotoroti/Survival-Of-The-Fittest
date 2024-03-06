using System.Collections.Generic;
using UnityEngine;

public class GenerateGrid : MonoBehaviour
{
    [SerializeField] private int _sizeX, _sizeZ, _spacing;
    [SerializeField] private GameObject _gridObj;

    [HideInInspector] public List<GameObject> GridCells = new List<GameObject>();

    public static GenerateGrid Instance;

    private void Awake()
    {
        if (Instance != null)
            Destroy(Instance);
        else
            Instance = this;
    }

    private void Start()
    {
        GameObject parent = new GameObject("GridParent");
        for (int x = 0; x < _sizeX; x++)
        {
            for (int z = 0; z < _sizeZ; z++)
            {
                GridCells.Add(Instantiate(_gridObj, new Vector3(x * _spacing, 0, z * _spacing), Quaternion.identity, parent.transform));
            }
        }
        StartCoroutine(FoodManager.Instance.IGenerateFood());
    }
}
