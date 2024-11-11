using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class Bang : Cards
{
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

        // 뱅을 쓰는 경우
        // 1. 공격(볼캐닉을 장착 중이거나, 플레이어 캐릭터가 윌리 더 키드인 경우를 고려하여 작성할 것)
        // 2. 인디언 대응
        // 3. 결투


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
