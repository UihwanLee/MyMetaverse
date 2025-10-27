using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallSurviveLooper : MonoBehaviour
{
    [Header("Background Value")]
    [SerializeField] private int bgCount = 5;

    [Header("Ground Object")]
    [SerializeField] private Transform groundParent;
    [SerializeField] private Vector3 startPosition = new Vector3(0f, 0.35f, 0f);
    [SerializeField] private int groundCount = 10;
    [SerializeField] private GameObject groundPrefab;
    private Vector3 lastPositionGround;

    private void Start()
    {
        PlaceAllGrounds();
    }

    private void PlaceAllGrounds()
    {
        if (groundPrefab == null) return;

        lastPositionGround = startPosition;
        for (int i = 0; i < groundCount; i++)
        {
            var ground = Instantiate(groundPrefab, groundParent).GetComponent<Ground>();
            lastPositionGround = ground.SetRandomPlace(lastPositionGround);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("BackGround"))
        {
            // 오브젝트 height 값 구하기
            
            float heightOffset = (collision.name.Contains("ground")) ? ((BoxCollider2D)collision).size.x : ((BoxCollider2D)collision).size.y;
            Vector3 pos = collision.transform.position;

            // 다시 배치
            pos.y -= heightOffset * bgCount;
            collision.transform.position = pos;

            return;
        }

        Ground ground = collision.gameObject.GetComponent<Ground>();
        if (ground)
        {
            lastPositionGround = ground.SetRandomPlace(lastPositionGround);
        }
    }
}
