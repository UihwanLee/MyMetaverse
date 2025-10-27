using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MiniGameSingleton<T> : MiniGameBase where T : MiniGameSingleton<T>
{
    public static T Instance { get; private set; }

    protected virtual void Awake()
    {
        currentGameState = MiniGameState.Ready;
        Instance = (T)this;
    }
}
