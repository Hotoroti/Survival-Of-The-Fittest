using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NewAnimalController : MonoBehaviour
{
    [SerializeField] private float _energy;

    private NewAnimalManager _animalManager;
    public AnimalDNA AnimalDNA { get; private set; }
    public float CurrentEnergy {  get; private set; }   

    private void Awake()
    {
        _animalManager = GetComponentInParent<NewAnimalManager>();
        AnimalDNA = GetComponent<AnimalDNA>();
        transform.localScale *= AnimalDNA.Chromosomes[1];

        CurrentEnergy = _energy;
    }

    private void FixedUpdate()
    {
        EnergyConsumption();
    }

    private void EnergyConsumption()
    {
        if (CurrentEnergy > 0f)
            CurrentEnergy -= Time.fixedDeltaTime * (.5f * AnimalDNA.Chromosomes[1] * (AnimalDNA.Chromosomes[0] * AnimalDNA.Chromosomes[0]));
        else if (CurrentEnergy <= 0f)
            Die();
    }
    
    public void ResetEnergy()
    {
        CurrentEnergy = _energy;
    }
    public void Die()
    {
        _animalManager.AnimalDied(gameObject);
    }
}
