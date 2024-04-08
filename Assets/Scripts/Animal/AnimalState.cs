using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public  class AnimalState : MonoBehaviour
{
    public enum States { Walking, Eating, Mating, Running }
    public GameObject MateTarget {  get; set; }

    private States _currentState;
    private NewAnimalController _animalController;

    private Vector3 _walkTarget;
    private List<GameObject> _foodObjects = new List<GameObject>();
    private List<GameObject> _mateObjects = new List<GameObject>();

    [SerializeField] private float _eatingTime;
    private float _eatingTimer;
    private void Start()
    {
        _animalController = GetComponent<NewAnimalController>();

        _walkTarget = _animalController.transform.position + Random.onUnitSphere * _animalController.AnimalDNA.Chromosomes[2];
        _walkTarget = new Vector3(_walkTarget.x, _animalController.transform.position.y, _walkTarget.z);

        _currentState = States.Walking;
    }

    private void FixedUpdate()
    {
        switch (_currentState)
        {
            case States.Walking:
                Walking();
                break;
            case States.Eating:
                Eating();
                break;
            case States.Mating:
                Mating(); 
                break;
        }  
    }

    private void Walking()
    {
        if (_walkTarget == null || Vector3.Distance(transform.position, _walkTarget) <= .5f)
        {
            _walkTarget = _animalController.transform.position + Random.onUnitSphere * _animalController.AnimalDNA.Chromosomes[2];
            _walkTarget = new Vector3(_walkTarget.x, _animalController.transform.position.y, _walkTarget.z);
        }
        else
        {
            var targetMask = 1 << 7;
            //Check if targetposition is on floor
            if (Physics.Raycast(new Vector3(_walkTarget.x, .1f, _walkTarget.z), Vector3.down, .5f, targetMask))
            {
                _animalController.transform.position = Vector3.MoveTowards(_animalController.transform.position, _walkTarget, Time.fixedDeltaTime * _animalController.AnimalDNA.Chromosomes[0]);
                _animalController.transform.LookAt(_walkTarget);
            }
            else
            {
                _walkTarget = _animalController.transform.position + Random.onUnitSphere * _animalController.AnimalDNA.Chromosomes[2];
                _walkTarget = new Vector3(_walkTarget.x, _animalController.transform.position.y, _walkTarget.z);
            }
        }
    }

    private void Eating()
    {
        if (Vector3.Distance(transform.position, _foodObjects[0].transform.position) <= .1f)
        {
            _eatingTimer += Time.fixedDeltaTime;
            if(_eatingTimer >= _eatingTime)
            {
                _foodObjects[0].GetComponentInParent<FoodObject>().HasBeenEaten();
                _foodObjects.Clear();
                _eatingTimer = 0;
                _animalController.ResetEnergy();
                _currentState = States.Walking;
            }
        }
        else
        {
            _animalController.transform.position = Vector3.MoveTowards(_animalController.transform.position, _foodObjects[0].transform.position, Time.fixedDeltaTime * _animalController.AnimalDNA.Chromosomes[0]);
            _animalController.transform.LookAt(_foodObjects[0].transform.position);
        }
        
    }

    private void Mating()
    {
        Debug.Log("Mating");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_currentState != States.Walking) return;

        if (_animalController.CurrentEnergy <= 500 &&  other.GetComponentInParent<FoodObject>())
        {
            _foodObjects.Add(other.gameObject);
            _currentState = States.Eating;
        }

        if(_animalController.ReadyToMate && other.CompareTag("BodyCollider") && _animalController.AnimalManager.AnimalsReadyToMate.Contains(other.GetComponentInParent<AnimalDNA>().gameObject))
        {
            _mateObjects.Add(other.gameObject);
            _currentState = States.Mating;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (_foodObjects.Count <= 0) return;

        if (other.GetComponentInParent<FoodObject>())
        {
            if (_foodObjects.Contains(other.gameObject))
            {
                _foodObjects.Remove(other.gameObject);
            }
        }
    }
}
