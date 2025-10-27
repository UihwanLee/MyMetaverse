using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newDialog", menuName = "Dialog/newDialog")]
public class Dialog : ScriptableObject
{
    // ��ȭ �� ����ϴ� Dialog ������ : ��ȭ�� ���� ������ �������

    public string name;                     // NPC �̸�
    public Sprite sprite;                   // NPC �̹���
    public List<string> dialogList;         // NPC ��ȭ ����Ʈ
    public List<string> optionList;         // NPC �ɼ� ����Ʈ
    public List<int> optionIdxList;         // NPC �ɼ� �ε��� ����Ʈ
    public List<string> backRowDialogList;     // NPC �Ŀ� ��ȭ ����Ʈ
}
