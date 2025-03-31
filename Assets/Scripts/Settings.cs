using UnityEngine;

public class Settings : MonoBehaviour
{
    public static Settings Instance;

    [SerializeField, Range(0, 100)] private int _mutationChange;
    [SerializeField] private int _amountOfAnimals;


    [SerializeField] private GameObject _animalOBJ;
    [SerializeField, Tooltip("The start values for the first organisms that the random value will be in between")] private Vector2 _minMaxLife, _minMaxSpeed, _minMaxSense, _minMaxEnergy;
    [SerializeField, Tooltip("The negative and positive mutation values")] private Vector2 _mutationMultiplier;
    [SerializeField] private float _mateTime;

    public float MateTime => _mateTime;
    public int MutationChangeMax => _mutationChange;
    public int AmountOfAnimals => _amountOfAnimals;
    public Vector2 MutationMultiplier => _mutationMultiplier;
    public GameObject AnimalObject => _animalOBJ;

    public GameObject AnimalParent { get; private set; }

    private void Awake()
    {
        if (Instance != null)
            Destroy(Instance);
        else
            Instance = this;
    }

    private void Start()
    {
        AnimalParent = new GameObject("AnimalParent");
        StartWorld();
    }

    public void StartWorld()
    {
        EnvironmentMaker.Instance.CreateBushes();

        for (int i = 0; i < AmountOfAnimals; i++)
        {
            AnimalDNA animalDNA = Instantiate(_animalOBJ, new Vector3(45, 0, 45), Quaternion.identity, AnimalParent.transform).GetComponent<AnimalDNA>();

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
