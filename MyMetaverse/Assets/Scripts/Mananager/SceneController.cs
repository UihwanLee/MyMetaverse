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

            // �� ��ȯ ������ �����ϱ� ���� �ı����� �ʰ� ����
            DontDestroyOnLoad(gameObject);

            // �� �ε� �̺�Ʈ�� �����Ͽ� ��� �� �ε� �� ó���� ���⼭ ���
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
        // �� �ε尡 �Ϸ�Ǹ� Player ��ü ����
        PlayerController player = GameObject.FindObjectOfType<PlayerController>();

        if (player != null)
        {
            GameManager.Instance.UpdatePlayer(player);
        }
    }
}
