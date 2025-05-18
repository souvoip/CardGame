using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEventTest : MonoBehaviour
{
    [SerializeField]
    private GameEventData tempData;

    [SerializeField]
    private GameEventUI gameEventUI;

    private void Start()
    {
        gameEventUI.Show(tempData);
    }
}
