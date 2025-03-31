using UnityEngine;

public class SenseColliderScript : MonoBehaviour
{
    public delegate void OnFoodEnterDelegate(GameObject food);
    public OnFoodEnterDelegate OnFoodEnter;

    public delegate void OnFoodExitDelegate(GameObject food);
    public OnFoodExitDelegate OnFoodExit;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Food")
            if (OnFoodEnter != null)
                OnFoodEnter(other.gameObject);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Food")
            if (OnFoodExit != null)
                OnFoodExit(other.gameObject);
    }
}
