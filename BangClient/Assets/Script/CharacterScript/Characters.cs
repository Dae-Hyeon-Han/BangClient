using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Characters : MonoBehaviour
{
    public string characterName;        // ĳ���� �̸�
    public int life;                    // �⺻ ���. �������� ������ ��(�߰� ����)
    public string coments;              // ĳ���Ϳ� ���� ����

    public abstract void CharacterAbility();
}