using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FreeNet;
using BangGameServer;
using TMPro;

// BattleRoom ����
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
    public NetworkManager networkManager;

    // �α��� ��

    // ��ȭ�� �ؽ�Ʈ
    public TextMeshProUGUI RecvText;        // ��ȭ������ ��ϵǴ� â
    public TextMeshProUGUI SendText;        // ���� �Է��� �ؽ�Ʈ
    private string myId;                     // ê �� �� �� ���̵� ���� ���� ��

    // ���� ���¿� ���� ���� �ٸ� GUI ����� �����ϱ� ���� �ʿ��� ���� ����
    GAME_STATE game_state;

    // �¸��� �÷��̾� �ε���
    byte winPlayerIndex;

    // ���� ���� ���� �÷��̾ ��Ÿ���� ��ü. �� ����?
    //CBattleInfoPanel battleInfo;

    // ������ ���� �Ǿ������� ��Ÿ���� �÷���
    bool isGameFinished;

    public string MyId
    {
        get { return myId; }
        set { myId = value; }
    }

    // Update is called once per frame
    void Update()
    {
        if (isGameFinished)
        {
            if (Input.GetMouseButtonDown(0))
                BackToMain();
        }
    }

    // ���� ���� �ÿ� �ʱ�ȭ�� �޼���
    public void Clear()
    {
        this.current_player_index = 0;
        //this.step = 0;
        //this.draw = this.on_gui_playing;
        //this.is_game_finished = false;
    }

    /// <summary>
    /// ���ӹ濡 ������ �� ȣ��. ���ҽ� �ε��� ����
    /// </summary>
    public void StartLoading(byte playerMeIndex)
    {
        Debug.Log("�ε� ����");
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

        Debug.Log($"{protocol_id}");

        switch (protocol_id)
        {
            case BangProtocol.GAME_START:
                Debug.Log("���� ����");
                break;
            case BangProtocol.PLAYER_FIRST_ACT:
                Debug.Log("�÷��̾� ���� �۾�");
                break;
            case BangProtocol.PLAYER_NORMAL_ACT:
                Debug.Log("�÷��̾� �� �۾�");
                break;
            case BangProtocol.START_PLAYER_TURN:
                Debug.Log("��?��");
                break;
            case BangProtocol.ROOM_REMOVED:
                {
                    OnRoomRemoved();
                    Debug.Log("�� �ı�");
                }
                break;
            case BangProtocol.GAME_OVER:
                {
                    //OnGameOver();
                    Debug.Log("������");
                }
                break;
            case BangProtocol.PLAYER_CHAT:
                {
                    Debug.Log("�޽��� ����");
                    TextRecv(msg);
                }
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
        yield return null;
    }

    // ä�� �����⿡ ���� �޼���
    public void TextSend()
    {
        Debug.Log($"{SendText.text}�� ����");
        CPacket msg = CPacket.create((short)BangProtocol.PLAYER_CHAT);
        msg.push(myId+SendText.text);               // ���̵�� �޽��� ���̿� ���������� ���� �ʿ� ����
        networkManager.send(msg);
    }

    // ä�� �ޱ⿡ ���� �޼���
    void TextRecv(CPacket msg)
    {
        RecvText.text += msg.pop_string() + "\n";
    }
}
