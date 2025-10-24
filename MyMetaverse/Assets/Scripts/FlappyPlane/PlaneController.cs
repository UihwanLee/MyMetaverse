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

    #region 입력 처리

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

    #region 동작 처리

    private void Movement()
    {
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


}
