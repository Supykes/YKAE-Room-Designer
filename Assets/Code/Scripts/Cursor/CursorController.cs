using UnityEngine;

public class CursorController : MonoBehaviour
{
    [Header("Objects Parent")]
    [SerializeField] Transform objects;

    [Header("Objects To Spawn")]
    [SerializeField] GameObject bookcase;
    [SerializeField] GameObject table;
    [SerializeField] GameObject restChair;

    [Header("UI Objects")]
    [SerializeField] GameObject UIBookcase;
    [SerializeField] GameObject UITable;
    [SerializeField] GameObject UIRestChair;

    [Header("UI Text")]
    [SerializeField] GameObject placeTextBox;
    [SerializeField] GameObject modificationTextBox;
    [SerializeField] GameObject deleteTextBox;

    Vector3 cursorPosition;
    public static GameObject spawnedObject;
    public static GameObject placedObject;
    bool canBeSpawnedInRoom = false;
    bool canBeSpawned = true;
    public static bool stickSpawnedObjectToCursor;
    public static bool stickPlacedObjectToCursor;
    bool inYPositionMode = false;
    int layerMask;
    Vector3 objectOffset = new(0f, 1f, 0f);
    int timesPressedZKey = 0;
    bool canPressLCtrl = false;
    public static bool isPickedUp;
    public static bool resetValues;
    public static int pickedUpObjectId;

    void Awake()
    {
        spawnedObject = null;
        placedObject = null;
        stickSpawnedObjectToCursor = true;
        stickPlacedObjectToCursor = false;
        isPickedUp = false;
        resetValues = false;
        pickedUpObjectId = -1;
    }

    void Start()
    {
        layerMask = LayerMask.GetMask("Room");
    }

    void FixedUpdate()
    {
        GetCursorPosition();
        GetPlacedObject();

        StickObjectToCursor(spawnedObject, stickSpawnedObjectToCursor);
        StickObjectToCursor(placedObject, stickPlacedObjectToCursor);
    }

    void Update()
    {
        SpawnObjects();

        PlaceObject(spawnedObject);
        PlaceObject(placedObject);

        PickUpPlacedObject();

        ResetValues();
    }

    void GetCursorPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hitInfo, 100f, layerMask))
        {
            cursorPosition = hitInfo.point;

            canBeSpawnedInRoom = true;

            if (canBeSpawned)
            {
                ChangeUIObjectsColour(Color.green);
            }
        }
        else
        {
            canBeSpawnedInRoom = false;

            ChangeUIObjectsColour(Color.red);
        }
    }

    void GetPlacedObject()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (!isPickedUp && !spawnedObject)
        {
            if (Physics.Raycast(ray, out RaycastHit hitInfo, 100f))
            {
                if (hitInfo.transform.CompareTag("Placed Object"))
                {
                    placedObject = hitInfo.transform.gameObject;

                    canPressLCtrl = true;
                }
                else
                {
                    canPressLCtrl = false;
                }
            }
        }
    }

    void SpawnObjects()
    {
        if (canBeSpawnedInRoom)
        {
            if (Input.GetKeyDown(KeyCode.A) && canBeSpawned)
            {
                spawnedObject = Instantiate(bookcase, cursorPosition + objectOffset, bookcase.transform.rotation, objects);
                placedObject = null;

                canBeSpawned = false;

                ChangeUIObjectsColour(Color.red);

                placeTextBox.SetActive(true);
            }
            else if (Input.GetKeyDown(KeyCode.S) && canBeSpawned)
            {
                spawnedObject = Instantiate(table, cursorPosition + objectOffset, table.transform.rotation, objects);
                placedObject = null;

                canBeSpawned = false;

                ChangeUIObjectsColour(Color.red);

                placeTextBox.SetActive(true);
            }
            else if (Input.GetKeyDown(KeyCode.D) && canBeSpawned)
            {
                spawnedObject = Instantiate(restChair, cursorPosition + objectOffset, restChair.transform.rotation, objects);
                placedObject = null;

                canBeSpawned = false;

                ChangeUIObjectsColour(Color.red);

                placeTextBox.SetActive(true);
            }
        }
    }

    void StickObjectToCursor(GameObject obj, bool stickObjectToCursor)
    {
        if (obj && stickObjectToCursor)
        {
            obj.transform.position = cursorPosition + objectOffset;
        }
    }

    void PlaceObject(GameObject obj)
    {
        if (Input.GetKey(KeyCode.LeftShift) && !inYPositionMode && RoomCollision.canBePlaced)
        {
            inYPositionMode = true;

            stickSpawnedObjectToCursor = false;
            stickPlacedObjectToCursor = false;
        }

        ChangeObjectYPosition(obj);

        if (Input.GetKeyUp(KeyCode.LeftShift) && inYPositionMode && RoomCollision.canBePlaced)
        {
            inYPositionMode = false;

            if (obj)
            {
                obj.tag = "Placed Object";
                obj.GetComponent<Renderer>().material.SetColor("_Color", Color.white);
                obj.GetComponent<Object>().SetObjectValues(ObjectsManager.objects.Count, obj, obj.transform.position, Color.white);

                obj.AddComponent<RoomCollision>();
                obj.GetComponent<MeshCollider>().isTrigger = true;

                ObjectsManager.objects.Add(obj);

                ObjectData objectData = new();
                objectData.SetObjectDataValues(ObjectsManager.objectsData.Count, obj.name, obj.transform.position.x, obj.transform.position.y,
                obj.transform.position.z, Color.white.r, Color.white.g, Color.white.b, Color.white.a);
                ObjectsManager.objectsData.Add(objectData);
            }
            else
            {
                if (placedObject && pickedUpObjectId != -1)
                {
                    Object listedObject = ObjectsManager.objects[pickedUpObjectId].GetComponent<Object>();

                    listedObject.SetObjectValues(pickedUpObjectId, placedObject, placedObject.transform.position, listedObject.Colour);

                    ObjectsManager.objectsData[pickedUpObjectId].SetObjectDataValues(pickedUpObjectId, placedObject.name,
                    placedObject.transform.position.x, placedObject.transform.position.y, placedObject.transform.position.z,
                    listedObject.Colour.r, listedObject.Colour.g, listedObject.Colour.b, listedObject.Colour.a);

                    // Object's current colour
                    placedObject.GetComponent<Renderer>().material.SetColor("_Color", listedObject.Colour);

                    placedObject.AddComponent<RoomCollision>();
                    placedObject.GetComponent<MeshCollider>().isTrigger = true;
                }
            }

            resetValues = true;
        }
    }

    void ChangeObjectYPosition(GameObject obj)
    {
        if (obj)
        {
            if (Input.GetKeyDown(KeyCode.Z) && inYPositionMode && timesPressedZKey != 5)
            {
                obj.transform.position += new Vector3(0f, 0.5f, 0f);

                timesPressedZKey++;
            }
            else if (Input.GetKeyDown(KeyCode.X) && inYPositionMode && ObjectCollision.canBeLowered)
            {
                obj.transform.position -= new Vector3(0f, 0.5f, 0f);

                timesPressedZKey--;
            }
        }
    }

    void PickUpPlacedObject()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl) && canPressLCtrl && placedObject)
        {
            Destroy(placedObject.GetComponent<RoomCollision>());
            placedObject.GetComponent<MeshCollider>().isTrigger = false;

            pickedUpObjectId = placedObject.GetComponent<Object>().Id;

            isPickedUp = true;
            stickPlacedObjectToCursor = true;
            canBeSpawned = false;

            placedObject.GetComponent<Renderer>().material.SetColor("_Color", Color.green);

            ChangeUIObjectsColour(Color.red);
            placeTextBox.SetActive(true);
            modificationTextBox.SetActive(true);
            deleteTextBox.SetActive(true);
        }
    }

    void ResetValues()
    {
        if (resetValues)
        {
            pickedUpObjectId = -1;

            spawnedObject = null;
            placedObject = null;

            isPickedUp = false;
            canBeSpawned = true;
            stickSpawnedObjectToCursor = true;
            stickPlacedObjectToCursor = false;

            timesPressedZKey = 0;

            placeTextBox.SetActive(false);
            modificationTextBox.SetActive(false);
            deleteTextBox.SetActive(false);

            resetValues = false;
        }
    }

    void ChangeUIObjectsColour(Color colour)
    {
        UIBookcase.GetComponent<Renderer>().material.SetColor("_Color", colour);
        UITable.GetComponent<Renderer>().material.SetColor("_Color", colour);
        UIRestChair.GetComponent<Renderer>().material.SetColor("_Color", colour);
    }
}