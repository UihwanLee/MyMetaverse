using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    public PlayerController player { get; private set; }

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
        player = GameObject.FindObjectOfType<PlayerController>();
    }

    public void UpdatePlayer(PlayerController player)
    {
        this.player = player;
    }

    public void StartMiniGame()
    {
        SceneController.Instance.LoadScene(1);
    }
}
