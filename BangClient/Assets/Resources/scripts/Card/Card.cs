using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Card : MonoBehaviour
{
    public string cardName;
    public string funcText;

    // ī�� ����ϱ�
    public abstract void UseCard();

    // ī�� ȿ�� �����ֱ�
    public abstract void MouseIn_ViewCardFunc();
    public abstract void MouseOut_ViewCardFunc();
}
