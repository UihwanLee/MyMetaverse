using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoliderNPC : MonoBehaviour, IInteractable
{
    [SerializeField] private CustomizeManager customizeManager;
    [SerializeField] private GameObject customizingUI;

    public void Interact()
    {
        GameManager.Instance.ChangeGameState(GameState.UI_Active);
        customizingUI.SetActive(true);
        customizeManager.InitAvatarColor();
    }
}
