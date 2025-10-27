using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class Ground : MonoBehaviour
{
    [Header("Setting Value")]
    [SerializeField] private float minSize;
    [SerializeField] private float maxSize;
    [SerializeField] private float minPosX;
    [SerializeField] private float maxPosX;
    [SerializeField] private float minObstaclePosX;
    [SerializeField] private float maxObstaclePosX;
    [SerializeField] private float padding;

    public Vector3 SetRandomPlace(Vector3 lastPosition)
    {
        // 랜덤 크기
        float groundSize = Random.Range(minSize, maxSize);

        transform.GetChild(0).localScale = new Vector3(groundSize, transform.GetChild(0).localScale.y, 0);

        // y 위치는 마지막 Ground에서 paddding 값만큼
        float posY = lastPosition.y - padding;

        // 랜덤 배치 : 설정한 범위 내에서 랜덤 값
        float posX = Random.Range(minPosX, maxPosX);

        transform.position = new Vector3(posX, posY, 0);

        // 랜덤으로 Obstacle 오브젝트 생성
        SetObstacleOnGround(groundSize);

        // Position 전달
        return transform.position;
    }

    private void SetObstacleOnGround(float groundSize)
    {
        float obstaclePosRange = ConvertSizeToPositionOffset(groundSize, minSize, maxSize, minObstaclePosX, maxObstaclePosX);
        bool isObstacle = (Random.value < 0.4f) ? true : false;
        transform.GetChild(1).transform.gameObject.SetActive(isObstacle);

        if (isObstacle)
        {
            Vector3 posObstacle = transform.GetChild(1).transform.localPosition;
            posObstacle.x = Random.Range(-obstaclePosRange, obstaclePosRange);
            transform.GetChild(1).transform.localPosition = posObstacle;
        }
    }

    private float ConvertSizeToPositionOffset(float inputValue, float INPUT_MIN, float INPUT_MAX, float OUTPUT_MIN, float OUTPUT_MAX)
    {
        // 특정 범위 값으로 clamp
        float clampedValue = Mathf.Clamp(inputValue, INPUT_MIN, INPUT_MAX);

        // 선형 보간을 위한 t 설정 : 현재 비율
        float t = (clampedValue - INPUT_MIN) / (INPUT_MAX - INPUT_MIN);

        // Lerp를 이용하여 새로 설정한 범위 내 값 구하기
        float outputValue = Mathf.Lerp(OUTPUT_MIN, OUTPUT_MAX, t);

        return outputValue;
    }
}
