using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using FreeNet;
using BangGameServer;

public class Schofield : Cards
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

        cardName = "빗나감!";
        funcText = "사정 거리 내의 한 사람에게 공격을 가한다.";
    }

    public override void UseCard()
    {
        //battleRoom.UseCardEvent(cardName);
        Debug.Log("카드가 쏨");

        //[프로토콜][카드 이름][나의 인덱스]
        CPacket msg = CPacket.create((short)PROTOCOL.USECARD);
        msg.push("REMINGTON");
        msg.push(controller.player_me_index);
        controller.network_manager.send(msg);

        controller.RemoveCard(cardIndex, cardName, shape, number);
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
