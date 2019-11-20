using System.Collections;

[System.Serializable]
public class Value
{
    public uint value;
    public int indexID;

    public Value(int indexNumber)
    {
        indexID = indexNumber;
    }
}