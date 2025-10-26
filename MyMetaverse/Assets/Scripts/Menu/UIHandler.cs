using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIHandler : MonoBehaviour
{
    [SerializeField] private GameObject ui;

    private bool isUIActive = false;
    public void OnClickUIButton()
    {
        isUIActive = !isUIActive;
        ui.SetActive(isUIActive);
    }
}
