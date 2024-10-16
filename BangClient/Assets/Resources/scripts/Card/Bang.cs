using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class Bang : Card
{
    CBattleRoom battleRoom;
    Transform viewUi;
    EventSystem eventSystem;

    void Start()
    {
        battleRoom = GameObject.Find("BattleRoom").GetComponent<CBattleRoom>();
        viewUi = transform.GetChild(0);
        viewUi.transform.gameObject.SetActive(false);
        eventSystem = gameObject.GetComponent<EventSystem>();

        cardName = "��!";
        funcText = "���� �Ÿ� ���� �� ������� ������ ���Ѵ�.";
    }

    public override void UseCard()
    {
        battleRoom.UseCardEvent(cardName);
        Debug.Log("ī�尡 ��");
    }

    public override void MouseIn_ViewCardFunc()
    {
        //viewUi.gameObject.SetActive(true);
        //viewUi.GetComponent<TextMeshProUGUI>().text = funcText;

        battleRoom.UseViewUi(viewUi, true, funcText);
    }

    public override void MouseOut_ViewCardFunc()
    {
        //viewUi.gameObject.SetActive(true);
        //viewUi.GetComponent<TextMeshProUGUI>().text = funcText;
    }
}
