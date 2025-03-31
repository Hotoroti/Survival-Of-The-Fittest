using UnityEngine;

public class BodyColliderScript : MonoBehaviour
{
    public delegate void OnHerbivoreContactDelegate(AnimalController herbivoreContact);
    public OnHerbivoreContactDelegate OnHerbivoreContact;

    public delegate void OnHerbivoreExitDelegate(AnimalController herbivoreContact);
    public OnHerbivoreExitDelegate OnHerbivoreExit;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "HerbivoreAnimal")
            if (OnHerbivoreContact != null)
                OnHerbivoreContact(other.GetComponentInParent<AnimalController>());
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "HerbivoreAnimal")
            if (OnHerbivoreExit != null)
                OnHerbivoreExit(other.GetComponentInParent<AnimalController>());
    }

}
