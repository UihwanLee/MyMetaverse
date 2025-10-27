using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    [Header("Setting Value")]
    [SerializeField] private bool isForwardX;
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
        float randomValue = Random.Range(minOffsetDist, maxOffsetDist);

        // x, y위치는 상하좌우 움직임에 따라 다름

        float posX = (isForwardX) ? lastPosition.x + padding : lastPosition.x + randomValue;
        float posY = (isForwardX) ? lastPosition.y + randomValue : lastPosition.y + padding;

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
