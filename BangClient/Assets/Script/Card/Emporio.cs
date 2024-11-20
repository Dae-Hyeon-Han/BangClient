using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using FreeNet;
using BangGameServer;

public class Emporio : Cards
{
    //NetworkManager networkManager;

    CBattleRoom battleRoom;
    //Transform viewUi;
    EventSystem eventSystem;

    PlayerController controller;           // 플레이어 인덱스 번호를 가져오기 위함

    void Start()
    {
        battleRoom = GameObject.Find("BattleRoom").GetComponent<CBattleRoom>();
        //viewUi = transform.GetChild(0);
        //viewUi.transform.gameObject.SetActive(false);
        eventSystem = gameObject.GetComponent<EventSystem>();
        controller = GameObject.Find("PlayerController").GetComponent<PlayerController>();

        cardName = "잡화점!";
        funcText = "사정 거리 내의 한 사람에게 공격을 가한다.";
    }

    public override void UseCard()
    {
        //battleRoom.UseCardEvent(cardName);
        //Debug.Log("카드가 쏨");

        //[프로토콜][EMPORIO]
        CPacket msg = CPacket.create((short)PROTOCOL.USECARD);
        msg.push("EMPORIO");
        controller.network_manager.send(msg);
    }

    public override void MouseIn_ViewCardFunc() { }

    public override void MouseOut_ViewCardFunc() { }
}
