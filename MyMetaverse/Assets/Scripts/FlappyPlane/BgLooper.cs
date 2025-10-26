using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BgLooper : MonoBehaviour
{
    [Header("Background Value")]
    [SerializeField] private int bgCount = 5;

    [Header("Obsatcle Object")]
    [SerializeField] private Obstacle[] obstacles;
    private Vector3 lastPositionObstacle;

    [Header("Coin Object")]
    [SerializeField] private LayerMask coinLayer;
    [SerializeField] private int coinCount = 20;
    [SerializeField] private Coin coinPrefab;
    private Vector3 lastPositionCoin;

    private void Start()
    {
        obstacles = GameObject.FindObjectsOfType<Obstacle>();
        PlaceAllObstacle();
        PlaceAllCoins();
    }

    private void PlaceAllObstacle()
    {
        if(obstacles == null) return;

        lastPositionObstacle = obstacles[0].transform.position;
        for(int i=0; i<obstacles.Length; i++)
        {
            lastPositionObstacle = obstacles[i].SetRandomPlace(lastPositionObstacle);
        }
    }

    private void PlaceAllCoins()
    {
        if (coinPrefab == null) return;

        lastPositionCoin = new Vector3(3.5f, 0f, 0f);
        for (int i=0; i<coinCount; i++)
        {
            var coin = Instantiate(coinPrefab).GetComponent<Coin>();
            lastPositionCoin = coin.SetRandomPlace(lastPositionCoin);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("BackGround"))
        {
            // 오브젝트 width 값 구하기
            float widthOffset = ((BoxCollider2D)collision).size.x;
            Vector3 pos = collision.transform.position;

            // 다시 배치
            pos.x += widthOffset * bgCount;
            collision.transform.position = pos;

            return;
        }

        Coin coin;
        if ((coinLayer & (1 << collision.gameObject.layer)) != 0)
        {
            coin = collision.gameObject.GetComponent<Coin>();
            coin.Animator.SetBool("IsGetCoin", false);
            lastPositionCoin = coin.SetRandomPlace(lastPositionCoin);
        }

        Obstacle obstacle = collision.gameObject.GetComponent<Obstacle>();
        if(obstacle)
        {
            lastPositionObstacle = obstacle.SetRandomPlace(lastPositionObstacle);
        }
    }
}
