using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FreeNet;
using BangGameServer;
using TMPro;

public class MainTitle : MonoBehaviour
{
    public NetworkManager networkManager;
    public GamePlayManager gamePlayManager;
    public Canvas LogInCanvas;
    public Canvas WatingMatching;
    public TextMeshProUGUI id;
    bool isInput;

    USER_STATE user_state;

    enum USER_STATE
    {
        NOT_CONNECTED,
        CONNECTED,
        WAITING_MATCHING
    }

    // 게임이 시작되면 서버 연결 전까지 초기화할 작업.
    void Start()
    {
        user_state = USER_STATE.NOT_CONNECTED;
        gamePlayManager.gameObject.SetActive(false);
        WatingMatching.gameObject.SetActive(false);
        user_state = USER_STATE.NOT_CONNECTED;        // 왜 한번 더 있는지?
        Enter();
    }

    private void Update()
    {
        //switch (user_state)
        //{
        //    // 연결 전
        //    case USER_STATE.NOT_CONNECTED:
        //        {
        //            LogInCanvas.gameObject.SetActive(true);
        //            gamePlayManager.gameObject.SetActive(false);
        //            WatingMatching.gameObject.SetActive(false);
        //            Debug.Log("연결1");
        //        }
        //        break;

        //    // 연결 후
        //    case USER_STATE.CONNECTED:
        //        {
        //            LogInCanvas.gameObject.SetActive(false);
        //            gamePlayManager.gameObject.SetActive(true);
        //            WatingMatching.gameObject.SetActive(false);
        //            Debug.Log("연결2");
        //        }
        //        break;

        //    // 매칭 대기 중일 때
        //    case USER_STATE.WAITING_MATCHING:
        //        {
        //            LogInCanvas.gameObject.SetActive(false);
        //            gamePlayManager.gameObject.SetActive(false);
        //            WatingMatching.gameObject.SetActive(true);
        //            Debug.Log("연결3");
        //        }
        //        break;
        //}
    }

    public void Enter()
    {
        StopCoroutine("after_connected");
        networkManager.message_receiver = this;

        if (!networkManager.is_connected())
        {
            user_state = USER_STATE.CONNECTED;
            networkManager.connect();
        }
        else
        {
            on_connected();
        }
    }

    IEnumerator after_connected()
    {
        yield return new WaitForEndOfFrame();

        while (true)
        {
            Debug.Log($"isInput: {isInput}");

            // 연결 시도 중이며, 인풋이 트루면 접속함
            if (user_state == USER_STATE.CONNECTED && isInput)
            {
                user_state = USER_STATE.WAITING_MATCHING;

                // 패킷 to server 예시?
                CPacket msg = CPacket.create((short)BangProtocol.ENTER_GAME_ROOM_REQ);
                networkManager.send(msg);

                Debug.Log("연결!");

                StopCoroutine("after_connected");
            }

            yield return 0;
        }
    }

    public void on_connected()
    {
        user_state = USER_STATE.CONNECTED;

        StartCoroutine("after_connected");
    }

    /// <summary>
	/// 패킷을 수신 했을 때 호출됨.
	/// </summary>
	/// <param name="protocol"></param>
	/// <param name="msg"></param>
	public void OnRecv(CPacket msg)
    {
        // 제일 먼저 프로토콜 아이디를 꺼내온다.
        BangProtocol protocol_id = (BangProtocol)msg.pop_protocol_id();
        Debug.Log($"{protocol_id}");

        // 현재 문제 : StartLoading 이 먼저 와야함
        switch (protocol_id)
        {
            case BangProtocol.START_LOADING:
                {
                    byte player_index = msg.pop_byte();

                    gamePlayManager.gameObject.SetActive(true);
                    gamePlayManager.StartLoading(player_index);
                    WatingMatching.gameObject.SetActive(false);
                    gameObject.SetActive(false);
                }
                break;
        }
    }

    public void InputId()
    {
        // 대기 중 화면을 그리는 메서드 추가 요망
        //gamePlayManager.gameObject.SetActive(true);
        gamePlayManager.MyId = id.text;
        LogInCanvas.gameObject.SetActive(false);
        WatingMatching.gameObject.SetActive(true);
        isInput = true;
    }
}
