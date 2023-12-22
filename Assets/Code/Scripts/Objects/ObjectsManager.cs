using System.Collections.Generic;
using UnityEngine;

public class ObjectsManager : MonoBehaviour
{
    public static List<GameObject> objects;
    public static List<ObjectData> objectsData;
    Color red = new(0.5450980392156862f, 0f, 0f, 1f);
    Color green = new(0f, 0.39215686274509803f, 0f, 1f);
    Color blue = new(0f, 0f, 0.5450980392156862f, 1f);
    Color brown = new(0.5450980392156862f, 0.27058823529411763f, 0.07450980392156863f, 1f);

    void Awake()
    {
        objects = new List<GameObject>();
        objectsData = new List<ObjectData>();
    }

    void Update()
    {
        SetSelectedColour();

        DeleteObject();
    }

    void SetSelectedColour()
    {
        if (CursorController.isPickedUp)
        {
            Object pickedUpObject = objects[CursorController.pickedUpObjectId].GetComponent<Object>();

            if (Input.GetKeyDown(KeyCode.Keypad1) || Input.GetKeyDown(KeyCode.Alpha1))
            {
                pickedUpObject.SetColour(Color.white);
            }
            else if (Input.GetKeyDown(KeyCode.Keypad2) || Input.GetKeyDown(KeyCode.Alpha2))
            {
                pickedUpObject.SetColour(red);
            }
            else if (Input.GetKeyDown(KeyCode.Keypad3) || Input.GetKeyDown(KeyCode.Alpha3))
            {
                pickedUpObject.SetColour(green);
            }
            else if (Input.GetKeyDown(KeyCode.Keypad4) || Input.GetKeyDown(KeyCode.Alpha4))
            {
                pickedUpObject.SetColour(blue);
            }
            else if (Input.GetKeyDown(KeyCode.Keypad5) || Input.GetKeyDown(KeyCode.Alpha5))
            {
                pickedUpObject.SetColour(brown);
            }
        }
    }

    void DeleteObject()
    {
        if (Input.GetKeyDown(KeyCode.C) && CursorController.isPickedUp)
        {
            Destroy(objects[CursorController.pickedUpObjectId]);

            objects[CursorController.pickedUpObjectId] = null;
            objectsData[CursorController.pickedUpObjectId] = new();

            CursorController.resetValues = true;
        }
    }
}