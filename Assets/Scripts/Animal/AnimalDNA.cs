using System.Collections.Generic;
using UnityEngine;

public class AnimalDNA : MonoBehaviour
{
    [SerializeField] private float _minSpeed, _maxSpeed, _minSize, _maxSize, _minLife, _maxLife, _minSense, _maxSense;
    private float _speed, _size, _life, _sense, _gender;

    [SerializeField] private SphereCollider _senseCollider;
    [SerializeField] private Renderer _renderer;
    [HideInInspector] public List<float> Chromosomes;

    private void Awake()
    {
        _speed = (int)Random.Range(_minSpeed, _maxSpeed);
        _size = (int)Random.Range(_minSize, _maxSize);
        _life = (int)Random.Range(_minLife, _maxLife);
        _sense = (int)Random.Range(_minSense, _maxSense);
        _gender = (int)Random.Range(0, 2);

        if (_gender == 0)
        {
            _renderer.material.color = Color.blue;
            AnimalManager.Instance.AllMaleAnimals.Add(gameObject);
        }
        else if (_gender == 1)
        {
            _renderer.material.color = Color.magenta;
            AnimalManager.Instance.AllFemaleAnimals.Add(gameObject);
            AnimalManager.Instance.FemaleAnimalsToMate.Add(gameObject);
        }

        _senseCollider.radius = _sense;
        //Chromosome 0 = speed
        //Chromosome 1 = _size
        //Chromosome 2 = _life
        //Chromosome 3 = _sense
        //Chromosome 4 = _gender (0 = male, 1 female)
        Chromosomes = new List<float> { _speed, _size, _life, _sense, _gender };
    }
}
