using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BangGameServer
{
	public enum BangProtocol : short
	{
		BEGIN = 0,

		// �ε��� �����ض�.
		START_LOADING = 1,

		LOADING_COMPLETED = 2,

		// ���� ����.
		GAME_START = 3,

		// �� ����.
		START_PLAYER_TURN = 4,

		// ä�� �Է�
		PLAYER_CHAT_SEND = 5,

		// ä�� ����
		PLAYER_CHAT_RECV = 6,

		// �÷��̾��� �Ͽ� ������ �ൿ
		PLAYER_FIRST_ACT = 7,

		// �÷��̾��� �Ͽ� ���ϴ� �Ϲ� �ൿ
		PLAYER_NORMAL_ACT = 8,

		// Ŭ���̾�Ʈ�� �� ������ �������� �˸���.
		TURN_FINISHED_REQ = 9,

		// ���� �÷��̾ ���� ���� �����Ǿ���.
		ROOM_REMOVED = 10,

		// ���ӹ� ���� ��û.
		ENTER_GAME_ROOM_REQ = 11,

		// ���� ����.
		GAME_OVER = 12,

		END
	}
}
