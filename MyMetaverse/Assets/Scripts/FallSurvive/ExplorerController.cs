using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class ExplorerController : MonoBehaviour
{
    [Header("Component")]
    private Rigidbody2D _rigidbody;
    [SerializeField] private SpriteRenderer spriteRenderer;

    [Header("State Info")]
    [Range(1f, 20f)][SerializeField] private float moveSpeed;
    public float MoveSpeed { get { return moveSpeed; } }

    [SerializeField] private Vector2 moveDirection;
    public Vector2 MoveDirection { get { return moveDirection; } }

    private bool isDead;
    private bool isMoving;
    public bool IsMoving { get { return isMoving; } }


    [Header("Animation Info")]

    [SerializeField] private Animator animator;

    [Header("Layer Info")]
    [SerializeField] private LayerMask obstacleLayer;


    // �ִϸ��̼� ó��
    private static readonly int MoveDir = Animator.StringToHash("MoveDir");
    private static readonly int IsMove = Animator.StringToHash("IsMove");

    // GameManager
    private FallSurviveManager gameManager;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        _rigidbody = GetComponent<Rigidbody2D>();

        gameManager = FallSurviveManager.Instance;
    }

    private void Update()
    {
        if (FallSurviveManager.Instance.CurrentGameState == MiniGameState.Ready) return;

        GetUserInput();
        UpdateAnimation();
    }

    private void FixedUpdate()
    {
        if (FallSurviveManager.Instance.CurrentGameState == MiniGameState.Ready) return;

        Move();
        Rotate();
    }

    #region �Է� ó��

    /// <summary>
    /// ����� �Է� ó��
    /// </summary>
    private void GetUserInput()
    {
        if (isDead) return;

        Vector3 dir = Vector3.ClampMagnitude(new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0f), 1f);
        moveDirection = dir.normalized;
    }

    #endregion

    #region ���� ó��

    /// <summary>
    /// �÷��̾� �̵� ó��
    /// </summary>
    private void Move()
    {
        if (isDead) return;

        Vector3 dir = Vector3.ClampMagnitude(new Vector3(Input.GetAxis("Horizontal"), 0f, 0f), 1f);
        moveDirection = dir.normalized;

        transform.position += dir * moveSpeed * Time.deltaTime;

        isMoving = (moveDirection.magnitude != 0f);
    }

    private void Rotate()
    {
        bool isLeft = moveDirection.x < 0f;
        spriteRenderer.flipX = isLeft;
    }

    #endregion

    #region �ִϸ��̼� ó��

    /// <summary>
    /// �÷��̾� �ִϸ��̼� ó��
    /// </summary>
    private void UpdateAnimation()
    {
        if (animator == null) return;

        bool isMove = moveDirection.sqrMagnitude > 0.01f;
        animator.SetBool(IsMove, isMove);

        if (!isMoving)
            return;

        MoveDirectionState dir = MoveDirectionState.Side;
        animator.SetInteger(MoveDir, (int)dir);
    }

    public enum MoveDirectionState
    {
        Down = 0,
        Up = 1,
        Side = 2
    }

    #endregion

    #region �浹 ó��

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isDead) return;

        // ��ֹ��� ������ Dead
        if ((obstacleLayer & (1 << collision.gameObject.layer)) != 0)
        {
            isDead = true;
            this.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, 90f));
            gameManager.GameOver();
        }
    }

    #endregion
}
