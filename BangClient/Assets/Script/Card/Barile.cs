using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using FreeNet;
using BangGameServer;

public class Barile : Cards
{
    //NetworkManager networkManager;

    CBattleRoom battleRoom;
    //Transform viewUi;
    EventSystem eventSystem;

    PlayerController controller;

    void Start()
    {
        battleRoom = GameObject.Find("BattleRoom").GetComponent<CBattleRoom>();
        //viewUi = transform.GetChild(0);
        //viewUi.transform.gameObject.SetActive(false);
        eventSystem = gameObject.GetComponent<EventSystem>();
        controller = GameObject.Find("PlayerController").GetComponent<PlayerController>();

        cardName = "술통";
        funcText = "사정 거리 내의 한 사람에게 공격을 가한다.";
    }

    public override void UseCard()
    {
        //battleRoom.UseCardEvent(cardName);
        //Debug.Log("카드가 쏨");

        // 술통 이미지 보이기
        //controller.equipIcon[1].gameObject.SetActive(true);

        // 술통 효과 활성화

        //[프로토콜][카드 이름][나의 인덱스]
        CPacket msg = CPacket.create((short)PROTOCOL.USECARD);
        msg.push("BARILE");
        msg.push(controller.player_me_index);
        //networkManager.send(msg);
        controller.network_manager.send(msg);

        //controller.RemoveCard(cardIndex, cardName, shape, number);
        controller.EquipCard(cardIndex, cardName, shape, number);

        Debug.Log($"사용한 카드 인덱스: {cardIndex}. {cardName}");
    }
    public override void SetCard(int index, string cardName, string shape, string number)
    {
        cardIndex = index;
        this.cardName = cardName;
        this.shape = shape;
        this.number = number;
    }

    public override void MouseIn_ViewCardFunc(){}

    public override void MouseOut_ViewCardFunc(){}
}
