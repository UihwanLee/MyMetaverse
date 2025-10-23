using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    [Header("Component")]
    [SerializeField] private SpriteRenderer spriteRenderer;

    [Header("State Info")]
    [Range(1f, 20f)][SerializeField] private float moveSpeed;
    public float MoveSpeed { get { return moveSpeed; } }

    [SerializeField] private Vector2 moveDirection;
    public Vector2 MoveDirection { get { return moveDirection; } }

    private Rigidbody2D _rigidbody;

    public bool isMoving;
    public bool IsMoving { get { return isMoving; } }

    [Header("Animation Info")]

    // 애니메이션 처리
    private static readonly int MoveDir = Animator.StringToHash("MoveDir");
    private static readonly int IsMove = Animator.StringToHash("IsMove");

    [SerializeField] private Animator animator;


    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        GetUserInput();
        UpdateAnimation();
    }

    private void FixedUpdate()
    {
        Move();
        Rotate();
    }

    #region 입력 처리

    /// <summary>
    /// 사용자 입력 처리
    /// </summary>
    private void GetUserInput()
    {
        Vector3 dir = Vector3.ClampMagnitude(new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0f), 1f);
        moveDirection = dir.normalized;
    }

    #endregion


    #region 동작 처리

    /// <summary>
    /// 플레이어 이동 처리
    /// </summary>
    private void Move()
    {
        Vector3 dir = Vector3.ClampMagnitude(new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0f), 1f);
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

    #region 애니메이션 처리

    /// <summary>
    /// 플레이어 애니메이션 처리
    /// </summary>
    private void UpdateAnimation()
    {
        if (animator == null) return;

        bool isMove = moveDirection.sqrMagnitude > 0.01f;
        animator.SetBool(IsMove, isMove);

        if (!isMoving)
            return;

        MoveDirectionState dir = GetMoveDirection(moveDirection);
        animator.SetInteger(MoveDir, (int)dir);
    }

    private MoveDirectionState GetMoveDirection(Vector2 dir)
    {
        if (Mathf.Abs(dir.y) > Mathf.Abs(dir.x))
        {
            if (dir.y > 0)
                return MoveDirectionState.Down;
            else
                return MoveDirectionState.Up;
        }
        else
        {
            return MoveDirectionState.Side;
        }
    }

    public enum MoveDirectionState
    {
        Down = 0,
        Up = 1,
        Side = 2
    }

    #endregion
}
