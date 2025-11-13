using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
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

    [SerializeField]
    private GameObject selectRolePanel;

    [SerializeField]
    private Button role1Btn;
    [SerializeField]
    private Button role2Btn;
    [SerializeField]
    private Button backBtn;

    private void Awake()
    {
        startBtn.onClick.AddListener(OnStartBtnClick);
        continueBtn.onClick.AddListener(OnContinueBtnClick);
        role1Btn.onClick.AddListener(() => { RoleSelected(1); });
        role2Btn.onClick.AddListener(() => { RoleSelected(2); });
        backBtn.onClick.AddListener(() => { selectRolePanel.gameObject.SetActive(false); });

        if (!SaveManager.SaveFileExists())
        {
            continueBtn.interactable = false;
        }
    }

    private void OnStartBtnClick()
    {
        GameManager.isLoadGame = false;
        selectRolePanel.gameObject.SetActive(true);
    }

    private void OnContinueBtnClick()
    {
        GameManager.isLoadGame = true;
        loadingPanel.FadeIn(() =>
        {
            SceneManager.LoadScene("Game");
        });
    }

    private void RoleSelected(int role)
    {
        GameManager.selectRole = role;
        loadingPanel.FadeIn(() =>
        {
            SceneManager.LoadScene("Game");
        });
    }
}
