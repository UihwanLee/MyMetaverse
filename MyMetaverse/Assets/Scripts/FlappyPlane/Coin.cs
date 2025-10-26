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
        // x ��ġ�� ������ Coin���� paddding ����ŭ
        float posX = lastPosition.x + padding;

        // y ��ġ�� ������ Coin ��ġ���� ���� ����ŭ ����
        float randomY = Random.Range(minOffsetDist, maxOffsetDist);
        float posY = lastPosition.y + randomY;

        transform.position = new Vector3(posX, posY, 0);

        // Position ����
        return transform.position;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlaneController plane = collision.GetComponent<PlaneController>();
        if (plane != null)
        {
            // Plane�� �浹 �� Get �ִϸ��̼� ����
            animator.SetBool(IsGetCoin, true);
            gameManager.AddCoin(coinValue);
        }
    }
}
