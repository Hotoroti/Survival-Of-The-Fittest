using System.Collections.Generic;
using UnityEngine;

public class AnimalDNA : MonoBehaviour
{
    [SerializeField] private float _minSpeed, _maxSpeed, _minSize, _maxSize, _minLife, _maxLife;
    private float _speed, _size, _life;
    [HideInInspector] public List<float> Chromosomes;

    private void Awake()
    {
        if (AnimalManager.Instance.Generation <= 1)
        {
            _speed = (int)Random.Range(_minSpeed, _maxSpeed);
            _size = (int)Random.Range(_minSize, _maxSize);
            _life = (int)Random.Range(_minLife, _maxLife);
        }
        else
        {
            Chromosomes[0] = AnimalManager.Instance.BestOfNewestGeneration.Chromosomes[0] * Random.Range(.1f, 2f);
            Chromosomes[1] = AnimalManager.Instance.BestOfNewestGeneration.Chromosomes[1] * Random.Range(.1f, 2f);
            Chromosomes[2] = AnimalManager.Instance.BestOfNewestGeneration.Chromosomes[2] * Random.Range(.1f, 2f);
        }
        //Chromosome 0 = speed
        //Chromosome 1 = _size
        //Chromosome 2 = _life
        Chromosomes = new List<float> { _speed, _size, _life };
    }
}
