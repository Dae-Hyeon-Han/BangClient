using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FreeNet;
using BangGameServer;

public class React : MonoBehaviour
{
    CBattleRoom battleRoom;
    CNetworkManager networkManager;

    private void Start()
    {
        battleRoom = GameObject.Find("BattleRoom").GetComponent<CBattleRoom>();
    }


    /// <summary>
    /// 패킷 내용은, [프로토콜][피격 카드 이름][내 인덱스]
    /// </summary>


    // 뱅 맞은 경우 -> Yes
    public void Banged()
    {
        CPacket msg = CPacket.create((short)PROTOCOL.REACTION);
        msg.push("BANG");
        msg.push($"{battleRoom.player_me_index}");
        networkManager.send(msg);
    }
    
    // 결투 맞은 경우 -> Yes
    public void Duelloed()
    {
        CPacket msg = CPacket.create((short)PROTOCOL.REACTION);
        msg.push("DUELLO");
        msg.push($"{battleRoom.player_me_index}");
        networkManager.send(msg);
    }

    // 기관총 맞은 경우 -> Yes
    public void Gatlinged()
    {
        CPacket msg = CPacket.create((short)PROTOCOL.REACTION);
        msg.push("GATTLING");
        msg.push($"{battleRoom.player_me_index}");
        networkManager.send(msg);
    }

    // 인디언 맞은 경우 -> Yes
    public void Indianied()
    {
        CPacket msg = CPacket.create((short)PROTOCOL.REACTION);
        msg.push("INDIANI");
        msg.push($"{battleRoom.player_me_index}");
        networkManager.send(msg);
    }

    // 어떤 경우든, No 선택 시
    public void DenyReact(string cardName)
    {
        CPacket msg = CPacket.create((short)PROTOCOL.REACTION);
        msg.push(cardName);
        msg.push($"{battleRoom.player_me_index}");
        networkManager.send(msg);
    }

    // 잡화점. 메서드 추가 예정
}
