using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AnimalDNA : MonoBehaviour
{
    [SerializeField] private float _minSpeed, _maxSpeed, _minSize, _maxSize, _minSense, _maxSense;
    private float _speed, _size, _sense, _gender, _vore;

    private NewAnimalManager _animalManager;
    [SerializeField] private SphereCollider _senseCollider;
    [SerializeField] private Renderer _renderer;
    [HideInInspector] public List<float> Chromosomes;

    private void Awake()
    {
        _animalManager = GetComponentInParent<NewAnimalManager>();

        if(!_animalManager.IsNotFirstGeneration)
            FirstGenerationAnimals();
        else
            OtherGenerationAnimals();

        if (_gender == 0)
        {
            _renderer.material.color = Color.blue;
        }
        else if (_gender == 1)
        {
            _renderer.material.color = Color.magenta;
        }

        _senseCollider.radius = _sense;

        _speed /= _size;
        //Chromosome 0 = speed
        //Chromosome 1 = _size
        //Chromosome 2 = _sense
        //Chromosome 3 = _gender (0 = male, 1 female)
        //Chromosome 4 = _vore (0 = herbivore, 1 = carnivore)
        Chromosomes = new List<float> { _speed, _size, _sense, _gender, _vore };
    }

    public void FirstGenerationAnimals()
    {
        _speed = Random.Range(_minSpeed, _maxSpeed);
        _size = Random.Range(_minSize, _maxSize);
        _sense = Random.Range(_minSense, _maxSense);
        _gender = Random.Range(0, 2);
        _vore = 0;
    }

    public void OtherGenerationAnimals()
    {
        var parentController = transform.parent.gameObject.GetComponent<NewAnimalController>();
        transform.parent = _animalManager.Parent.transform;

        _speed = Random.Range(0, 2) < 1 ? parentController.AnimalDNA.Chromosomes[0] : parentController.Mate.AnimalDNA.Chromosomes[0];
        _size = Random.Range(0, 2) < 1 ? parentController.AnimalDNA.Chromosomes[1] : parentController.Mate.AnimalDNA.Chromosomes[1];
        _sense = Random.Range(0, 2) < 1 ? parentController.AnimalDNA.Chromosomes[2] : parentController.Mate.AnimalDNA.Chromosomes[2];
        _gender = Random.Range(0, 2);
        _vore = 0;

        _speed = Random.Range(0, 101) < 10 ? _speed : _speed * Random.Range(0.8f, 1.2f);
        _size = Random.Range(0, 101) < 10 ? _size : _size * Random.Range(0.8f, 1.2f);
        _sense = Random.Range(0, 101) < 10 ? _sense : _sense * Random.Range(0.8f, 1.2f);

        _size /= _size;

        _animalManager.Animals.Add(gameObject);
    }
}
