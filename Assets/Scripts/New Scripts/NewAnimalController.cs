using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NewAnimalController : MonoBehaviour
{
    public AnimalDNA AnimalDNA {  get; private set; }

    private Vector3 _walkTarget;
    private void Awake()
    {
        AnimalDNA = GetComponent<AnimalDNA>();
        transform.localScale *= AnimalDNA.Chromosomes[1];
    }

    private void FixedUpdate()
    {
        Walk();
    }

    private void Walk()
    {
        if(_walkTarget == null || Vector3.Distance(transform.position, _walkTarget) <= .5f)
        {
            _walkTarget = Random.onUnitSphere * AnimalDNA.Chromosomes[3];
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
                _walkTarget = Random.onUnitSphere * AnimalDNA.Chromosomes[3];
                _walkTarget = new Vector3(_walkTarget.x, transform.position.y, _walkTarget.z);
            }
        }
            
    }
}
