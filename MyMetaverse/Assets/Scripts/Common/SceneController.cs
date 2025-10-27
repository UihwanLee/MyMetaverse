using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public static SceneController Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;

            // 씬 전환 로직을 유지하기 위해 파괴되지 않게 설정
            DontDestroyOnLoad(gameObject);

            // 씬 로드 이벤트를 구독하여 모든 씬 로드 후 처리를 여기서 담당
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded; 
    }

    public void LoadScene(int index)
    {
        SceneManager.LoadScene(index);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // 씬 로드가 완료되면 Player 객체 갱신
        PlayerController player = GameObject.FindObjectOfType<PlayerController>();

        if (player != null)
        {
            GameManager.Instance.UpdatePlayer(player);
        }
    }
}
