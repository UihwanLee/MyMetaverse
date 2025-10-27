using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ground : MonoBehaviour
{
    [Header("Setting Value")]
    [SerializeField] private float minSize;
    [SerializeField] private float maxSize;
    [SerializeField] private float minPosX;
    [SerializeField] private float maxPosX;
    [SerializeField] private float padding;

    public Vector3 SetRandomPlace(Vector3 lastPosition)
    {
        // ���� ũ��
        float size = Random.Range(minSize, maxSize);
        transform.localScale = new Vector3(size, 1f, 0);

        // y ��ġ�� ������ Ground���� paddding ����ŭ
        float posY = lastPosition.y - padding;

        // ���� ��ġ : ������ ���� ������ ���� ��
        float posX = Random.Range(minPosX, maxPosX);

        transform.position = new Vector3(posX, posY, 0);

        // Position ����
        return transform.position;
    }
}
