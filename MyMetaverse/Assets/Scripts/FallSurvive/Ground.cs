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
        // 랜덤 크기
        float size = Random.Range(minSize, maxSize);
        transform.localScale = new Vector3(size, 1f, 0);

        // y 위치는 마지막 Ground에서 paddding 값만큼
        float posY = lastPosition.y - padding;

        // 랜덤 배치 : 설정한 범위 내에서 랜덤 값
        float posX = Random.Range(minPosX, maxPosX);

        transform.position = new Vector3(posX, posY, 0);

        // Position 전달
        return transform.position;
    }
}
