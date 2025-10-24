using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneController : MonoBehaviour
{
    private Rigidbody2D _rigidbody;
    private Animator animator;

    [Range(1f, 20f)][SerializeField] private float moveSpeed;
    [Range(1f, 20f)][SerializeField] private float jumpForce;

    private bool isJumping;
    private bool isDead;

    // �ִϸ��̼� ó��
    private static readonly int IsDie = Animator.StringToHash("IsDie");

    // GameManager
    private FlappyPlaneManager gameManager;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();

        isDead = false;
        isJumping = false;

        this.gameManager = FlappyPlaneManager.Instance;
    }

    private void Update()
    {
        GetUserInput();
    }

    #region �Է� ó��

    private void GetUserInput()
    {
        if (isDead) return;

        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
            isJumping = true;
    }

    #endregion

    private void FixedUpdate()
    {
        Movement();
    }

    #region ���� ó��

    private void Movement()
    {
        if (isDead) return;

        Vector3 velocitiy = _rigidbody.velocity;

        // ��ҿ��� ����
        velocitiy.x = moveSpeed;

        if(isJumping)
        {
            velocitiy.y += jumpForce;
            isJumping = false;
        }

        _rigidbody.velocity = velocitiy;

        // �ޱ� ����
        float angle = Mathf.Clamp(velocitiy.y * 10f, -90f, 90f);
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    #endregion

    #region �浹 ó��

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isDead) return;

        // �ƹ� ������Ʈ�̵� �浹�ϸ� ���ó��
        isDead = true;

        animator.SetInteger(IsDie, 1);

        gameManager.GameOver();
    }

    #endregion
}
