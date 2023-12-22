using UnityEngine;

public class ObjectSnap : MonoBehaviour
{
    void Update()
    {
        ControlSnapping();
    }

    void ControlSnapping()
    {
        if (CursorController.spawnedObject || CursorController.isPickedUp)
        {
            if (Input.GetKey(KeyCode.Space) && RoomCollision.canBePlaced)
            {
                CursorController.stickSpawnedObjectToCursor = false;
                CursorController.stickPlacedObjectToCursor = false;
            }
            else if (Input.GetKeyUp(KeyCode.Space) && RoomCollision.canBePlaced)
            {
                if (CursorController.spawnedObject)
                {
                    CursorController.spawnedObject.transform.position += new Vector3(-0.1f, 0f, 0.1f);
                }
                else if (CursorController.isPickedUp)
                {
                    CursorController.placedObject.transform.position += new Vector3(-0.1f, 0f, 0.1f);
                }

                CursorController.stickSpawnedObjectToCursor = true;
                CursorController.stickPlacedObjectToCursor = true;
            }
        }
    }
}