using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoliderNPC : MonoBehaviour, IInteractable
{
    [SerializeField] private CustomizeManager customizeManager;
    [SerializeField] private GameObject customizingUI;

    public void Interact()
    {
        customizingUI.SetActive(true);
        customizeManager.InitAvatarColor();
    }
}
