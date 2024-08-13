using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FreeNet;
using BangGameServer;

public class MainTitle : MonoBehaviour
{
    public NetworkManager networkManager;
    public GamePlayManager gamePlayManager;

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
        this.user_state = USER_STATE.NOT_CONNECTED;
        gamePlayManager.gameObject.SetActive(false);
        this.user_state = USER_STATE.NOT_CONNECTED;
        Enter();
    }

    public void Enter()
    {
        StopCoroutine("after_connected");
        networkManager.message_receiver = this;

        if(!this.networkManager.is_connected())
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

        while(true)
        {
            if(user_state == USER_STATE.CONNECTED)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    user_state = USER_STATE.WAITING_MATCHING;

                    CPacket msg = CPacket.create((short)PROTOCOL.ENTER_GAME_ROOM_REQ);
                    networkManager.send(msg);

                    StopCoroutine("after_connected");
                }
            }

            yield return 0;
        }
    }

    // GUI 그리는 부분
    void OnGUI()
    {
        switch (this.user_state)
        {
            case USER_STATE.NOT_CONNECTED:
                break;

            case USER_STATE.CONNECTED:
                Debug.Log("연결 중");
                break;

            case USER_STATE.WAITING_MATCHING:
                Debug.Log("매칭 됨");
                break;
        }
    }

    public void on_connected()
    {
        this.user_state = USER_STATE.CONNECTED;

        StartCoroutine("after_connected");
    }
}
