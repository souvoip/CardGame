
public class Buff_Strength : BuffBase
{
    public override void Initialize(CharacterBase owner, int initStacks = 1)
    {
        base.Initialize(owner, initStacks);
        //owner.Strength += currentStacks;
    }

    public override void AddStacks(int amount)
    {
        base.AddStacks(amount);
        //target.Strength += amount;
    }

    public override void RemoveBuff()
    {
        //target.Strength -= currentStacks;
        base.RemoveBuff();
    }
}
