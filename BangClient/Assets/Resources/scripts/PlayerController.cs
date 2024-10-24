using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using FreeNet;
using BangGameServer;

public class PlayerController : MonoBehaviour
{
    // �� ī�� �� ���� ī�� ���
    List<Card> handCardList = new List<Card>();
    List<Card> equipCardList = new List<Card>();

    // ĳ����
    Characters myChar;
    TextMeshProUGUI charExplaneBox;
    byte player_me_index;

    // ����
    public Transform player;

    // �տ� �� ī��
    Dictionary<string, Transform> handCard = new Dictionary<string, Transform>();

    // �ٸ� �÷��̾���� �������� ���. ������ ���� ���� �ʿ�
    public List<Transform> equips;

    // ä��
    [SerializeField] TextMeshProUGUI chat;
    [SerializeField] TextMeshProUGUI inputField;
    List<string> chatList = new List<string>();
    string chatText;

    // ��ſ�
    CNetworkManager network_manager;

    void Start()
    {
        foreach (Transform cards in player)
        {
            handCard[cards.name] = cards;
            cards.gameObject.SetActive(false);              // �÷��� �� ���а� ����� Ȱ��ȭ ��ų ��
        }

        this.network_manager = GameObject.Find("NetworkManager").GetComponent<CNetworkManager>();
    }

    // game room���� ó��?
    // �ڸ�Ʈ�� Ŭ���̾�Ʈ ������ ó��.
    public void SetMyNumber(byte index)
    {
        player_me_index = index;

        //Debug.Log($"���� �ѹ�: {index}");
    }

    public void SetMyChar(string charName, int life)
    {
        myChar.name = charName;
        myChar.life = life;
        //Debug.Log("ĳ���� ����");
    }

    // �� �߰�
    public void AddCard(string cardName, string shape, string number)
    {

    }

    // ä�� ������
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

        // ��ȭ ������ 10�������� �����
        if (chatList.Count >= 10)
            chatList.RemoveAt(0);

        chatText = "";

        foreach(string text in chatList)
        {
            chatText += text + "\n";
        }

        Debug.Log($"��ȭ : {msg}");
        Debug.Log($"��ȭ ���: {chatText}");
        chat.text = chatText;
    }
}
