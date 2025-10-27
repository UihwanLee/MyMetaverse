using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public enum GameState
{
    Playing,
    UI_Active,
    Event,
}

public class PlayerData
{
    public Vector3 Position { get; set; }
    public Material PlayerMaterial { get; set; }

    public int PlayerCoin { get; set; }

    public PlayerData(Vector3 position, Material material, int coin)
    {
        Position = position;
        PlayerMaterial = material;
        PlayerCoin = coin;
    }
}

public class GameManager : MonoBehaviour
{
    private static GameManager instance;

    public static GameManager Instance
    {
        get
        {
            return instance;
        }
    }

    private GameState currentState;
    public GameState CurrentState { get { return currentState; } }

    private PlayerData playerData;
    public PlayerController Player { get; private set; }

    public MiniGameBase currentMiniGame { get; private set; }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        currentState = GameState.Playing;
        Player = GameObject.FindObjectOfType<PlayerController>();
        playerData = new PlayerData(Player.transform.position, Player.GetComponentInChildren<SpriteRenderer>().material, Player.CurrentCoin);
    }

    public void SavePlayer()
    {
        Vector3 pos = Player.transform.position;
        Material material = Player.GetComponentInChildren<SpriteRenderer>().material;
        int coin = Player.CurrentCoin;
        playerData.Position = pos;
        playerData.PlayerMaterial = material;
        playerData.PlayerCoin = coin;
    }

    public void UpdatePlayer(PlayerController player)
    {
        this.Player = player;

        if (playerData != null)
        {
            this.Player.transform.position = playerData.Position;
            this.Player.GetComponentInChildren<SpriteRenderer>().material = playerData.PlayerMaterial;
            this.Player.CurrentCoin = playerData.PlayerCoin;
        }
    }

    public void SetCurrentMiniGame(MiniGameBase miniGame)
    {
        this.currentMiniGame = miniGame;
        Debug.Log($"GameManager: {miniGame.MiniGameName} 미니게임이 현재 게임으로 설정되었습니다.");
    }

    public void StartMiniGame()
    {
        SavePlayer();
        ChangeGameState(GameState.Event);
        SceneController.Instance.LoadScene(1);
    }

    public void StartMiniGame2()
    {
        SavePlayer();
        ChangeGameState(GameState.Event);
        SceneController.Instance.LoadScene(2);
    }

    public void ChangeGameState(GameState state)
    {
        currentState = state;
    }

    public void UpdateCoin(int coin)
    {
        playerData.PlayerCoin += coin;
    }

    public void GameQuit()
    {
        Application.Quit();
    }

    /// <summary>
    /// 현재 실행 중인 미니게임의 인스턴스를 원하는 타입으로 반환합니다.
    /// </summary>
    public T CurrentMiniGame<T>() where T : MiniGameBase
    {
        return currentMiniGame as T;
    }
}
