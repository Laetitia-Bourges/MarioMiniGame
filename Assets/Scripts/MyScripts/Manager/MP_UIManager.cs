using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MP_UIManager : MP_Singleton<MP_UIManager>
{
    [SerializeField] Button constantQuitButton = null, quitButon = null, restartButton = null, playButton = null;
    [SerializeField] TMP_Text marioPlayerName = null, luigiPlayerName = null, winnerTextBox = null;
    [SerializeField] TMP_InputField marioInputField = null, luigiInputField = null;
    [SerializeField] GameObject mainMenu = null, winnerMenu = null;
    bool canMarioStart = false, canLuigiStart = false;

    public bool IsUIValid => constantQuitButton && quitButon && restartButton && playButton && marioPlayerName && luigiPlayerName && winnerTextBox
        && mainMenu && winnerMenu && marioInputField && luigiInputField;

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
        marioInputField.onEndEdit.AddListener((s) => LockInput(marioInputField, ref canMarioStart));
        luigiInputField.onEndEdit.AddListener((s) => LockInput(luigiInputField, ref canLuigiStart));
    }
    void QuitGame() => Application.Quit();
    void RestartGame()
    {
        winnerMenu.SetActive(false);
        mainMenu.SetActive(true);
    }
    void PlayGame()
    {
        if (!canMarioStart || !canLuigiStart) return;
        MP_GameManager.Instance?.SetPlayers(new List<string>() { marioPlayerName.text, luigiPlayerName.text });
        MP_GameManager.Instance?.StartGame();
        mainMenu.SetActive(false);
    }

    public void ShowWinnerMenu(string _winnerName)
    {
        winnerTextBox.text = $"{_winnerName} won this party !!";
        winnerMenu.SetActive(true);
    }
    void LockInput(TMP_InputField input, ref bool _canStart)
    {
        if (input.text.Length > 0)
            _canStart = true;
        else if (input.text.Length == 0)
            _canStart = false;
    }
}
