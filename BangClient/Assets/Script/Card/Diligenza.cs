using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using FreeNet;
using BangGameServer;

public class Diligenza : Cards
{
    //NetworkManager networkManager;

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

        cardName = "역마차!";
        funcText = "사정 거리 내의 한 사람에게 공격을 가한다.";
    }
    
    public override void UseCard()
    {
        //battleRoom.UseCardEvent(cardName);
        //Debug.Log("카드가 쏨");

        //[프로토콜][DILIGENZA][내 index]
        CPacket msg = CPacket.create((short)PROTOCOL.USECARD);
        msg.push("DILIGENZA");
        msg.push(controller.player_me_index);                       // 그냥 서버에서 현재 턴인 플레이어 체력 올려도 되지 않나?
        controller.network_manager.send(msg);

        controller.RemoveCard(cardIndex, cardName, shape, number);

        Debug.Log($"사용한 카드 인덱스: {cardIndex}. {cardName}");
    }
    public override void SetCard(int index, string cardName, string shape, string number)
    {
        cardIndex = index;
        this.cardName = cardName;
        this.shape = shape;
        this.number = number;
    }

    public override void MouseIn_ViewCardFunc() { }

    public override void MouseOut_ViewCardFunc() { }
}
