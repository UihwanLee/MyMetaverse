using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatNPC : MonoBehaviour, IInteractable
{
    [SerializeField] private Dialog dialog;

    public void Interact(DialogManager dialogManager)
    {
        GameManager.Instance.ChangeGameState(GameState.Dialog);
        dialogManager.SetDialog(dialog);
        dialogManager.StartDialogTyping();
    }

    public void InteractDialog(DialogManager dialogManager)
    {
        dialogManager.DialogInteract();
    }
}
