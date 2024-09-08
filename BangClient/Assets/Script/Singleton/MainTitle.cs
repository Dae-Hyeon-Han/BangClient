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

    // ������ ���۵Ǹ� ���� ���� ������ �ʱ�ȭ�� �۾�.
    void Start()
    {
        user_state = USER_STATE.NOT_CONNECTED;
        gamePlayManager.gameObject.SetActive(false);
        WatingMatching.gameObject.SetActive(false);
        user_state = USER_STATE.NOT_CONNECTED;        // �� �ѹ� �� �ִ���?
        Enter();
    }

    private void Update()
    {
        //switch (user_state)
        //{
        //    // ���� ��
        //    case USER_STATE.NOT_CONNECTED:
        //        {
        //            LogInCanvas.gameObject.SetActive(true);
        //            gamePlayManager.gameObject.SetActive(false);
        //            WatingMatching.gameObject.SetActive(false);
        //            Debug.Log("����1");
        //        }
        //        break;

        //    // ���� ��
        //    case USER_STATE.CONNECTED:
        //        {
        //            LogInCanvas.gameObject.SetActive(false);
        //            gamePlayManager.gameObject.SetActive(true);
        //            WatingMatching.gameObject.SetActive(false);
        //            Debug.Log("����2");
        //        }
        //        break;

        //    // ��Ī ��� ���� ��
        //    case USER_STATE.WAITING_MATCHING:
        //        {
        //            LogInCanvas.gameObject.SetActive(false);
        //            gamePlayManager.gameObject.SetActive(false);
        //            WatingMatching.gameObject.SetActive(true);
        //            Debug.Log("����3");
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

            // ���� �õ� ���̸�, ��ǲ�� Ʈ��� ������
            if (user_state == USER_STATE.CONNECTED && isInput)
            {
                user_state = USER_STATE.WAITING_MATCHING;

                // ��Ŷ to server ����?
                CPacket msg = CPacket.create((short)BangProtocol.ENTER_GAME_ROOM_REQ);
                networkManager.send(msg);

                Debug.Log("����!");

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
	/// ��Ŷ�� ���� ���� �� ȣ���.
	/// </summary>
	/// <param name="protocol"></param>
	/// <param name="msg"></param>
	public void OnRecv(CPacket msg)
    {
        // ���� ���� �������� ���̵� �����´�.
        BangProtocol protocol_id = (BangProtocol)msg.pop_protocol_id();
        Debug.Log($"{protocol_id}");

        // ���� ���� : StartLoading �� ���� �;���
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
        // ��� �� ȭ���� �׸��� �޼��� �߰� ���
        //gamePlayManager.gameObject.SetActive(true);
        gamePlayManager.MyId = id.text;
        LogInCanvas.gameObject.SetActive(false);
        WatingMatching.gameObject.SetActive(true);
        isInput = true;
    }
}
