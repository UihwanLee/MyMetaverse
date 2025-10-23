using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;

    public static GameManager Instance
    {
        get
        {
            return instance;
        }
    }

    public PlayerController player { get; private set; }

    private void Awake()
    {
        instance = this;
        player = GameObject.FindObjectOfType<PlayerController>();
    }


}
