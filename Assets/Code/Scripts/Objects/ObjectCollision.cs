using UnityEngine;

public class ObjectCollision : MonoBehaviour
{
    public static bool canBeLowered;

    void Awake()
    {
        canBeLowered = true;
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Placed Object"))
        {
            canBeLowered = true;
        }
        else
        {
            canBeLowered = false;
        }
    }

    void OnTriggerExit(Collider collider)
    {
        canBeLowered = true;
    }
}