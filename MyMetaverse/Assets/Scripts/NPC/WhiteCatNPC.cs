using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhiteCatNPC : MonoBehaviour, IInteractable
{
    public void Interact()
    {
        GameManager.Instance.StartMiniGame();
    }
}
