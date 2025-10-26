using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    [Header("Setting Value")]
    [SerializeField] private float minOffsetDist;
    [SerializeField] private float maxOffsetDist;
    [SerializeField] private float padding;

    [SerializeField] private int coinValue = 5;

    private readonly int IsGetCoin = Animator.StringToHash("IsGetCoin");
    private Animator animator;
    public Animator Animator {  get { return animator; } }

    // GameManager
    private FlappyPlaneManager gameManager;

    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
        this.gameManager = FlappyPlaneManager.Instance;
    }

    public Vector3 SetRandomPlace(Vector3 lastPosition)
    {
        // x 위치는 마지막 Coin에서 paddding 값만큼
        float posX = lastPosition.x + padding;

        // y 위치는 마지막 Coin 위치에서 랜덤 값만큼 조정
        float randomY = Random.Range(minOffsetDist, maxOffsetDist);
        float posY = lastPosition.y + randomY;

        transform.position = new Vector3(posX, posY, 0);

        // Position 전달
        return transform.position;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlaneController plane = collision.GetComponent<PlaneController>();
        if (plane != null)
        {
            // Plane과 충돌 시 Get 애니메이션 적용
            animator.SetBool(IsGetCoin, true);
            gameManager.AddCoin(coinValue);
        }
    }
}
