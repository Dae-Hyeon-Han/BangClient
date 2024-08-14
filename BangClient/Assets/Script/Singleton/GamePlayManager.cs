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

    // ���� ���� ���� ���� �÷��̾��� �ε���
    byte current_player_index;

    // �������� ������ �� ������ �÷��̾� �ε���
    byte player_me_index;

    // ���� ���� �� �������� ���ư� �� ����ϱ� ���� MainTitle ��ü�� ���۷���
    public MainTitle mainTitle;

    // ��Ʈ��ũ ������ ��,������ ���� ��Ʈ��ũ �Ŵ��� ���۷���
    public CNetworkManager networkManager;

    // ���� ���¿� ���� ���� �ٸ� GUI ����� �����ϱ� ���� �ʿ��� ���� ����
    GAME_STATE game_state;

    // OnGui �޼��忡�� ȣ���� ��������Ʈ
    // ���������� �޼��带 ����� ���� ��Ȳ�� �°� draw�� ������ �ִ� ������� gui�� �����Ų��.
    delegate void GUIFUNC();
    GUIFUNC draw;

    // �¸��� �÷��̾� �ε���
    byte winPlayerIndex;

    // ���� ���� ���� �÷��̾ ��Ÿ���� ��ü. �� ����?
    CBattleInfoPanel battleInfo;

    // ������ ���� �Ǿ������� ��Ÿ���� �÷���
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

    // ���� ���� �ÿ� �ʱ�ȭ�� �޼���
    public void Clear()
    {

    }

    /// <summary>
    /// ���ӹ濡 ������ �� ȣ��. ���ҽ� �ε��� ����
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
    /// ��Ŷ�� �������� �� ȣ���.
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

    // ����, ���̳ʸ���Ʈ ������ �ֻ��� �����⸦ �� ��
    void OnPlayerFirstAct(CPacket msg)
    {
        byte playerIndex = msg.pop_byte();
        short from = msg.pop_int16();

        
    }

    // �� ���� �� �ֻ��� �����⿡ ���� �ڵ�
    IEnumerator GambleDice(byte playerIndex, short number)
    {

    }
}
