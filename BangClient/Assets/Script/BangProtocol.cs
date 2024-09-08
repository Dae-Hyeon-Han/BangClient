using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BangGameServer
{
	public enum BangProtocol : short
	{
		BEGIN = 0,

		// 로딩을 시작해라.
		START_LOADING = 1,

		LOADING_COMPLETED = 2,

		// 게임 시작.
		GAME_START = 3,

		// 턴 시작.
		START_PLAYER_TURN = 4,

		// 채팅 입력
		PLAYER_CHAT = 5,

		// 채팅 받음(삭제 필요)
		PLAYER_CHAT_RECV = 6,

		// 플레이어의 턴에 선행할 행동
		PLAYER_FIRST_ACT = 7,

		// 플레이어의 턴에 행하는 일반 행동
		PLAYER_NORMAL_ACT = 8,

		// 클라이언트의 턴 연출이 끝났음을 알린다.
		TURN_FINISHED_REQ = 9,

		// 상대방 플레이어가 나가 방이 삭제되었다.
		ROOM_REMOVED = 10,

		// 게임방 입장 요청.
		ENTER_GAME_ROOM_REQ = 11,

		// 게임 종료.
		GAME_OVER = 12,

		END
	}
}
