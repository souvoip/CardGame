using System;

[Serializable]
public class Block : ISaveLoad
{
    public int Value;
    public float Rate;

    public Block(int value, int rate) { Value = value; Rate = rate; }

    public Block(Block bk)
    {
        Value = bk.Value;
        Rate = bk.Rate;
    }

    public Block(JSONObject data)
    {
        Load(data);
    }

    public int GetBlockValue() { return (int)(Value * Rate); }

    public JSONObject Save()
    {
        JSONObject data = JSONObject.Create();
        data.AddField("Value", Value);
        data.AddField("Rate", Rate);
        return data;
    }

    public void Load(JSONObject data)
    {
        Value = (int)data.GetField("Value").i;
        Rate = data.GetField("Rate").n;
    }
}