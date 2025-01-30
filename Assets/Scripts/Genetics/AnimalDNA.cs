using System.Collections.Generic;
using UnityEngine;

public class AnimalDNA : MonoBehaviour
{
    public Dictionary<string, float> Chromosomes { get; private set; }
    public float Size { get; private set; }
    public float ReproductionRate { get; private set; }
    public float ReactionTime { get; private set; }
    public float Hunger { get; private set; }
    public int Gender { get; private set; }

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
