using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MiniGameBase : MonoBehaviour
{
    protected GameManager mainGameManager;

    public abstract string MiniGameName { get; }

    protected MiniGameState currentGameState;
    public MiniGameState CurrentGameState { get { return currentGameState; } set { currentGameState = value; } }

    public virtual void Init(GameManager gm)
    {
        mainGameManager = gm;
    }

    public virtual void Exit()
    {
        Debug.Log($"{MiniGameName} Á¾·áµÊ");
    }
}
