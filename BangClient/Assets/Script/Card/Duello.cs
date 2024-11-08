using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Duello : Cards
{
    CBattleRoom battleRoom;
    //Transform viewUi;
    EventSystem eventSystem;

    void Start()
    {
        battleRoom = GameObject.Find("BattleRoom").GetComponent<CBattleRoom>();
        //viewUi = transform.GetChild(0);
        //viewUi.transform.gameObject.SetActive(false);
        eventSystem = gameObject.GetComponent<EventSystem>();

        cardName = "결투";
        funcText = "사정 거리 내의 한 사람에게 공격을 가한다.";
    }

    public override void UseCard()
    {
        battleRoom.UseCardEvent(cardName);
        Debug.Log("카드가 쏨");
    }

    public override void MouseIn_ViewCardFunc()
    {
    }

    public override void MouseOut_ViewCardFunc()
    {
    }
}
