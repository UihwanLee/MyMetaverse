using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NPCInteractionHandler : MonoBehaviour
{
    [SerializeField] private GameObject InteractionUI;

    private IInteractable interactable;

    private void Awake()
    {
        interactable = GetComponent<IInteractable>();
    }

    public void ShowInteractionUI(bool show)
    {
        InteractionUI.SetActive(show);
    }

    public void Interact()
    {
        interactable?.Interact();
    }
}

public interface IInteractable
{
    void Interact();
}
