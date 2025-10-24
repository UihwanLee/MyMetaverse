using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NPCController : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject InteractionUI;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerController player = collision.gameObject.GetComponent<PlayerController>();
        if (player != null)
        {
            InteractionUI.SetActive(true);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        PlayerController player = collision.gameObject.GetComponent<PlayerController>();
        if (player != null)
        {
            // FŰ�� ������ Scene ��ȯ
            bool isInteraction = Input.GetKeyDown(KeyCode.F);
            if (isInteraction)
                SceneManager.LoadScene(1);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        PlayerController player = collision.gameObject.GetComponent<PlayerController>();
        if (player != null)
        {
            InteractionUI.SetActive(false);
        }
    }
}
