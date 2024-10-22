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
    public void SetMyChar(byte index, string charName, int life)
    {
        myChar.name = charName;
        myChar.life = life;
        player_me_index = index;
    }

    // �� �߰�
    public void AddCard(string cardName, string shape, string number)
    {

    }

    // ä�� ������
    public void ChatSend()
    {
        CPacket msg = CPacket.create((short)PROTOCOL.CHAT);
        msg.push(player_me_index);
        msg.push(inputField.text);
        inputField.text = null;

        //Debug.Log($"{inputField.text}");
    }

    public void ChatReceive(string msg)
    {
        //dialogue.text += "\n" + msg;            // �� �� �ٲ� �� �ؽ�Ʈ �Է�
        //dialogueQueue.Enqueue($"\n{msg}");
        chatList.Add(msg);

        // ��ȭ ������ 10�������� �����
        if (chatList.Count > 10)
            chatList.RemoveAt(0);

        // ���÷���
        for(int i=0; i < chatList.Count; i++)
        {
            chat.text = "\n" + chatList[i];
        }
    }
}
