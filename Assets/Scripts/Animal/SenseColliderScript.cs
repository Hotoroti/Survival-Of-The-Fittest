using UnityEngine;

public class SenseColliderScript : MonoBehaviour
{
    public delegate void OnFoodEnterDelegate(GameObject food);
    public OnFoodEnterDelegate OnFoodEnter;

    public delegate void OnFoodExitDelegate(GameObject food);
    public OnFoodExitDelegate OnFoodExit;

    public delegate void OnHerbivoreAnimalEnterDelegate(GameObject animal);
    public OnHerbivoreAnimalEnterDelegate OnHerbivoreAnimalEnter;

    public delegate void OnHerbivoreAnimalExitDelegate(GameObject animal);
    public OnHerbivoreAnimalExitDelegate OnHerbivoreAnimalExit;

    public delegate void OnCarnivoreAnimalEnterDelegate(GameObject animal);
    public OnCarnivoreAnimalEnterDelegate OnCarnivoreAnimalEnter;

    public delegate void OnCarnivoreAnimalExitDelegate(GameObject animal);
    public OnCarnivoreAnimalExitDelegate OnCarnivoreAnimalExit;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Food")
            if (OnFoodEnter != null)
                OnFoodEnter(other.gameObject);

        if (other.tag == "HerbivoreAnimal")
            if (OnHerbivoreAnimalEnter != null)
                OnHerbivoreAnimalEnter(other.gameObject);

        if (other.tag == "CarnivoreAnimal")
            if (OnCarnivoreAnimalEnter != null)
                OnCarnivoreAnimalEnter(other.gameObject);

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Food")
            if (OnFoodExit != null)
                OnFoodExit(other.gameObject);

        if (other.tag == "HerbivoreAnimal")
            if (OnHerbivoreAnimalExit != null)
                OnHerbivoreAnimalExit(other.gameObject);

        if (other.tag == "CarnivoreAnimal")
            if (OnCarnivoreAnimalExit != null)
                OnCarnivoreAnimalExit(other.gameObject);
    }
}
