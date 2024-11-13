using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using BangGameServer;
using FreeNet;

public class Bang : Cards
{
    NetworkManager networkManager;

    CBattleRoom battleRoom;
    //Transform viewUi;
    EventSystem eventSystem;

    //public byte player_me_index;
    //public byte currnt_player_index;            // 이 두 값이 일치하면 my turn.
    public PlayerController controller;           // 뱅을 쓸 때, 어떤 상태에서 쓰이는지 확인하기 위함
    public byte targetIndex;

    void Start()
    {
        battleRoom = GameObject.Find("BattleRoom").GetComponent<CBattleRoom>();
        //viewUi = transform.GetChild(0);
        //viewUi.transform.gameObject.SetActive(false);
        eventSystem = gameObject.GetComponent<EventSystem>();
        controller = GameObject.Find("PlayerController").GetComponent<PlayerController>();

        cardName = "뱅!";
        funcText = "사정 거리 내의 한 사람에게 공격을 가한다.";
    }

    public override void UseCard()
    {
        //battleRoom.UseCardEvent(cardName);
        Debug.Log("카드가 쏨");

        if (controller.CanBang == false)
            Debug.Log("또 쏠 수 없음");
        else
        {
            CPacket msg = CPacket.create((short)PROTOCOL.USECARD);
            msg.push(cardName);
            msg.push(targetIndex);
            networkManager.send(msg);
        }
    }

    public override void MouseIn_ViewCardFunc()
    {
        //viewUi.gameObject.SetActive(true);
        //viewUi.GetComponent<TextMeshProUGUI>().text = funcText;

        //battleRoom.UseViewUi(viewUi, true, funcText);
    }

    public override void MouseOut_ViewCardFunc()
    {
        //viewUi.gameObject.SetActive(true);
        //viewUi.GetComponent<TextMeshProUGUI>().text = funcText;
    }
}
