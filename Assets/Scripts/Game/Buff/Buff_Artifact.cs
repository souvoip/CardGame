public class Buff_Artifact : BuffBase
{
    private bool isConsumed;

    // 当尝试施加减益时触发
    public bool TryBlockDebuff()
    {
        if (isConsumed) return false;

        isConsumed = true;
        RemoveBuff();
        return true;
    }
}