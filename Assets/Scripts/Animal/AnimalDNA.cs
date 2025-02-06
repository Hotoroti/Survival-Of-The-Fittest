using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AnimalDNA : MonoBehaviour
{
    public Dictionary<string, float> Chromosomes { get; private set; }
    public float Size { get; private set; }
    public float ReproductionRate { get; private set; }
    public float ReactionTime { get; private set; }
    public float Hunger { get; private set; }
    public int Gender { get; private set; }

    public UnityEvent Initialise { get; private set; } = new UnityEvent();

    private void Start()
    {
        SetChromosomes(500f, 11f, 11f, 111f);
    }

    public void SetChromosomes(float life, float speed, float sense, float energy)
    {
        Chromosomes = new Dictionary<string, float>()
        {
            { "Life", life },
            { "WalkingSpeed", speed},
            { "Sense", sense},
            { "Energy", energy}
        };

        SetValues();
        Initialise?.Invoke();
    }

    private void SetValues()
    {
        Gender = Random.Range(0, 2);
        Size = Chromosomes["Life"] / 100f;
        ReproductionRate = Chromosomes["Life"] / 2f;
        ReactionTime = Chromosomes["Sense"] * 2f;
        Hunger = Chromosomes["Energy"] / 10f;
    }
}
