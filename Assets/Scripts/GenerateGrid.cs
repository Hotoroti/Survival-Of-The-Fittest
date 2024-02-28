using System.Collections.Generic;
using UnityEngine;

public class GenerateGrid : MonoBehaviour
{
    [SerializeField] private int _sizeX, _sizeZ, _spacing;
    [SerializeField] private GameObject _gridObj;

    [HideInInspector] public List<GameObject> GridCells = new List<GameObject>();
    private void Start()
    {
        for (int x = 0; x < _sizeX; x++)
        {
            for (int z = 0; z < _sizeZ; z++)
            {
                if (x == 0 || z == 0 || x == _sizeX - 1 || z == _sizeZ - 1)
                    continue;
                GridCells.Add(Instantiate(_gridObj, new Vector3(x * _spacing, 0, z * _spacing), Quaternion.identity));


            }
        }
    }
}
