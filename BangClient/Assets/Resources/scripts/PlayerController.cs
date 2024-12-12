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
    //List<Card> equipCardList = new List<Card>();
    List<Card> myCard = new List<Card>();

    // 0번 = 총
    // 1번 = 술통
    // 2번 = 조랑말
    // 3번 = 조준경
    [Header("User Equipment")]
    public List<Image> equipIcon = new List<Image>();

    // 캐릭터
    Characters myChar;
    TextMeshProUGUI charExplaneBox;

    // 유저
    [Header("User Information")]
    public byte player_me_index;
    public Transform player;
    //public CPlayer mePlayer;
    public byte current_player_index;
    public byte Target;

    // 다른 플레이어들이 장착중인 장비. 설명을 보기 위해 필요
    //public List<Transform> equips;

    // 채팅
    [Header("Chat func")]
    [SerializeField] TextMeshProUGUI chat;
    [SerializeField] TextMeshProUGUI inputField;
    List<string> chatList = new List<string>();
    string chatText;

    // 통신용
    [Header("Network")]
    public CNetworkManager network_manager;

    // 게임 진행용
    [Header("Game Help")]
    public bool CanDraw;            // 드로우 할 수 있는지
    private bool canBang;            // 뱅을 쏠 수 있는지. 턴 시작시 true가 되고, 뱅 쏜 후에 false
    //public bool CanBang;            // 뱅 쏠 수 있는지
    Cards cards;

    [SerializeField] Image explaneBox;
    [SerializeField] TextMeshProUGUI explaneText;
    //Card explaneSample;
    Dictionary<string, string> explaneWord = new Dictionary<string, string>();
    public TextMeshProUGUI turnCheck;
    public TextMeshProUGUI targetCheck;

    // 타깃 설정을 위해 on/off 되어야 하는 부분(클릭 가능 여부 => 뱅을 쏠 때, 플레이어 타깃 버튼의 콜라이더가 너무 커서 필요함)
    // 강탈 및 캣 벌로우 사용을 위해 필요
    [Header("Target Check")]
    public string targetCard;
    public string targetEquip;

    // 리액션 용
    [Header("Reaction")]
    [SerializeField] Transform ReactMancato;
    [SerializeField] Transform ReactBang;
    [SerializeField] Transform ReactDuello;
    Card tempCard;
    int checkCount;

    // 뱅을 쏠 수 있는지 확인 여부용. 턴 시작 시 true로 교체
    public bool CanBang
    {
        get { return canBang; }
        set
        {
            canBang = value;

            // 플레이어 캐릭터가 윌리 더 키드인 경우, 볼캐닉을 장착 중인 경우 canBang을 다시 true로 바꿀 것
        }
    }

    // 타깃 설정을 위한 열거형
    public enum PlayerState
    {
        BANG = 0,             // 뱅
        DUELLO = 1,           // 결투
        PANICO = 2,           // 강탈
        CAT_BALOU = 3,        // 캣 벌로우
        PRIGIONE = 4,         // 감옥
        DINAMITE = 5,         // 다이너마이트
        NONE = 100,           // 그 외
    }

    // 게임 상태 확인용
    public enum EventState
    {
        // 뱅
        DUELLO = 0,         // 결투
        INDIANI = 1,        // 인디언

        // 빗나감
        GATLING = 3,        // 기관총
        BANG = 4,           // 내가 피격 당한 경우
        //BARILE = 5,         // 술통. 빗나감이 있어도, 술통이 있거나 주르도네라면 먼제 적용해야 함

        // 잡화점
        EMPORIO = 6,

        // 내 턴 시작 전 선행 작업
        PRIGIONE = 8,       // 감옥
        DINAMITE = 9,       // 다이너마이트
    }

    public EventState myEventState;
    public PlayerState myState;
    public GameObject setTargetUi;

    // 디버그
    //[SerializeField] TextMeshProUGUI debug;

    void Start()
    {
        // pool setting
        //foreach (Transform myCard in player.GetChild(4))
        //{
        //myCardPool.Add(myCard);
        //myCardShape.Add(myCard.GetChild(0).GetComponent<Image>());
        //myCardNumber.Add(myCard.GetChild(1).GetComponent<TextMeshProUGUI>());
        //useCard.Add(false);
        //}

        this.network_manager = GameObject.Find("NetworkManager").GetComponent<CNetworkManager>();

        // 0 ~14
        foreach (Transform myCards in player.GetChild(4))
        {
            myCard.Add(myCards.GetComponent<Card>());
        }

        //CanBang = true;

        Target = 100;
    }

    // game room에서 처리?
    // 코멘트는 클라이언트 측에서 처리.
    public void SetMyNumber(byte index)
    {
        player_me_index = index;

        //Debug.Log($"마이 넘버: {index}");
    }

    //public void SetMyChar(string charName, int life)
    //{
    //    myChar.name = charName;
    //    myChar.life = life;
    //    //Debug.Log("캐릭터 셋팅");
    //}

    public void SetMyCard(CPacket msg)
    {
        byte playerCount = msg.pop_byte();
        byte index = msg.pop_byte();

        //Debug.Log($"인덱스: {index}, {player_me_index}");

        for (byte i = 0; i < playerCount; i++)
        {
            if (index == player_me_index)
            {
                int cardCount = msg.pop_int32();

                for (int j = 0; j < cardCount; j++)
                {
                    //Debug.Log($"내 인덱스2: {player_me_index}, {msg.pop_string()}, {msg.pop_string()}, {msg.pop_string()}");
                    string cardName = msg.pop_string();
                    string shape = msg.pop_string();
                    string number = msg.pop_string();
                    PlusCard(cardName, shape, number);

                    //PlusCard(msg.pop_string(), msg.pop_string(), msg.pop_string());

                    //useCard[i] = true;
                }
            }
        }
    }

    // 패 추가
    public void PlusCard(string cardName, string shape, string number)
    {
        #region
        //// fullCard는 리스트의 사용 중이지 않은 인덱스 번호를 찾기 위한 수단
        //// usedCard의 반복문 시작값을 리스트 마지막 값으로 셋팅하는 방법?
        //for (int i = 0; i < useCard.Count; i++)
        //{
        //    if (useCard[i] == false)
        //    {
        //        #region
        //        //findCardName.Add(cardName);                     // 빗나감 등 카드 찾기 기능에 사용할 용도
        //        //findCardShape.Add(shape);
        //        //findCardNumber.Add(number);
        //        #endregion

        //        #region
        //        //Cards card = new Cards;

        //        #endregion

        //        myCardPool[i].gameObject.SetActive(true);
        //        myCardPool[i].GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/CardImage/" + cardName);
        //        myCardShape[i].sprite = Resources.Load<Sprite>("Images/CardImage/" + shape);
        //        myCardNumber[i].text = number;
        //        useCard[i] = true;

        //        // 여기에 카드 이벤트 추가
        //        AddEventOnCard(i, cardName, shape, number);

        //        // 각 카드에 인덱스 부여
        //        //SetCard

        //        return;
        //    }
        //}
        #endregion

        for (int i = 0; i < myCard.Count; i++)
        {
            // 사용할 수 없는 카드라면. 카드 추가 후 return.
            // 사용할 수 있는 카드면 다음 인덱스로.
            if (myCard[i].gameObject.activeSelf == false)
            {
                myCard[i].SetCard(i, cardName, shape, number);
                return;
            }
        }
    }

    // 카드 사용시 호출
    public void RemoveCard(int index, string cardName, string shape, string number)
    {
        #region
        //Debug.Log($"버리는 카드: {index},{cardName},{shape},{number}");

        //// 카드 안 보이게 하고, 카드 사용 가능 여부 false로 변경
        //myCardPool[index].gameObject.SetActive(false);
        //useCard[index] = false;

        //// 사용된 카드 더미에 추가하는건 서버에서 처리
        //CPacket msg = CPacket.create((short)PROTOCOL.DROPCARD);
        //msg.push(player_me_index);
        //msg.push(findCardName[index]);
        //msg.push(findCardShape[index]);
        //msg.push(findCardNumber[index]);
        //network_manager.send(msg);

        //findCardName.RemoveAt(index);
        //findCardShape.RemoveAt(index);
        //findCardNumber.RemoveAt(index);

        //Destroy(myCardPool[index].GetComponent<Cards>());
        #endregion
    }

    public void EquipCard(int index, string cardName, string shape, string number)
    {
        #region
        //// 카드 안 보이게 하고, 카드 사용 가능 여부 false로 변경
        //myCardPool[index].gameObject.SetActive(false);
        //useCard[index] = false;

        //#region 추가분. 서버에 카드 수 줄이는 메시지 요청 필요
        ////findCardName.RemoveAt(index);
        ////findCardShape.RemoveAt(index);
        ////findCardNumber.RemoveAt(index);

        ////CPacket msg = CPacket.create((short)PROTOCOL.DROPCARD)
        ////network_manager.send();
        //#endregion

        //Destroy(myCardPool[index].GetComponent<Cards>());
        #endregion
    }

    public void AddEventOnCard(int i, string cardName, string shape, string number)
    {
        ////Debug.Log($"리스너 {cardName},{i}");
        //// 뱅
        //if (cardName == "BANG") 
        //{
        //    cards = myCardPool[i].gameObject.AddComponent<Bang>();
        //    //cards.SetCard(i, cardName, shape, number);
        //}
        //// 빗나감
        //else if (cardName == "MANCATO") { cards = myCardPool[i].gameObject.AddComponent<Mancato>(); }
        //// 맥주
        //else if (cardName == "BIRRA") { cards = myCardPool[i].gameObject.AddComponent<Birra>(); }
        //// 기관총
        //else if (cardName == "GATLING") { cards = myCardPool[i].gameObject.AddComponent<Gatling>(); }
        //// 결투
        //else if (cardName == "DUELLO") { cards = myCardPool[i].gameObject.AddComponent<Duello>(); }
        //// 인디언
        //else if (cardName == "INDIANI") { cards = myCardPool[i].gameObject.AddComponent<Indiani>(); }
        //// 주점
        //else if (cardName == "SALOON") { cards = myCardPool[i].gameObject.AddComponent<Saloon>(); }
        //// 강탈
        //else if (cardName == "PANICO") { cards = myCardPool[i].gameObject.AddComponent<Panico>(); }
        //// 캣 벌로우
        //else if (cardName == "CAT BALOU") { cards = myCardPool[i].gameObject.AddComponent<CatBalou>(); }
        //// 잡화점
        //else if (cardName == "EMPORIO") { cards = myCardPool[i].gameObject.AddComponent<Emporio>(); }
        //// 역마차
        //else if (cardName == "DILIGENZA") { cards = myCardPool[i].gameObject.AddComponent<Diligenza>(); }
        //// 웰스파고 은행
        //else if (cardName == "WELLS FARGO") { cards = myCardPool[i].gameObject.AddComponent<WellsFargo>(); }
        //// 스코필드
        //else if (cardName == "SCHOFIELD") { cards = myCardPool[i].gameObject.AddComponent<Schofield>(); }
        //// 레밍턴
        //else if (cardName == "REMINGTON") { cards = myCardPool[i].gameObject.AddComponent<Remington>(); }
        //// 카빈
        //else if (cardName == "CARABINE") { cards = myCardPool[i].gameObject.AddComponent<Carabine>(); }
        //// 윈체스터
        //else if (cardName == "WINCHESTER") { cards = myCardPool[i].gameObject.AddComponent<Winchester>(); }
        //// 볼캐닉
        //else if (cardName == "VOLCANIC") { cards = myCardPool[i].gameObject.AddComponent<Volcanic>(); }
        //// 조준경
        //else if (cardName == "MIRONO") { cards = myCardPool[i].gameObject.AddComponent<Mirono>(); }
        //// 야생마
        //else if (cardName == "MUSTANG") { cards = myCardPool[i].gameObject.AddComponent<Mustang>(); }
        //// 술통
        //else if (cardName == "BARILE") { cards = myCardPool[i].gameObject.AddComponent<Barile>(); }
        //// 감옥
        //else if (cardName == "PRIGIONE") { cards = myCardPool[i].gameObject.AddComponent<Prigione>(); }
        //// 다이너마이트
        //else if (cardName == "DINAMITE") { cards = myCardPool[i].gameObject.AddComponent<Dinamite>(); }

        ////Debug.Log($"오브젝트 이름2: {myCardPool[i].name}");
        ////myCardPool[i].gameObject.AddComponent<Button>();
        //cards.SetCard(i, cardName, shape, number);
        //myCardPool[i].gameObject.GetComponent<Button>().onClick.AddListener(cards.UseCard);
    }

    public void TurnEnd()
    {
        if (current_player_index != player_me_index)
        {
            //Debug.Log($"현재 플레이어: {current_player_index}");
            return;
        }

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

        //Debug.Log($"대화 : {msg}");
        //Debug.Log($"대화 목록: {chatText}");
        chat.text = chatText;
    }

    public void DrawCardEvent()
    {
        // 내 턴에만 드로우 가능
        if (CanDraw)
        {
            //Debug.Log("드로우!");
            CPacket msg = CPacket.create((short)PROTOCOL.DRAWCARD);
            msg.push(player_me_index);
            msg.push(2);
            this.network_manager.send(msg);
            CanDraw = false;
        }
        else
        {
            Debug.Log($"{current_player_index}의 턴임");
        }
    }

    public void UsedDeckClickEvent()
    {
        // 사용한 카드 덱
        //Debug.Log("");
    }

    public void UseCardEvent()
    {
        // 뱅 등 타깃 지정이 필요한 카드는 이 메서드로 연결할 것
    }

    public void CardInfoDisplay()
    {
        //Debug.Log("카드 설명 보이기");
        //explaneBox.gameObject.SetActive(true);
        //explaneText.text = explaneWord[useCard[]].;


    }

    public void CardInfoCover()
    {
        //Debug.Log("카드 설명 감추기");
        //explaneBox.gameObject.SetActive(false);
        //explaneText.text = "";
    }

    // 실제로 쏠 수 있는지 확인하기 위한 거리 체크용 메서드
    // 뱅 사용시 호출
    public bool CanAttack()
    {


        return true;
    }

    public void Request(CPacket msg)
    {
        byte target = msg.pop_byte();
        //Debug.Log($"타깃1: {target}, {player_me_index}");

        //if (target != player_me_index)
        //{
        //    Debug.Log($"타깃2: {target}");
        //    return;
        //}
        string requestCard = msg.pop_string();
        //Debug.Log($"뱅 메시지: {requestCard}");

        if (requestCard == "MANCATO")
        {
            //Debug.Log("빗나감을 사용하시겠습니까?");
            RequestMancato();
        }
        else if (requestCard == "BANG")
        {
            //Debug.Log("뱅을 사용하시겠습니까?");
            RequestBang();
        }
        else if (requestCard == "Duello")
        {
            RequestDuello();
        }
    }

    public void RequestMancato()
    {
        #region
        ////Debug.Log("빗나감 페이지 요청");

        //for (int i = 0; i < findCardName.Count; i++)
        //{
        //    if (findCardName[i] == "MANCATO")
        //    {
        //        deleteIndex = i;
        //        deleteCardName = findCardName[i];
        //        deleteCardShape = findCardShape[i];
        //        deleteCardNumber = findCardNumber[i];
        //        ReactMancato.gameObject.SetActive(true);
        //        EventMancato(i);
        //        return;
        //    }
        //    else
        //    {
        //        Deny();
        //    }
        //}
        #endregion

        // 내 손에 빗나감이 있는지 확인
        for (int i = 0; i < myCard.Count; i++)
        {
            if (myCard[i].cardName == "MANCATO")
            {
                // 페이지 요청
                ReactMancato.gameObject.SetActive(true);
                tempCard = myCard[i];
            }
            else
            {
                //Deny();
                checkCount++;
            }

            if (checkCount == myCard.Count)
                Deny();
        }
        checkCount = 0;
        // 빗나감 쓸지 페이지 요청
        // 안쓰면 체력--
        // 쓰면 UseCard하기
    }

    public void RequestBang()
    {
        // 내 손에 빗나감이 있는지 확인
        for (int i = 0; i < myCard.Count; i++)
        {
            if (myCard[i].cardName == "BANG")
            {
                // 페이지 요청
                ReactBang.gameObject.SetActive(true);
                tempCard = myCard[i];
            }
            else
            {
                //Deny();
                checkCount++;
            }

            if (checkCount == myCard.Count)
                Deny();
        }
        checkCount = 0;
    }
    public void RequestDuello()
    {
        // 내 손에 빗나감이 있는지 확인
        for (int i = 0; i < myCard.Count; i++)
        {
            if (myCard[i].cardName == "BANG")
            {
                // 페이지 요청
                ReactDuello.gameObject.SetActive(true);
                tempCard = myCard[i];
            }
            else
            {
                //Deny();
                checkCount++;
            }

            if (checkCount == myCard.Count)
                Deny();
        }
        checkCount = 0;
    }

    public void EventMancato()
    {
        Debug.Log("빗나감 사용함, 오브젝트 꺼져라!");
        CPacket msg = CPacket.create((short)PROTOCOL.REACTION);
        msg.push(player_me_index);
        msg.push("MANCATO");
        network_manager.send(msg);

        tempCard.gameObject.SetActive(false);
        tempCard = null;

        AllReactShutDown();
    }

    public void EventBang()
    {
        CPacket msg = CPacket.create((short)PROTOCOL.REACTION);
        msg.push(player_me_index);
        msg.push("BANG");
        network_manager.send(msg);

        tempCard.gameObject.SetActive(false);
        tempCard = null;

        AllReactShutDown();
    }

    public void EventDuello()
    {
        CPacket msg = CPacket.create((short)PROTOCOL.REACTION);
        msg.push(player_me_index);
        msg.push("DUELLO");
        network_manager.send(msg);

        tempCard.gameObject.SetActive(false);
        tempCard = null;

        AllReactShutDown();
    }

    public void Deny()
    {
        CPacket msg = CPacket.create((short)PROTOCOL.REACTION);
        msg.push(player_me_index);
        msg.push("DENY");
        network_manager.send(msg); ;

        Debug.Log("공격 맞음. 오브젝트 꺼져라!");

        AllReactShutDown();
    }

    public void AllReactShutDown()
    {
        ReactMancato.gameObject.SetActive(false);
        ReactBang.gameObject.SetActive(false);
        ReactDuello.gameObject.SetActive(false);
    }
}
