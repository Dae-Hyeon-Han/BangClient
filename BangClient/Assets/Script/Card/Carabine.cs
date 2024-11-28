using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using FreeNet;
using BangGameServer;

public class Carabine : Cards
{
    //NetworkManager networkManager;

    CBattleRoom battleRoom;
    //Transform viewUi;
    EventSystem eventSystem;

    //CPlayer myPlayer;
    PlayerController controller;

    void Start()
    {
        battleRoom = GameObject.Find("BattleRoom").GetComponent<CBattleRoom>();
        //viewUi = transform.GetChild(0);
        //viewUi.transform.gameObject.SetActive(false);
        eventSystem = gameObject.GetComponent<EventSystem>();
        //myPlayer = GameObject.Find("player0").GetComponent<CPlayer>();
        controller = GameObject.Find("PlayerController").GetComponent<PlayerController>();

        cardName = "카빈";
        funcText = "사정 거리 내의 한 사람에게 공격을 가한다.";
    }

    public override void UseCard()
    {
        // 총 이미지 보이기
        //controller.equipIcon[0].sprite = Resources.Load<Sprite>("Images/CardImage/CARABINE");
        //controller.equipIcon[0].gameObject.SetActive(true);

        // 서버에 총 장착 메시지 보내기
        CPacket msg = CPacket.create((short)PROTOCOL.USECARD);
        msg.push("CARABINE");
        msg.push(controller.player_me_index);
        controller.network_manager.send(msg);

        //controller.RemoveCard(cardIndex, cardName, shape, number);
        controller.EquipCard(cardIndex, cardName, shape, number);

        Debug.Log($"사용한 카드 인덱스: {cardIndex}. {cardName}");
        //DelEvent();
    }

    public override void DelEvent()
    {
        Destroy(this);
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
