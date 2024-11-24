using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Dinamite : Cards
{
    CBattleRoom battleRoom;
    //Transform viewUi;
    EventSystem eventSystem;

    public PlayerController controller;           // 플레이어 인덱스 번호를 가져오기 위함

    void Start()
    {
        battleRoom = GameObject.Find("BattleRoom").GetComponent<CBattleRoom>();
        //viewUi = transform.GetChild(0);
        //viewUi.transform.gameObject.SetActive(false);
        eventSystem = gameObject.GetComponent<EventSystem>();
        controller = GameObject.Find("PlayerController").GetComponent<PlayerController>();

        cardName = "다이너마이트";
        funcText = "사정 거리 내의 한 사람에게 공격을 가한다.";
    }

    public override void UseCard()
    {
        //battleRoom.UseCardEvent(cardName);


        //controller.RemoveCard(cardIndex, cardName, shape, number);
        Debug.Log($"사용한 카드 인덱스: {cardIndex}. {cardName}");
    }
    public override void SetCard(int index, string cardName, string shape, string number)
    {
        cardIndex = index;
        this.cardName = cardName;
        this.shape = shape;
        this.number = number;
    }

    public override void MouseIn_ViewCardFunc()
    {
    }

    public override void MouseOut_ViewCardFunc()
    {
    }
}
