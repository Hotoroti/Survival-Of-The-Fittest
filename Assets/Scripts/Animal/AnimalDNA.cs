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

    private const float STARTHUNGER = 1000f;

    /// <summary>
    /// Call this function to set the inheritable chromosomes of the Organism
    /// </summary>
    /// <param name="life">The size of the life chromosome</param>
    /// <param name="speed">The size of the walkingspeed chromosome</param>
    /// <param name="sense">The size of the sense chromosome</param>
    /// <param name="energy">The size of the energy chromosome</param>
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

    /// <summary>
    /// Call this function to set the unheritable values
    /// </summary>
    private void SetValues()
    {
        Gender = Random.Range(0, 2);
        Size = Chromosomes["Life"] / 100f;
        ReproductionRate = Chromosomes["Life"] / 2f;
        ReactionTime = Chromosomes["Sense"] * 2f;
        Hunger = STARTHUNGER - (Chromosomes["Energy"] / 10f);
    }
}
