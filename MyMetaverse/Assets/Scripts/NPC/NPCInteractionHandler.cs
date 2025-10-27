using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NPCInteractionHandler : MonoBehaviour
{
    [SerializeField] private DialogManager dialogManager;
    [SerializeField] private GameObject InteractionUI;

    private IInteractable interactable;

    private void Start()
    {
        interactable = GetComponent<IInteractable>();
    }

    public void ShowInteractionUI(bool show)
    {
        InteractionUI.SetActive(show);
    }

    public void Interact()
    {
        interactable?.Interact(dialogManager);
    }

    public void InteractDialog()
    {
        interactable?.InteractDialog(dialogManager);
    }
}

public interface IInteractable
{
    void Interact(DialogManager dialogManager);
    void InteractDialog(DialogManager dialogManager);
}
