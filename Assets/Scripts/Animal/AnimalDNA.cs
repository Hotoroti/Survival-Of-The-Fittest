using System.Collections.Generic;
using UnityEngine;

public class AnimalDNA : MonoBehaviour
{
    [SerializeField] private float _minSpeed, _maxSpeed, _minSize, _maxSize, _minLife, _maxLife;
    private float _speed, _size, _life;
    [HideInInspector] public List<float> Chromosomes;

    private void Awake()
    {
        _speed = (int)Random.Range(_minSpeed, _maxSpeed);
        _size = (int)Random.Range(_minSize, _maxSize);
        _life = (int)Random.Range(_minLife, _maxLife);

        //Chromosome 0 = speed
        //Chromosome 1 = _size
        //Chromosome 2 = _life
        Chromosomes = new List<float> { _speed, _size, _life };
    }
}
