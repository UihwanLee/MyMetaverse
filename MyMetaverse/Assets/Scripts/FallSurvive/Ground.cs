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
        // ���� ũ��
        float groundSize = Random.Range(minSize, maxSize);

        transform.GetChild(0).localScale = new Vector3(groundSize, transform.GetChild(0).localScale.y, 0);

        // y ��ġ�� ������ Ground���� paddding ����ŭ
        float posY = lastPosition.y - padding;

        // ���� ��ġ : ������ ���� ������ ���� ��
        float posX = Random.Range(minPosX, maxPosX);

        transform.position = new Vector3(posX, posY, 0);

        // �������� Obstacle ������Ʈ ����
        SetObstacleOnGround(groundSize);

        // Position ����
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
        // Ư�� ���� ������ clamp
        float clampedValue = Mathf.Clamp(inputValue, INPUT_MIN, INPUT_MAX);

        // ���� ������ ���� t ���� : ���� ����
        float t = (clampedValue - INPUT_MIN) / (INPUT_MAX - INPUT_MIN);

        // Lerp�� �̿��Ͽ� ���� ������ ���� �� �� ���ϱ�
        float outputValue = Mathf.Lerp(OUTPUT_MIN, OUTPUT_MAX, t);

        return outputValue;
    }
}
