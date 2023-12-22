[System.Serializable]
public class ObjectData
{
    public int id;
    public string type;
    public float locationX, locationY, locationZ;
    public float colourR, colourG, colourB, colourA;

    public ObjectData()
    {

    }

    public void SetObjectDataValues(int id, string type, float locationX, float locationY, float locationZ,
    float colourR, float colourG, float colourB, float colourA)
    {
        this.id = id;
        this.type = type;
        this.locationX = locationX;
        this.locationY = locationY;
        this.locationZ = locationZ;
        this.colourR = colourR;
        this.colourG = colourG;
        this.colourB = colourB;
        this.colourA = colourA;
    }
}