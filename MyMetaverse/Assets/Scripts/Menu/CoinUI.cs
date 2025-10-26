using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CoinUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI coinTxt;

    private GameManager gameManager;

    private void Start()
    {
        gameManager = GameManager.Instance;
    }

    private void Update()
    {
        UpdateCoin(gameManager.Player.CurrentCoin);
    }

    public void UpdateCoin(int coin)
    {
        coinTxt.text = coin.ToString();
    }
}
