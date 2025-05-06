using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemainsItemChangeAttribute : RemainsItem
{
    public ETriggerTime TriggerTime;

    public ERoleAttribute ChangeAttribute;

    public EBuffTarget Target;

    public override void OnAcquire()
    {
    }
}
