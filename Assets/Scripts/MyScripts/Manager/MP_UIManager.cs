﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MP_UIManager : MP_Singleton<MP_UIManager>
{
    [SerializeField] Button constantQuitButton = null, quitButon = null, restartButton = null, playButton = null;
    [SerializeField] TMP_Text marioPlayerName = null, luigiPlayerName = null, winnerTextBox = null;
    [SerializeField] GameObject mainMenu = null, winnerMenu = null;

    public bool IsUIValid => constantQuitButton && quitButon && restartButton && playButton && marioPlayerName && luigiPlayerName && winnerTextBox
        && mainMenu && winnerMenu;

    private void Start()
    {
        InitButtons();
    }
    void InitButtons()
    {
        if (!IsUIValid) return;
        constantQuitButton.onClick.AddListener(QuitGame);
        quitButon.onClick.AddListener(QuitGame);
        restartButton.onClick.AddListener(RestartGame);
        playButton.onClick.AddListener(PlayGame);
    }
    void QuitGame() => Application.Quit();
    void RestartGame()
    {
        winnerMenu.SetActive(false);
        mainMenu.SetActive(true);
    }
    void PlayGame()
    {
        if (string.IsNullOrEmpty(marioPlayerName.text) || string.IsNullOrEmpty(luigiPlayerName.text)) return;
        Debug.Log("test");
        MP_GameManager.Instance?.SetPlayers(new List<string>() { marioPlayerName.text, luigiPlayerName.text });
        MP_GameManager.Instance?.StartGame();
        mainMenu.SetActive(false);
    }

    public void ShowWinnerMenu(string _winnerName)
    {
        winnerTextBox.text = $"{_winnerName} won this party !!";
        winnerMenu.SetActive(true);
    }
}
