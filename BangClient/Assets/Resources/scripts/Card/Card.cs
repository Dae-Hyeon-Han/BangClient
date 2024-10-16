using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Card : MonoBehaviour
{
    public string cardName;
    public string funcText;

    // 카드 사용하기
    public abstract void UseCard();

    // 카드 효과 보여주기
    public abstract void MouseIn_ViewCardFunc();
    public abstract void MouseOut_ViewCardFunc();
}
