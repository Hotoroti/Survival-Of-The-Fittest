using UnityEngine;

public class Settings : MonoBehaviour
{
    public static Settings Instance;

    [SerializeField, Range(0, 100)] private int _mutationChange;
    [SerializeField] private int _amountOfAnimals;


    [SerializeField] private GameObject _animalOBJ;
    [SerializeField] private Vector2 _minMaxLife, _minMaxSpeed, _minMaxSense, _minMaxEnergy;
    public int MutationChangeMax => _mutationChange;
    public int AmountOfAnimals => _amountOfAnimals;

    private void Awake()
    {
        if (Instance != null)
            Destroy(Instance);
        else
            Instance = this;
    }

    private void Start()
    {
        StartWorld();
    }

    public void StartWorld()
    {
        EnvironmentMaker.Instance.CreateBushes();

        for (int i = 0; i < AmountOfAnimals; i++)
        {
            AnimalDNA animalDNA = Instantiate(_animalOBJ, new Vector3(45, 0, 45), Quaternion.identity).GetComponent<AnimalDNA>();

            if (animalDNA == null)
            {
                Debug.LogError("Can not set the animalDNA it does not exist");
                return;
            }

            animalDNA.SetChromosomes(
                Random.Range(_minMaxLife.x, _minMaxLife.y),
                Random.Range(_minMaxSpeed.x, _minMaxSpeed.y),
                Random.Range(_minMaxSense.x, _minMaxSense.y),
                Random.Range(_minMaxEnergy.x, _minMaxEnergy.y));
        }
    }
}
