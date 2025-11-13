using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameWinUI : MonoBehaviour
{
    [SerializeField] private Button backMainMenuButton;

    private void Awake()
    {
        backMainMenuButton.onClick.AddListener(() =>
        {
            SaveManager.DeleteSaveFile();
            SceneManager.LoadScene("MainMenu");
        });
    }
}
