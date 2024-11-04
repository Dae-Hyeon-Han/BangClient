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
    List<Card> equipCardList = new List<Card>();

    // 캐릭터
    Characters myChar;
    TextMeshProUGUI charExplaneBox;
    byte player_me_index;

    // 유저
    public Transform player;

    // 손에 든 카드
    #region 이 셋은 세트
    List<Transform> myCardPool = new List<Transform>();
    List<Image> myCardShape = new List<Image>();
    List<TextMeshProUGUI> myCardNumber = new List<TextMeshProUGUI>();
    #endregion
    List<Card> myCard = new List<Card>();
    List<bool> fullCard = new List<bool>();                     // 카드 추가 시 false 인 곳의 인덱스만 사용할 것
    Image card;

    // 다른 플레이어들이 장착중인 장비. 설명을 보기 위해 필요
    public List<Transform> equips;

    // 채팅
    [SerializeField] TextMeshProUGUI chat;
    [SerializeField] TextMeshProUGUI inputField;
    List<string> chatList = new List<string>();
    string chatText;

    // 통신용
    CNetworkManager network_manager;

    // 턴
    public byte current_player_index;

    // 게임 진행용
    public bool CanDraw;            // 드로우 할 수 있는지
    public bool CanBang;            // 뱅 쏠 수 있는지

    // 디버그
    [SerializeField] TextMeshProUGUI debug;

    void Start()
    {
        // pool setting
        foreach (Transform myCard in player.GetChild(4))
        {
            myCardPool.Add(myCard);
            myCardShape.Add(myCard.GetChild(0).GetComponent<Image>());
            myCardNumber.Add(myCard.GetChild(1).GetComponent<TextMeshProUGUI>());
            fullCard.Add(false);
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

    public void SetMyCard(CPacket msg)
    {
        #region
        //Debug.Log($"패킷: {msg.buffer.Length}");

        //for(int i=0; i<4; i++)
        //{
        //    PlusCard(msg.pop_string(), msg.pop_string(), msg.pop_string());
        //}

        //while (true)
        //{
        //    string name, shape, number;
        //    name = msg.pop_string();
        //    shape = msg.pop_string();
        //    number = msg.pop_string();

        //    int pivot = 0;

        //    try
        //    {


        //        pivot++;


        //    }
        //    catch
        //    {
        //        Debug.Log("예외 발생");
        //        return;
        //    }


        //    //if(string.IsNullOrEmpty(msg.pop_string()) == false)
        //    //{

        //    //}
        //    //else
        //    //{
        //    //    break;
        //    //}
        //}

        //Debug.Log("while 탈출");
        #endregion

        byte count = msg.pop_byte();
        byte index = msg.pop_byte();

        //Debug.Log($"인덱스: {index}, {player_me_index}");

        for (byte i = 0; i < count; i++)
        {
            if (index == player_me_index)
            {
                int cardCount = msg.pop_int32();

                for (int j = 0; j < cardCount; j++)
                {
                    //Debug.Log($"내 인덱스2: {player_me_index}, {msg.pop_string()}, {msg.pop_string()}, {msg.pop_string()}");
                    PlusCard(msg.pop_string(), msg.pop_string(), msg.pop_string());
                }
            }
            else
            {
                debug.text = $"인덱스: {index}, {player_me_index}";
            }

        }
    }

    // 패 추가
    public void PlusCard(string cardName, string shape, string number)
    {
        Debug.Log($"{cardName},{shape},{number}");

        #region
        //for(int i=0; i<myCardPool.Count; i++)
        //{
        //    if(myCardPool[i].gameObject.activeSelf == false)
        //    {
        //        myCardPool[i].gameObject.SetActive(true);
        //        myCardPool[i].GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/CardImage/" + cardName);
        //        myCardShape[i].sprite = Resources.Load<Sprite>("Images/CardImage/" + shape);
        //        myCardNumber[i].text = number;

        //        // 글자색 셋팅
        //        if (shape == "DIAMOND" || shape == "HEART")
        //            myCardNumber[i].color = Color.red;
        //        else
        //            myCardNumber[i].color = Color.black;
        //    }
        //}
        #endregion

        // fullCard는 리스트의 사용 중이지 않은 인덱스 번호를 찾기 위한 수단
        for (int i = 0; i < fullCard.Count; i++)
        {
            if (fullCard[i])
                continue;
            else
            {
                myCardPool[i].gameObject.SetActive(true);
                myCardPool[i].GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/CardImage/" + cardName);
                myCardShape[i].sprite = Resources.Load<Sprite>("Images/CardImage/" + shape);
                myCardNumber[i].text = number;
                fullCard[i] = true;
                return;
            }
        }
    }

    public void TurnEnd()
    {
        CPacket msg = CPacket.create((short)PROTOCOL.TURN_FINISHED_REQ);
        msg.push(player_me_index);
        this.network_manager.send(msg);
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

        foreach (string text in chatList)
        {
            chatText += text + "\n";
        }

        Debug.Log($"대화 : {msg}");
        Debug.Log($"대화 목록: {chatText}");
        chat.text = chatText;
    }

    public void DrawCardEvent()
    {
        // 내 턴에만 드로우 가능
        if (CanDraw)
        {
            Debug.Log("드로우!");
            CPacket msg = CPacket.create((short)PROTOCOL.DRAWCARD);
            msg.push(player_me_index);
            this.network_manager.send(msg);
        }
        else
        {
            Debug.Log($"{current_player_index}의 턴임");
        }
    }

    public void UsedDeckClickEvent()
    {
        // 사용한 카드 덱
        Debug.Log("");
    }
}
