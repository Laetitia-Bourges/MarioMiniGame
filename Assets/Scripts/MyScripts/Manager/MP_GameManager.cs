using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class MP_GameManager : MP_Singleton<MP_GameManager>
{
    public event Action OnTimerEnd = null;
    public event Action OnGameEnd = null;

    #region Fields/Properties
    [SerializeField] Transform spawnObstaclesPosition = null;
    [SerializeField] GameObject obstacleObject = null;
    [SerializeField, Range(0, 100)] float minTimer = 5, maxTimer = 20;
    [SerializeField, Range(0, 10)] int minObstacles = 1, maxObstacles = 5;
    [SerializeField] List<MP_Player> players = new List<MP_Player>();
    float timer = 0, maxCurrentTimer = 0;
    int currentNbObstacles = 0;
    int nbPlayerAlive = 0;
    bool isStartGame = false;

    public bool IsValid => spawnObstaclesPosition && obstacleObject && players.Count > 1;
    #endregion

    #region UnityMethods
    private void Update() => UpdateTimer();
    private void OnDestroy()
    {
        OnTimerEnd = null;
        OnGameEnd = null;
    }
    #endregion

    #region CustomMethods
    public void StartGame()
    {
        if (!IsValid) return;
        SetNewTimer();
        OnGameEnd += FinishGame;
        OnTimerEnd += () =>
        {
            SetNewTimer();
            StartCoroutine(InstantiateObstacle());
        };
        isStartGame = true;
    }
    #region Timer
    void SetNewTimer()
    {
        nbPlayerAlive = players.Count;
        currentNbObstacles = Random.Range(minObstacles, maxObstacles);
        maxCurrentTimer = Random.Range(minTimer, maxTimer);
        timer = 0;
    }
    void UpdateTimer()
    {
        if (!IsValid || !isStartGame) return;
        timer += Time.deltaTime;
        if (timer > maxCurrentTimer)
            OnTimerEnd?.Invoke();
    }
    #endregion
    #region Players
    public void VerifyPlayers()
    {
        nbPlayerAlive--;
        if (nbPlayerAlive <= 1) OnGameEnd?.Invoke();
    }
    public void SetPlayers(List<string> _names)
    {
        for (int i = 0; i < _names.Count; i++)
            players[i].InitPlayer(_names[i]);
    }
    string GetWinnerPlayer()
    {
        for (int i = 0; i < players.Count; i++)
            if (players[i].IsAlive) return players[i].Name;
        return "";
    }
    #endregion
    IEnumerator InstantiateObstacle()
    {
        for (int i = 0; i < currentNbObstacles; i++)
        {
            GameObject _obstacle = Instantiate(obstacleObject, spawnObstaclesPosition);
            _obstacle.transform.position = spawnObstaclesPosition.position;
            yield return new WaitForSeconds(.1f);
        }
    }
    public void FinishGame()
    {
        isStartGame = false;
        MP_Obstacles[] _obstacles = FindObjectsOfType<MP_Obstacles>();
        for (int i = 0; i < _obstacles.Length; i++)
            Destroy(_obstacles[i].gameObject);
        MP_UIManager.Instance?.ShowWinnerMenu(GetWinnerPlayer());
    }
    #endregion
}
