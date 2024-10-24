using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using FreeNet;
using BangGameServer;

public class PlayerController : MonoBehaviour
{
    // 손 카드 및 장착 카드 목록
    List<Card> handCardList = new List<Card>();
    List<Card> equipCardList = new List<Card>();

    // 캐릭터
    Characters myChar;
    TextMeshProUGUI charExplaneBox;
    byte player_me_index;

    // 유저
    public Transform player;

    // 손에 든 카드
    Dictionary<string, Transform> handCard = new Dictionary<string, Transform>();

    // 다른 플레이어들이 장착중인 장비. 설명을 보기 위해 필요
    public List<Transform> equips;

    // 채팅
    [SerializeField] TextMeshProUGUI chat;
    [SerializeField] TextMeshProUGUI inputField;
    List<string> chatList = new List<string>();
    string chatText;

    // 통신용
    CNetworkManager network_manager;

    void Start()
    {
        foreach (Transform cards in player)
        {
            handCard[cards.name] = cards;
            cards.gameObject.SetActive(false);              // 플레이 중 손패가 생기면 활성화 시킬 것
        }

        this.network_manager = GameObject.Find("NetworkManager").GetComponent<CNetworkManager>();
    }

    // game room에서 처리?
    // 코멘트는 클라이언트 측에서 처리.
    public void SetMyNumber(byte index)
    {
        player_me_index = index;

        //Debug.Log($"마이 넘버: {index}");
    }

    public void SetMyChar(string charName, int life)
    {
        myChar.name = charName;
        myChar.life = life;
        //Debug.Log("캐릭터 셋팅");
    }

    // 패 추가
    public void AddCard(string cardName, string shape, string number)
    {

    }

    // 채팅 보내기
    public void ChatSend()
    {
        CPacket msg = CPacket.create((short)PROTOCOL.CHAT);
        msg.push(player_me_index + ": " + inputField.text);
        inputField.text = "";

        this.network_manager.send(msg);
    }

    public void ChatReceive(string msg)
    {
        chatList.Add(msg);

        // 대화 개수는 10개까지만 남기기
        if (chatList.Count >= 10)
            chatList.RemoveAt(0);

        chatText = "";

        foreach(string text in chatList)
        {
            chatText += text + "\n";
        }

        Debug.Log($"대화 : {msg}");
        Debug.Log($"대화 목록: {chatText}");
        chat.text = chatText;
    }
}
