using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using FreeNet;
using BangGameServer;

public class Mustang : Cards
{
    NetworkManager networkManager;

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

        cardName = "조랑말";
        funcText = "사정 거리 내의 한 사람에게 공격을 가한다.";
    }

    public override void UseCard()
    {
        //battleRoom.UseCardEvent(cardName);
        Debug.Log("카드가 쏨");

        // 조랑말 이미지 보이기
        controller.equipIcon[2].gameObject.SetActive(true);
    }

    public override void MouseIn_ViewCardFunc(){}

    public override void MouseOut_ViewCardFunc(){}
}
