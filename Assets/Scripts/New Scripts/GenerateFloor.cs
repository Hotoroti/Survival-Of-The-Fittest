using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateFloor : MonoBehaviour
{
    [SerializeField] private int _sizeX, _sizeZ, _spacing;
    [SerializeField] private GameObject _gridObj;

    [HideInInspector] public List<GameObject> GridCells = new List<GameObject>();

    private void Awake()
    {
        GameObject parent = new GameObject("GridParent");
        parent.transform.SetParent(transform);
        for (int x = 0; x < _sizeX; x++)
        {
            for (int z = 0; z < _sizeZ; z++)
            {
                GridCells.Add(Instantiate(_gridObj, new Vector3(x * _spacing, 0, z * _spacing), Quaternion.identity, parent.transform));
            }
        }
    }
}
