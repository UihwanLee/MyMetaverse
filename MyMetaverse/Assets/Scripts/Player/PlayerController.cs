using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    [Header("Component")]
    private Rigidbody2D _rigidbody;
    [SerializeField] private SpriteRenderer spriteRenderer;

    [Header("State Info")]
    [Range(1f, 20f)][SerializeField] private float moveSpeed;
    public float MoveSpeed { get { return moveSpeed; } }

    [SerializeField] private Vector2 moveDirection;
    public Vector2 MoveDirection { get { return moveDirection; } }

    private bool isMoving;
    public bool IsMoving { get { return isMoving; } }


    [Header("Animation Info")]

    [SerializeField] private Animator animator;

    [Header("SpeechBubble Info")]
    [SerializeField] private SpeechBubbleHandler speechBubbleHandler;

    private int currentCoin;
    public int CurrentCoin { get { return currentCoin; } set { currentCoin = value; } }


    // �ִϸ��̼� ó��
    private static readonly int MoveDir = Animator.StringToHash("MoveDir");
    private static readonly int IsMove = Animator.StringToHash("IsMove");

    private NPCInteractionHandler currentNPC;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (GameManager.Instance.CurrentState != GameState.Playing) return;

        GetUserInput();
        UpdateAnimation();
    }

    private void FixedUpdate()
    {
        if (GameManager.Instance.CurrentState != GameState.Playing) return;

        Move();
        Rotate();
    }

    #region �Է� ó��

    /// <summary>
    /// ����� �Է� ó��
    /// </summary>
    private void GetUserInput()
    {
        Vector3 dir = Vector3.ClampMagnitude(new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0f), 1f);
        moveDirection = dir.normalized;

        if (currentNPC != null && Input.GetKeyDown(KeyCode.F))
        {
            currentNPC.Interact();
        }
    }

    #endregion

    #region ���� ó��

    /// <summary>
    /// �÷��̾� �̵� ó��
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

    #region �޼��� ó��

    public void SendMessageOnSpeechBubble(string message)
    {
        if (speechBubbleHandler)
        {
            speechBubbleHandler.gameObject.SetActive(true);
            speechBubbleHandler.StartSpeech(message);
        }
        else
        {
            Debug.Log("���� X");
        }    
    }

    #endregion

    #region �浹 ó��

    private void OnTriggerEnter2D(Collider2D collision)
    {
        NPCInteractionHandler npc = collision.GetComponent<NPCInteractionHandler>();
        if (npc != null)
        {
            currentNPC = npc;
            npc.ShowInteractionUI(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        NPCInteractionHandler npc = collision.GetComponent<NPCInteractionHandler>();
        if (npc != null && npc == currentNPC)
        {
            npc.ShowInteractionUI(false);
            currentNPC = null;
        }
    }

    #endregion
}
