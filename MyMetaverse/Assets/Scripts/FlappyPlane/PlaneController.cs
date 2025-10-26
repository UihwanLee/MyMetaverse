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

    // 코인 LayerMask
    [SerializeField] private LayerMask coinLayerMask;

    // 애니메이션 처리
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
        if (FlappyPlaneManager.Instance.CurrentGameState == MiniGameState.Ready) return;

        GetUserInput();
    }

    #region 입력 처리

    private void GetUserInput()
    {
        if (isDead) return;

        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
            isJumping = true;
    }

    #endregion

    private void FixedUpdate()
    {
        SetGameReady();

        if (FlappyPlaneManager.Instance.CurrentGameState == MiniGameState.Ready) return;

        Movement();
    }

    #region 동작 처리

    private void SetGameReady()
    {
        // 게임 준비 상태라면 Rigidbody kinematic으로 변경
        MiniGameState state = FlappyPlaneManager.Instance.CurrentGameState;

        RigidbodyType2D bodyType = (state == MiniGameState.Ready) ? RigidbodyType2D.Kinematic : RigidbodyType2D.Dynamic;

        this._rigidbody.bodyType = bodyType;
    }

    private void Movement()
    {
        if (isDead) return;

        Vector3 velocitiy = _rigidbody.velocity;

        // 평소에는 전진
        velocitiy.x = moveSpeed;

        if(isJumping)
        {
            velocitiy.y += jumpForce;
            isJumping = false;
        }

        _rigidbody.velocity = velocitiy;

        // 앵글 조정
        float angle = Mathf.Clamp(velocitiy.y * 10f, -90f, 90f);
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    #endregion

    #region 충돌 처리

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isDead) return;

        // 아무 오브젝트이든 충돌하면 사망처리
        isDead = true;

        animator.SetInteger(IsDie, 1);

        gameManager.GameOver();
    }

    #endregion
}
