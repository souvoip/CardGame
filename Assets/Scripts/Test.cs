using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    private void Start()
    {
        CardDataManager.Init();
        var ta = CardDataManager.GetAtkCard(1);
    }
}
