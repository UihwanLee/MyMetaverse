using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class Obstacle : MonoBehaviour
{
    [Header("Child Object")]
    [SerializeField] private Transform top;
    [SerializeField] private Transform bottom;

    [Header("Setting Value")]
    [SerializeField] private float minOffsetDist;
    [SerializeField] private float maxOffsetDist;
    [SerializeField] private float padding;

    // GameManager
    private FlappyPlaneManager gameManager;

    private void Start()
    {
        this.gameManager = FlappyPlaneManager.Instance;
    }

    public Vector3 SetRandomPlace(Vector3 lastPosition)
    {
        // x 위치는 마지막 Obstacle에서 paddding 값만큼
        float posX = lastPosition.x + padding;
        transform.position = new Vector3(posX, 0, 0);

        // 랜덤 배치 : top과 bottom 거리 벌리기
        float holeSize = Random.Range(minOffsetDist, maxOffsetDist);
        float halfHoleSize = holeSize / 2;
        top.localPosition = new Vector3(0, halfHoleSize);
        bottom.localPosition = new Vector3(0, -halfHoleSize);

        // Position 전달
        return transform.position;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        PlaneController plane = collision.GetComponent<PlaneController>();
        if (plane != null)
            gameManager.AddScore(1);
    }
}
