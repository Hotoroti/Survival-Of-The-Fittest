using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NewAnimalController : MonoBehaviour
{
    [SerializeField] private float _energy;
    [SerializeField] private int _fitnessScoreReadyToMate;
    public NewAnimalManager AnimalManager { get; private set; }
    public AnimalDNA AnimalDNA { get; private set; }
    public float CurrentEnergy {  get; private set; }
    
    public float FitnessScore {  get; private set; }

    public bool ReadyToMate {  get; set; }

    private void Awake()
    {
        AnimalManager = GetComponentInParent<NewAnimalManager>();
        AnimalDNA = GetComponent<AnimalDNA>();
        transform.localScale *= AnimalDNA.Chromosomes[1];

        CurrentEnergy = _energy;

        FitnessScore = 0;
    }

    private void FixedUpdate()
    {
        EnergyConsumption();
        FitnessScore += Time.fixedDeltaTime;
        if(!ReadyToMate && FitnessScore >= _fitnessScoreReadyToMate)
        {
            ReadyToMate = true;
            AnimalManager.AnimalsReadyToMate.Add(gameObject);
        }
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
        AnimalManager.AnimalDied(gameObject);
    }
}
