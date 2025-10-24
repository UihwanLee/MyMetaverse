using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneController : MonoBehaviour
{
    private Rigidbody2D _rigidbody;

    [Range(1f, 20f)][SerializeField] private float moveSpeed;
    [Range(1f, 20f)][SerializeField] private float jumpForce;

    private bool isJumping;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();

        isJumping = false;
    }

    private void Update()
    {
        GetUserInput();
    }

    #region �Է� ó��

    private void GetUserInput()
    {
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


}
