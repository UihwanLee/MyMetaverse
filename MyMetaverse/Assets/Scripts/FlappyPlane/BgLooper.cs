using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BgLooper : MonoBehaviour
{
    [Header("Background Value")]
    [SerializeField] private int bgCount = 5;

    [Header("Obsatcle Object")]
    [SerializeField] private Obstacle[] obstacles;
    private Vector3 lastPosition;

    private void Start()
    {
        obstacles = GameObject.FindObjectsOfType<Obstacle>();
        PlaceAllObstacle();
    }

    private void PlaceAllObstacle()
    {
        if(obstacles == null) return;

        lastPosition = obstacles[0].transform.position;
        for(int i=0; i<obstacles.Length; i++)
        {
            lastPosition = obstacles[i].SetRandomPlace(lastPosition);
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

        Obstacle obstacle = collision.gameObject.GetComponent<Obstacle>();
        if(obstacle)
        {
            lastPosition = obstacle.SetRandomPlace(lastPosition);
        }
    }
}
