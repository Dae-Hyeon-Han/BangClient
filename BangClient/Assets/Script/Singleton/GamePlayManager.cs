using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FreeNet;
using BangGameServer;


public class GamePlayManager : MonoBehaviour
{
    enum GAME_STATE
    {
        READY = 0,
        STARTED
    }

    List<CPlayer> players;

    // 현재 턴을 진행 중인 플레이어의 인덱스
    byte current_player_index;

    // 서버에서 지정해 준 본인의 플레이어 인덱스
    byte player_me_index;

    // 게임 종료 후 메인으로 돌아갈 때 사용하기 위한 MainTitle 객체의 레퍼런스
    public MainTitle mainTitle;

    // 네트워크 데이터 송,수신을 위한 네트워크 매니저 레퍼런스
    public CNetworkManager networkManager;

    // 게임 상태에 따라 각각 다른 GUI 모습을 구현하기 위해 필요한 상태 변수
    GAME_STATE game_state;

    // OnGui 메서드에서 호출할 델리게이트
    // 여러종류의 메서드를 만들어 놓고 상황에 맞게 draw에 대입해 주는 방식으로 gui를 변경시킨다.
    delegate void GUIFUNC();
    GUIFUNC draw;

    // 승리한 플레이어 인덱스
    byte winPlayerIndex;

    // 현재 진행 중인 플레이어를 나타내는 객체. 안 쓸듯?
    CBattleInfoPanel battleInfo;

    // 게임이 종료 되었는지를 나타내는 플래그
    bool isGameFinished;

    private void Awake()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(isGameFinished)
        {
            if (Input.GetMouseButtonDown(0))
                BackToMain();
        }
    }

    // 게임 시작 시에 초기화용 메서드
    public void Clear()
    {

    }

    /// <summary>
    /// 게임방에 입장할 때 호출. 리소스 로딩을 시작
    /// </summary>
    public void StartLoading(byte playerMeIndex)
    {
        Clear();
        networkManager.message_receiver = this;
        player_me_index = playerMeIndex;

        CPacket msg = CPacket.create((short)BangProtocol.LOADING_COMPLETED);
        networkManager.send(msg);
    }

    /// <summary>
    /// 패킷을 수신했을 때 호출됨.
    /// </summary>
    /// <param name="msg"></param>
    void OnRecv(CPacket msg)
    {
        BangProtocol protocol_id = (BangProtocol)msg.pop_protocol_id();

        switch(protocol_id)
        {
            case BangProtocol.GAME_START:
                break;
            case BangProtocol.PLAYER_FIRST_ACT:
                break;
            case BangProtocol.PLAYER_NORMAL_ACT:
                break;
            case BangProtocol.START_PLAYER_TURN:
                break;
            case BangProtocol.ROOM_REMOVED:
                break;
            case BangProtocol.GAME_OVER:
                break;
        }
    }

    void OnRoomRemoved()
    {
        if (!isGameFinished)
            BackToMain();
    }

    void BackToMain()
    {
        mainTitle.gameObject.SetActive(false);
        mainTitle.Enter();
        gameObject.SetActive(false);
    }

    void OnGameOver(CPacket msg)
    {
        isGameFinished = true;
        winPlayerIndex = msg.pop_byte();
        // draw
    }

    // 감옥, 다이너마이트 등으로 주사위 굴리기를 할 때
    void OnPlayerFirstAct(CPacket msg)
    {
        byte playerIndex = msg.pop_byte();
        short from = msg.pop_int16();

        
    }

    // 턴 시작 전 주사위 굴리기에 대한 코드
    IEnumerator GambleDice(byte playerIndex, short number)
    {

    }
}
