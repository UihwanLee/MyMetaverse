using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Zoom Setting")]
    [Range(1f, 20f)][SerializeField] private float zoomSpeed;
    [SerializeField] private float minZoom = 4f;
    [SerializeField] private float maxZoom = 10f;

    [Header("Move Setting")]
    [Range(1f, 20f)][SerializeField] private float moveSpeed;

    [Header("Camera Limit Area")]
    [SerializeField] private Vector2 minLimit = new Vector2(-30f, -30f);
    [SerializeField] private Vector2 maxLimit = new Vector2(30f, 30f);

    private Camera cam;
    private Vector3 startDragPos;

    private void Start()
    {
        cam = GetComponent<Camera>();
    }

    private void Update()
    {
        if(GameManager.Instance.CurrentState == GameState.Playing)
        {
            HandleZoom();
            HandleMove();
        }
    }

    /// <summary>
    /// 마우스 휠 줌 처리
    /// </summary>
    private void HandleZoom()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        // 마우스 휠 움직임이 있다면
        if(Mathf.Abs(scroll) > 0.01f)
        {
            // 지정한 Zoom 영역 내에서 카메라 사이즈 변경
            cam.orthographicSize -= scroll * zoomSpeed;
            cam.orthographicSize = Mathf.Clamp(cam.orthographicSize, minZoom, maxZoom);
        }

    }

    /// <summary>
    /// 마우스 드래그 이동 처리
    /// </summary>
    private void HandleMove()
    {
        if(Input.GetMouseButtonDown(2))
        {
            // 드래그 시작 포인트 위치 지정
            startDragPos = Input.mousePosition;
        }

        if(Input.GetMouseButton(2))
        {
            // 드래그 중이라면 시작 포지션에서 이동한 좌표만큼 이동 
            Vector3 offset = cam.ScreenToViewportPoint(Input.mousePosition) - cam.ScreenToViewportPoint(startDragPos);
            transform.position -= offset * moveSpeed;

            // 영역 제한
            Vector3 pos = transform.position;

            float vertExtent = cam.orthographicSize;
            float horzExtent = vertExtent * cam.aspect;

            pos.x = Mathf.Clamp(pos.x, minLimit.x + horzExtent, maxLimit.x - horzExtent);
            pos.y = Mathf.Clamp(pos.y, minLimit.y + vertExtent, maxLimit.y - vertExtent);

            transform.position = pos;

            startDragPos = Input.mousePosition;
        }
    }
}
