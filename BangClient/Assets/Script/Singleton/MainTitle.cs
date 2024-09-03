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

    // ������ ���۵Ǹ� ���� ���� ������ �ʱ�ȭ�� �۾�.
    void Start()
    {
        user_state = USER_STATE.NOT_CONNECTED;
        gamePlayManager.gameObject.SetActive(false);
        user_state = USER_STATE.NOT_CONNECTED;
        Enter();
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
            if (user_state == USER_STATE.CONNECTED)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    user_state = USER_STATE.WAITING_MATCHING;

                    // ��Ŷ to server ����?
                    CPacket msg = CPacket.create((short)BangProtocol.ENTER_GAME_ROOM_REQ);
                    networkManager.send(msg);

                    Debug.Log("����!");

                    StopCoroutine("after_connected");
                }
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
	/// ��Ŷ�� ���� ���� �� ȣ���.
	/// </summary>
	/// <param name="protocol"></param>
	/// <param name="msg"></param>
	public void on_recv(CPacket msg)
    {
        // ���� ���� �������� ���̵� �����´�.
        BangProtocol protocol_id = (BangProtocol)msg.pop_protocol_id();
        Debug.Log($"{protocol_id}");

        switch (protocol_id)
        {
            case BangProtocol.START_LOADING:
                {
                    byte player_index = msg.pop_byte();
                    gamePlayManager.gameObject.SetActive(true);
                    gamePlayManager.StartLoading(player_index);
                    gameObject.SetActive(false);
                }
                break;
        }
    }
}
