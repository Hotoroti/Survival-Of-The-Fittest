using System.Collections.Generic;
using UnityEngine;

public class AnimalDNA : MonoBehaviour
{
    [SerializeField] private float _minSpeed, _maxSpeed, _minSize, _maxSize, _minLife, _maxLife, _minSense, _maxSense;
    private float _speed, _size, _life, _sense;

    [SerializeField] private SphereCollider _senseCollider;
    [HideInInspector] public List<float> Chromosomes;

    private void Awake()
    {
        _speed = (int)Random.Range(_minSpeed, _maxSpeed);
        _size = (int)Random.Range(_minSize, _maxSize);
        _life = (int)Random.Range(_minLife, _maxLife);
        _sense = (int)Random.Range(_minSense, _maxSense);

        _senseCollider.radius = _sense;
        //Chromosome 0 = speed
        //Chromosome 1 = _size
        //Chromosome 2 = _life
        Chromosomes = new List<float> { _speed, _size, _life, _sense };
    }
}
