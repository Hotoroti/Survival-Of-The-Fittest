using System.Collections.Generic;
using UnityEngine;

public class AnimalDNA : MonoBehaviour
{
    [SerializeField] private float _speed, _size, _life;
    [HideInInspector] public List<float> Chromosomes;

    private void Awake()
    {
        //Chromosome 0 = speed
        //Chromosome 1 = _size
        //Chromosome 2 = _life
        Chromosomes = new List<float> { _speed, _size, _life };
    }
}
