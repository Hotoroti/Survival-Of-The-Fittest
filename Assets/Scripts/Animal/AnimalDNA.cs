using System.Collections.Generic;
using UnityEngine;

public class AnimalDNA : MonoBehaviour
{
    [SerializeField] private float _minSpeed, _maxSpeed, _minSize, _maxSize, _minLife, _maxLife, _minSense, _maxSense;
    private float _speed, _size, _life, _sense, _gender, _vore;

    [SerializeField] private SphereCollider _senseCollider;
    [SerializeField] private Renderer _renderer;
    [HideInInspector] public List<float> Chromosomes;

    private void Awake()
    {
        if (!AnimalManager.Instance.FirstGenerationPast)
            FirstGenerationAnimals();
        else
            OtherGenerationAnimals();

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

        _speed /= _size;
        //Chromosome 0 = speed
        //Chromosome 1 = _size
        //Chromosome 2 = _life
        //Chromosome 3 = _sense
        //Chromosome 4 = _gender (0 = male, 1 female)
        //Chromosome 5 = _vore (0 = herbivore, 1 = carnivore)
        Chromosomes = new List<float> { _speed, _size, _life, _sense, _gender, _vore };
    }

    public void FirstGenerationAnimals()
    {
        _speed = (int)Random.Range(_minSpeed, _maxSpeed);
        _size = (int)Random.Range(_minSize, _maxSize);
        _life = (int)Random.Range(_minLife, _maxLife);
        _sense = (int)Random.Range(_minSense, _maxSense);
        _gender = (int)Random.Range(0, 2);
        _vore = 0;
    }

    public void OtherGenerationAnimals()
    {
        var parentController = transform.parent.gameObject.GetComponent<AnimalController>();
        transform.parent = AnimalManager.Instance.Parent.transform;

        _speed = Random.Range(0, 2) < 1 ? parentController.AnimalDNA.Chromosomes[0] : parentController.Mate.AnimalDNA.Chromosomes[0];
        _size = Random.Range(0, 2) < 1 ? parentController.AnimalDNA.Chromosomes[1] : parentController.Mate.AnimalDNA.Chromosomes[1];
        _life = Random.Range(0, 2) < 1 ? parentController.AnimalDNA.Chromosomes[2] : parentController.Mate.AnimalDNA.Chromosomes[2];
        _sense = Random.Range(0, 2) < 1 ? parentController.AnimalDNA.Chromosomes[3] : parentController.Mate.AnimalDNA.Chromosomes[3];
        _gender = Random.Range(0, 2);
        _vore = 0;

        _speed = Random.Range(0, 101) < 10 ? _speed : _speed * Random.Range(0.8f, 1.2f);
        _size = Random.Range(0, 101) < 10 ? _size : _size * Random.Range(0.8f, 1.2f);
        _life = Random.Range(0, 101) < 10 ? _life : _life * Random.Range(0.8f, 1.2f);
        _sense = Random.Range(0, 101) < 10 ? _sense : _sense * Random.Range(0.8f, 1.2f);

        _size /= _size;
    }
}
