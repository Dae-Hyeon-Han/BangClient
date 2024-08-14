using System;

namespace BangGameServer
{
    public enum BangProtocol
    {
		BEGIN = 0,

		// �ε��� �����ض�.
		START_LOADING = 1,

		LOADING_COMPLETED = 2,

		// ���� ����.
		GAME_START = 3,

		// �� ����.
		START_PLAYER_TURN = 4,

		// �÷��̾��� �Ͽ� ������ �ൿ
		PLAYER_FIRST_ACT = 5,

		// �÷��̾��� �Ͽ� ���ϴ� �Ϲ� �ൿ
		PLAYER_NORMAL_ACT = 6,

		// Ŭ���̾�Ʈ�� �� ������ �������� �˸���.
		TURN_FINISHED_REQ = 7,

		// ���� �÷��̾ ���� ���� �����Ǿ���.
		ROOM_REMOVED = 8,

		// ���ӹ� ���� ��û.
		ENTER_GAME_ROOM_REQ = 9,

		// ���� ����.
		GAME_OVER = 10,

		END
	}
}