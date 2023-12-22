using UnityEngine;

public class Object : MonoBehaviour
{
    public int Id { get; private set; }
    public GameObject Type { get; private set; }
    public Vector3 Location { get; private set; }
    public Color Colour { get; private set; }

    public void SetObjectValues(int id, GameObject type, Vector3 location, Color colour)
    {
        this.Id = id;
        this.Type = type;
        this.Location = location;
        this.Colour = colour;
    }

    public void SetColour(Color colour)
    {
        this.Colour = colour;
    }
}