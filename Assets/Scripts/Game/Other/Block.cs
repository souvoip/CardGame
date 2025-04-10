using System;

[Serializable]
public class Block
{
    public int Value;
    public float Rate;

    public Block(int value, int rate) { Value = value; Rate = rate; }

    public Block(Block bk)
    {
        Value = bk.Value;
        Rate = bk.Rate;
    }

    public int GetBlockValue() { return (int)(Value * Rate); }
}