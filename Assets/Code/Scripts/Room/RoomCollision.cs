using UnityEngine;

public class RoomCollision : MonoBehaviour
{
    public static bool canBePlaced;

    void Awake()
    {
        canBePlaced = true;
    }

    void OnTriggerStay(Collider obj)
    {
        Renderer renderer = obj.gameObject.GetComponent<Renderer>();

        if (renderer)
        {
            renderer.material.SetColor("_Color", Color.red);
        }

        canBePlaced = false;
    }

    void OnTriggerExit(Collider obj)
    {
        Renderer renderer = obj.gameObject.GetComponent<Renderer>();

        if (renderer)
        {
            renderer.material.SetColor("_Color", Color.green);
        }

        canBePlaced = true;
    }
}