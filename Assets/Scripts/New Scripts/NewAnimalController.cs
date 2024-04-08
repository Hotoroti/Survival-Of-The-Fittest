using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NewAnimalController : MonoBehaviour
{
    [SerializeField] private float _energy;

    private Vector3 _walkTarget;

    public AnimalDNA AnimalDNA { get; private set; }
    public float CurrentEnergy {  get; private set; }   
    private void Awake()
    {
        AnimalDNA = GetComponent<AnimalDNA>();
        transform.localScale *= AnimalDNA.Chromosomes[1];

        CurrentEnergy = _energy;

        _walkTarget = transform.position + Random.onUnitSphere * AnimalDNA.Chromosomes[2];
        _walkTarget = new Vector3(_walkTarget.x, transform.position.y, _walkTarget.z);
    }

    private void FixedUpdate()
    {
        Walk();
        EnergyConsumption();
    }

    private void EnergyConsumption()
    {
        if (CurrentEnergy > 0f)
            CurrentEnergy -= Time.fixedDeltaTime * (.5f * AnimalDNA.Chromosomes[1] * (AnimalDNA.Chromosomes[0] * AnimalDNA.Chromosomes[0]));
        else if (CurrentEnergy <= 0f)
            Die();
    }
    private void Walk()
    {
        if(_walkTarget == null || Vector3.Distance(transform.position, _walkTarget) <= .5f)
        {
            _walkTarget = transform.position + Random.onUnitSphere * AnimalDNA.Chromosomes[2];
            _walkTarget = new Vector3(_walkTarget.x, transform.position.y, _walkTarget.z);
        }
        else
        {
            var targetMask = 1 << 7;
            if(Physics.Raycast(new Vector3(_walkTarget.x, .1f, _walkTarget.z), Vector3.down, .5f, targetMask))
            {
                Debug.Log("Hit Floor");
                transform.position = Vector3.MoveTowards(transform.position, _walkTarget, Time.fixedDeltaTime * AnimalDNA.Chromosomes[0]);
                transform.LookAt(_walkTarget);
            }
            else
            {
                _walkTarget = transform.position + Random.onUnitSphere * AnimalDNA.Chromosomes[2];
                _walkTarget = new Vector3(_walkTarget.x, transform.position.y, _walkTarget.z);
            }
        }
            
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}
