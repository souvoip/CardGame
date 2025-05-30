using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField]
    private Button startBtn;

    [SerializeField]
    private Button continueBtn;

    [SerializeField]
    private SceneTransition loadingPanel;

    private void Awake()
    {
        startBtn.onClick.AddListener(OnStartBtnClick);
        continueBtn.onClick.AddListener(OnContinueBtnClick);

        if (!SaveManager.SaveFileExists())
        {
            continueBtn.interactable = false;
        }
    }

    private void OnStartBtnClick()
    {
        GameManager.isLoadGame = false;
        loadingPanel.FadeIn(() =>
        {
            SceneManager.LoadScene("Game");
        });
    }

    private void OnContinueBtnClick()
    {
        GameManager.isLoadGame = true;
        loadingPanel.FadeIn(() =>
        {
            SceneManager.LoadScene("Game");
        });
    }
}
