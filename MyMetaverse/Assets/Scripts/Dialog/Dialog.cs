using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newDialog", menuName = "Dialog/newDialog")]
public class Dialog : ScriptableObject
{
    // 대화 시 사용하는 Dialog 데이터 : 대화에 대한 내용이 들어있음

    public string name;                     // NPC 이름
    public Sprite sprite;                   // NPC 이미지
    public List<string> dialogList;         // NPC 대화 리스트
    public List<string> optionList;         // NPC 옵션 리스트
    public List<int> optionIdxList;         // NPC 옵션 인덱스 리스트
    public List<string> backRowDialogList;     // NPC 후열 대화 리스트
}
