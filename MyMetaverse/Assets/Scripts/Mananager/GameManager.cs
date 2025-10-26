using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class PlayerData
{
    public Vector3 Position { get; set; }
    public Material PlayerMaterial { get; set; }

    public PlayerData(Vector3 position, Material material)
    {
        Position = position;
        PlayerMaterial = material;
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

    private PlayerData playerData;
    public PlayerController Player { get; private set; }

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
        Player = GameObject.FindObjectOfType<PlayerController>();
        playerData = new PlayerData(Player.transform.position, Player.GetComponentInChildren<SpriteRenderer>().material);
    }

    public void SavePlayer()
    {
        Vector3 pos = Player.transform.position;
        Material material = Player.GetComponentInChildren<SpriteRenderer>().material;
        playerData.Position = pos;
        playerData.PlayerMaterial = material;
    }

    public void UpdatePlayer(PlayerController player)
    {
        this.Player = player;

        if (playerData != null)
        {
            this.Player.transform.position = playerData.Position;
            this.Player.GetComponentInChildren<SpriteRenderer>().material = playerData.PlayerMaterial;
        }
    }

    public void StartMiniGame()
    {
        SavePlayer();
        SceneController.Instance.LoadScene(1);
    }
}
