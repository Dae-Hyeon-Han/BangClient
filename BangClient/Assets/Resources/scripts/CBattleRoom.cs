using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using FreeNet;
using BangGameServer;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CBattleRoom : MonoBehaviour
{

    enum GAME_STATE
    {
        READY = 0,
        STARTED
    }

    // 가로, 세로 칸 수를 의미한다.
    public static readonly int COL_COUNT = 7;

    List<CPlayer> players;

    // 현재 턴을 진행중인 플레이어 인덱스.
    byte current_player_index;

    // 서버에서 지정해준 본인의 플레이어 인덱스.
    byte player_me_index;

    // 상황에 따른 터치 입력을 처리하기 위한 변수.
    byte step;

    // 게임 종료 후 메인으로 돌아갈 때 사용하기 위한 MainTitle객체의 레퍼런스.
    CMainTitle main_title;

    // 네트워크 데이터 송,수신을 위한 네트워크 매니저 레퍼런스.
    CNetworkManager network_manager;

    // 게임 상태에 따라 각각 다른 GUI모습을 구현하기 위해 필요한 상태 변수.
    GAME_STATE game_state;

    // OnGUI매소드에서 호출할 델리게이트.
    // 여러 종류의 매소드를 만들어 놓고 상황에 맞게 draw에 대입해주는 방식으로 GUI를 변경시킨다.
    delegate void GUIFUNC();
    GUIFUNC draw;

    // 승리한 플레이어 인덱스.
    // 무승부일때는 byte.MaxValue가 들어간다.
    byte win_player_index;

    // 점수를 표시하기 위한 이미지 숫자 객체.
    // 선명하고 예쁘게 표현하기 위해 폰트 대신 이미지로 만들어 사용한다.
    //CImageNumber score_images;

    // 현재 진행중인 플레이어를 나타내는 객체.
    CBattleInfoPanel battle_info;

    // 게임이 종료되었는지를 나타내는 플래그.
    bool is_game_finished;

    #region 뱅 용
    // 캐릭터 선택창
    [SerializeField] Canvas CharacterPick;
    public Transform playerGroup;
    string characterNameLeft;
    string characterNameRight;
    
    // 플레이어 정보 처리용
    Dictionary<string, Transform> playerIndex = new Dictionary<string, Transform>();        // 숫자 출력용
    Dictionary<string, Image> playerCharImage = new Dictionary<string, Image>();            // 숫자 출력용
    Dictionary<string, Transform> playerObj = new Dictionary<string, Transform>();          // 실제 제어용
    
    // 컨트롤러
    public PlayerController controller;

    // 플레잉 카드 사용시 구분용
    public List<GameObject> Cards = new List<GameObject>();
    List<Card> HandCard = new List<Card>();

    // 디버그 용
    Card card;
    EventTrigger trigger;
    EventTrigger.Entry entry_PointerEnter = new EventTrigger.Entry();
    
    #endregion
    //public enum Characters
    //   {
    //	character1,
    //	character2
    //   }

    void Awake()
    {
        this.network_manager = GameObject.Find("NetworkManager").GetComponent<CNetworkManager>();

        this.game_state = GAME_STATE.READY;

        this.main_title = GameObject.Find("MainTitle").GetComponent<CMainTitle>();

        this.win_player_index = byte.MaxValue;
        //this.battle_info = gameObject.AddComponent<CBattleInfoPanel>();

        // 이거 쓸모 없나?
        foreach (Transform players in playerGroup)
        {
            playerIndex[players.name] = players.GetChild(0);
            playerCharImage[players.name] = players.GetChild(1).GetComponent<Image>();
            playerObj[players.name] = players;

            //Debug.Log($"플레이어 이름: {playerCharImage[players.name].name}");
        }
    }

    // 게임 매칭이 되면 게임룸 오브젝트가 자동으로 활성화되므로, 사실상 매칭 완료 후 첫 페이지 액션.
    private void OnEnable()
    {
        //CharacterPick.gameObject.SetActive(true);
    }

    void reset()
    {
        // 보드판 데이터를 모두 초기화 한다.
    }


    void clear()
    {
        this.current_player_index = 0;
        this.is_game_finished = false;
    }

    /// <summary>
    /// 게임방에 입장할 때 호출된다. 리소스 로딩을 시작한다.
    /// </summary>
    public void start_loading(byte player_me_index)
    {
        clear();

        this.network_manager.message_receiver = this;
        this.player_me_index = player_me_index;

        controller.SetMyNumber(player_me_index);

        CPacket msg = CPacket.create((short)PROTOCOL.LOADING_COMPLETED);
        this.network_manager.send(msg);

        // 나와 다른 플레이어의 인덱스 번호 시각화
        int j=0;
        
        for(int i=0; i<7; i++)
        {
            j = player_me_index + i;
            if (j < 7)
                playerIndex["player" + i].GetComponent<TextMeshProUGUI>().text = j + "번 플레이어";
            else
                playerIndex["player" + i].GetComponent<TextMeshProUGUI>().text = j - 7 + "번 플레이어";
        }
    }


    /// <summary>
    /// 패킷을 수신 했을 때 호출됨.
    /// </summary>
    /// <param name="protocol"></param>
    /// <param name="msg"></param>
    void on_recv(CPacket msg)
    {
        PROTOCOL protocol_id = (PROTOCOL)msg.pop_protocol_id();


        #region 왜 이 코드를 쓰면 오브젝트가 안 생김?
        //if (string.IsNullOrEmpty(msg.pop_string()))
        //{
        //    Debug.Log("Using Card");
        //    useCard = msg.pop_string();
        //}
        #endregion

        // USECARD 프로토콜의 경우, 어떻게 받아올지?

        switch (protocol_id)
        {
            case PROTOCOL.GAME_START:
                on_game_start(msg);
                Debug.Log("게임 시작!");
                break;

            case PROTOCOL.PLAYER_MOVED:         // 다른 플레이어가 움직였을 때?
                on_player_moved(msg);
                Debug.Log("움직임!");
                break;
            case PROTOCOL.USECARD:
                {
                    Debug.Log("카드 사용!");
                }
                break;
            case PROTOCOL.CHAT:
                {
                    controller.ChatReceive(msg.pop_string());
                }
                break;
            case PROTOCOL.START_PLAYER_TURN:
                on_start_player_turn(msg);
                Debug.Log("플레이어 턴 시작!");
                break;

            case PROTOCOL.ROOM_REMOVED:
                on_room_removed();
                Debug.Log("방 파괴!");
                break;

            case PROTOCOL.GAME_OVER:
                on_game_over(msg);
                Debug.Log("게임 오버!");
                break;
        }
    }


    void on_room_removed()
    {
        if (!is_game_finished)
        {
            back_to_main();
        }
    }


    void back_to_main()
    {
        this.main_title.gameObject.SetActive(true);
        this.main_title.enter();

        gameObject.SetActive(false);
    }


    void on_game_over(CPacket msg)
    {
        this.is_game_finished = true;
        this.win_player_index = msg.pop_byte();
        this.draw = this.on_gui_game_result;
    }


    void Update()
    {
        if (this.is_game_finished)
        {
            if (Input.GetMouseButtonDown(0))
            {
                back_to_main();
            }
        }
    }

    void on_game_start(CPacket msg)
    {
        this.players = new List<CPlayer>();


        byte count = msg.pop_byte();

        // 디버그
        //PlayerHandCard_FirstSet();

        for (byte i = 0; i < count; ++i)
        {
            byte player_index = msg.pop_byte();
            string charName = msg.pop_string();
            int life = msg.pop_int32();
            string job = msg.pop_string();

            GameObject obj = new GameObject(string.Format("player{0}", i));
            CPlayer player = obj.AddComponent<CPlayer>();
            player.initialize(player_index, charName, life, job);
            player.clear();

            // 플레이어 캐릭터 정리
            playerCharImage[$"player{i}"].sprite = Resources.Load<Sprite>("Images/Char/Char_" + charName);

            //Debug.Log($"인덱스: {player_index}");
            //Debug.Log($"{i}의 캐릭터 이름: {charName}");


            this.players.Add(player);
        }

        for (int i = 0; i < 7; i++)
        {
            //playerCharImage[$"player{i}"].sprite = Resources.Load<Sprite>("Images/Char/Char_" + charName);
        }




        this.current_player_index = msg.pop_byte();
        reset();

        this.game_state = GAME_STATE.STARTED;
    }

    #region 기존 코드
    void on_player_moved(CPacket msg)
    {
        byte player_index = msg.pop_byte();
        short from = msg.pop_int16();
        short to = msg.pop_int16();

        StartCoroutine(on_selected_cell_to_attack(player_index, from, to));
    }


    void on_start_player_turn(CPacket msg)
    {
        phase_end();

        this.current_player_index = msg.pop_byte();
    }



    //float ratio = 1.0f;
    void OnGUI()
    {
        //this.draw();
    }


    /// <summary>
    /// 게임 진행 화면 그리기.
    /// </summary>
    void on_gui_playing()
    {
        if (this.game_state != GAME_STATE.STARTED)
        {
            return;
        }

        draw_board();
    }


    /// <summary>
    /// 결과 화면 그리기.
    /// </summary>
    void on_gui_game_result()
    {
        on_gui_playing();
    }

    void draw_board()
    {

    }


    // 적 셀 공격을 택했을 때
    IEnumerator on_selected_cell_to_attack(byte player_index, short from, short to)
    {
        //byte distance = CHelper.howfar_from_clicked_cell(from, to);
        //if (distance == 1)
        //{
        //    // copy to cell
        //    yield return StartCoroutine(reproduce(to));
        //}
        //else if (distance == 2)
        //{
        //    // move
        //    //this.board[from] = short.MaxValue;
        //    this.players[player_index].remove(from);
        //    yield return StartCoroutine(reproduce(to));
        //}

        //CPacket msg = CPacket.create((short)PROTOCOL.TURN_FINISHED_REQ);
        //this.network_manager.send(msg);

        yield return 0;
    }

    void phase_end()
    {
        this.step = 0;
    }


    void refresh_available_cells(short cell)
    {

    }

    void clear_available_attacking_cells()
    {

    }

    //IEnumerator reproduce(short cell)
    //{
    //    CPlayer current_player = this.players[this.current_player_index];
    //    CPlayer other_player = this.players.Find(obj => obj.player_index != this.current_player_index);

    //    clear_available_attacking_cells();
    //    //yield return new WaitForSeconds(0.5f);

    //    current_player.add(cell);

    //    yield return new WaitForSeconds(0.5f);

    //    // eat.
    //    List<short> neighbors = CHelper.find_neighbor_cells(cell, other_player.cell_indexes, 1);
    //    foreach (short obj in neighbors)
    //    {
    //        current_player.add(obj);

    //        other_player.remove(obj);

    //        yield return new WaitForSeconds(0.2f);
    //    }
    //}

    // 이게 뭔지?
    bool validate_begin_cell(short cell)
    {
        return this.players[this.current_player_index].cell_indexes.Exists(obj => obj == cell);
    }
    #endregion

    #region 여기서부터 뱅용 메서드
    // 플레이어가 버튼(?) 클릭 할 때
    void on_click(short cell)
    {
        // 자신의 차례가 아니면 처리하지 않고 리턴한다.
        if (this.player_me_index != this.current_player_index)
        {
            return;
        }
    }

    public void CharacterChoice(int characterName)
    {
        string charName;        // 실제 서버에 전송될 캐릭터 이름 변수

        Debug.Log($"캐릭터 이름: {characterName}");
        CPacket msg = CPacket.create((short)PROTOCOL.GAME_START);       // 프로토콜 확실히 정리 후 다시 작성할 것

        if (characterName == 0)
            charName = characterNameLeft;
        else
            charName = characterNameRight;

        msg.push(charName);
        //this.network_manager.send(msg);
    }

    private void CharNameSet()
    {
        List<string> charName = new List<string>();
    }

    // 디버그용. 카드 이벤트 확인용
    public void PlayerHandCard_FirstSet()
    {
        //Debug.Log($"숫자: {playerObj["player0"].GetChild(4).childCount}");

        for (int i = 0; i < playerObj["player0"].GetChild(4).childCount; i++)
        {
            // 카드 기능
            Cards.Add(playerObj["player0"].GetChild(4).GetChild(i).gameObject);                     // 손패 리스트 push
            card = playerObj["player0"].GetChild(4).GetChild(i).gameObject.AddComponent<Bang>();
            playerObj["player0"].GetChild(4).GetChild(i).gameObject.GetComponent<Button>()
                .onClick.AddListener(card.UseCard);

            // 카드 기능 뷰
            trigger = playerObj["player0"].GetChild(4).GetChild(i).gameObject.AddComponent<EventTrigger>();
            entry_PointerEnter.eventID = EventTriggerType.PointerEnter;
            entry_PointerEnter.callback.AddListener((data) => { UseViewUi((PointerEventData)data); });
            trigger.triggers.Add(entry_PointerEnter);
        }
    }

    // 이게 아닌거 같은디....;;
    public void PlayerHandCard_Set(string cardName)
    {
        // HandCards obj
        for(int i=0; i< playerIndex["player0"].GetChild(4).childCount; i++)
        {
            Cards.Add(playerIndex["player0"].GetChild(4).GetChild(i).gameObject);
        }
    }

    public void OtherPlayerCardSet(CPacket msg)
    {
        // 프로토콜 확정 시 작성
    }

    public void UseCardEvent(string cardName)
    {
        Debug.Log("실제로 쏨");

        CPacket msg = CPacket.create((short)PROTOCOL.USECARD);

        //if()
    }

    // 왜 11개가 호출되지?
    public void UseViewUi(PointerEventData data)
    {
        Debug.Log("뷰 띄우기");
    }

    public void UseViewUi(Transform textPro, bool mouseIn, string funcText)
    {

    }

    public void TurnEnd()
    {
        Debug.Log("턴 엔드 버튼 이벤트 등록 필요");

        CPacket msg = CPacket.create((short)PROTOCOL.TURN_FINISHED_REQ);
        msg.push(player_me_index);
        this.network_manager.send(msg);
    }
    #endregion
}
