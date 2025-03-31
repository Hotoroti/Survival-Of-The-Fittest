using System.Collections.Generic;
using UnityEngine;

public class EnvironmentMaker : MonoBehaviour
{
    [SerializeField] private float _bushCount = 10, _bushRadius = 1;
    [SerializeField] private GameObject _bush;
    [SerializeField] private Collider _bushesSpawnBox;

    private List<GameObject> _bushes = new List<GameObject>();

    const int MAX_ATTEMPTS = 10;

    public GameObject FoodParent { get; private set; }

    public static EnvironmentMaker Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(Instance);
            Debug.LogError("More then one Instance of the TimeSettings, destroy all");
            return;
        }
        else
        {
            Instance = this;
        }

        FoodParent = new GameObject("FoodParent");
    }

    public void CreateBushes()
    {
        GameObject parent = new GameObject("BushesParent");
        for (int i = 0; i < _bushCount; i++)
        {
            Vector3 spawnPos = Vector3.zero;
            bool validPos = false;

            for (int attempt = 0; attempt < MAX_ATTEMPTS; attempt++)
            {
                float posX = _bushesSpawnBox.bounds.center.x + Random.Range(-_bushesSpawnBox.bounds.extents.x, _bushesSpawnBox.bounds.extents.x);
                float posZ = _bushesSpawnBox.bounds.center.z + Random.Range(-_bushesSpawnBox.bounds.extents.z, _bushesSpawnBox.bounds.extents.z);
                spawnPos = new Vector3(posX, _bushesSpawnBox.transform.position.y, posZ);

                if (!Physics.CheckSphere(spawnPos, _bushRadius, LayerMask.GetMask("Bush")))
                {
                    validPos = true;
                    break;
                }
            }

            if (validPos)
            {
                _bushes.Add(Instantiate(_bush, spawnPos, Quaternion.identity, parent.transform));
            }
            else
            {
                Debug.Log("Could not spawn bush, because it overlaps with other bushes");
            }
        }
    }
}
