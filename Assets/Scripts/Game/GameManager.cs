using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        BuffDataManager.Init();
        CardDataManager.Init();
        CharacterDataManager.Init();
    }
}
