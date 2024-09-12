using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using FreeNet;
using BangGameServer;

public class CBattleRoom : MonoBehaviour {

	enum GAME_STATE
	{
		READY = 0,
		STARTED
	}

	// ����, ���� ĭ ���� �ǹ��Ѵ�.
	public static readonly int COL_COUNT = 7;

	List<CPlayer> players;

	// �������� ���� �������� ���¸� ��Ÿ���� ������.
	//List<short> board;

	// 0~49������ �ε����� ���� �ִ� ������ ������.
	//List<short> table_board;

	// ���� ������ ������ ��Ÿ�� �� ����ϴ� ����Ʈ.
	//List<short> available_attack_cells;

	// ���� ���� �������� �÷��̾� �ε���.
	byte current_player_index;

	// �������� �������� ������ �÷��̾� �ε���.
	byte player_me_index;

	// ��Ȳ�� ���� ��ġ �Է��� ó���ϱ� ���� ����.
	byte step;

	// ���� ���� �� �������� ���ư� �� ����ϱ� ���� MainTitle��ü�� ���۷���.
	CMainTitle main_title;

	// ��Ʈ��ũ ������ ��,������ ���� ��Ʈ��ũ �Ŵ��� ���۷���.
	CNetworkManager network_manager;

	// ���� ���¿� ���� ���� �ٸ� GUI����� �����ϱ� ���� �ʿ��� ���� ����.
	GAME_STATE game_state;

	// OnGUI�żҵ忡�� ȣ���� ��������Ʈ.
	// ���� ������ �żҵ带 ����� ���� ��Ȳ�� �°� draw�� �������ִ� ������� GUI�� �����Ų��.
	delegate void GUIFUNC();
	GUIFUNC draw;

	// �¸��� �÷��̾� �ε���.
	// ���º��϶��� byte.MaxValue�� ����.
	byte win_player_index;

	// ������ ǥ���ϱ� ���� �̹��� ���� ��ü.
	// �����ϰ� ���ڰ� ǥ���ϱ� ���� ��Ʈ ��� �̹����� ����� ����Ѵ�.
	//CImageNumber score_images;

	// ���� �������� �÷��̾ ��Ÿ���� ��ü.
	CBattleInfoPanel battle_info;

	// ������ ����Ǿ������� ��Ÿ���� �÷���.
	bool is_game_finished;

	// ���� �̹��� �ؽ��ĵ�.
	//List<Texture> img_players;
	//Texture background;
	//Texture blank_image;
	//Texture game_board;

	//Texture graycell;
	//Texture focus_cell;

	//Texture win_img;
	//Texture lose_img;
	//Texture draw_img;
	//Texture gray_transparent;

	void Awake()
	{
		this.network_manager = GameObject.Find("NetworkManager").GetComponent<CNetworkManager>();

		this.game_state = GAME_STATE.READY;

		this.main_title = GameObject.Find("MainTitle").GetComponent<CMainTitle>();

		this.win_player_index = byte.MaxValue;
		this.battle_info = gameObject.AddComponent<CBattleInfoPanel>();
	}
	
	void reset()
	{
		// ������ �����͸� ��� �ʱ�ȭ �Ѵ�.
	}


	void clear()
	{
		this.current_player_index = 0;
		this.is_game_finished = false;
	}

	/// <summary>
	/// ���ӹ濡 ������ �� ȣ��ȴ�. ���ҽ� �ε��� �����Ѵ�.
	/// </summary>
	public void start_loading(byte player_me_index)
	{
		clear();

		this.network_manager.message_receiver = this;
		this.player_me_index = player_me_index;

		CPacket msg = CPacket.create((short)PROTOCOL.LOADING_COMPLETED);
		this.network_manager.send(msg);
	}


	/// <summary>
	/// ��Ŷ�� ���� ���� �� ȣ���.
	/// </summary>
	/// <param name="protocol"></param>
	/// <param name="msg"></param>
	void on_recv(CPacket msg)
	{
		PROTOCOL protocol_id = (PROTOCOL)msg.pop_protocol_id();

		switch (protocol_id)
		{
			case PROTOCOL.GAME_START:
				on_game_start(msg);
				Debug.Log("���� ����!");
				break;

			case PROTOCOL.PLAYER_MOVED:			// �ٸ� �÷��̾ �������� ��?
				on_player_moved(msg);
				Debug.Log("������!");
				break;

			case PROTOCOL.START_PLAYER_TURN:
				on_start_player_turn(msg);
				Debug.Log("�÷��̾� �� ����!");
				break;

			case PROTOCOL.ROOM_REMOVED:
				on_room_removed();
				Debug.Log("�� �ı�!");
				break;

			case PROTOCOL.GAME_OVER:
				on_game_over(msg);
				Debug.Log("���� ����!");
				break;
		}
	}


	void on_room_removed()
	{
		if (!is_game_finished)
		{
			back_to_main();
		}
	}


	void back_to_main()
	{
		this.main_title.gameObject.SetActive(true);
		this.main_title.enter();

		gameObject.SetActive(false);
	}


	void on_game_over(CPacket msg)
	{
		this.is_game_finished = true;
		this.win_player_index = msg.pop_byte();
		this.draw = this.on_gui_game_result;
	}


	void Update()
	{
		if (this.is_game_finished)
		{
			if (Input.GetMouseButtonDown(0))
			{
				back_to_main();
			}
		}
	}

	void on_game_start(CPacket msg)
	{
		this.players = new List<CPlayer>();

		byte count = msg.pop_byte();
		for (byte i = 0; i < count; ++i)
		{
			byte player_index = msg.pop_byte();

			GameObject obj = new GameObject(string.Format("player{0}", i));
			CPlayer player = obj.AddComponent<CPlayer>();
			player.initialize(player_index);
			player.clear();

			byte virus_count = msg.pop_byte();
			for (byte index = 0; index < virus_count; ++index)
			{
				short position = msg.pop_int16();
				player.add(position);
			}

			this.players.Add(player);
		}

		this.current_player_index = msg.pop_byte();
		reset();

		this.game_state = GAME_STATE.STARTED;
	}


	void on_player_moved(CPacket msg)
	{
		byte player_index = msg.pop_byte();
		short from = msg.pop_int16();
		short to = msg.pop_int16();

		StartCoroutine(on_selected_cell_to_attack(player_index, from, to));
	}


	void on_start_player_turn(CPacket msg)
	{
		phase_end();

		this.current_player_index = msg.pop_byte();
	}



	//float ratio = 1.0f;
	void OnGUI()
	{
		//this.draw();
	}


	/// <summary>
	/// ���� ���� ȭ�� �׸���.
	/// </summary>
	void on_gui_playing()
	{
		if (this.game_state != GAME_STATE.STARTED)
		{
			return;
		}

		draw_board();
	}


	/// <summary>
	/// ��� ȭ�� �׸���.
	/// </summary>
	void on_gui_game_result()
	{
		on_gui_playing();
	}
	
	void draw_board()
	{
		
	}

	// �÷��̾ ��ư(?) Ŭ�� �� ��
	void on_click(short cell)
	{
		// �ڽ��� ���ʰ� �ƴϸ� ó������ �ʰ� �����Ѵ�.
		if (this.player_me_index != this.current_player_index)
		{
			return;
		}
	}

	// �� �� ������ ������ ��
	IEnumerator on_selected_cell_to_attack(byte player_index, short from, short to)
	{
		byte distance = CHelper.howfar_from_clicked_cell(from, to);
		if (distance == 1)
		{
			// copy to cell
			yield return StartCoroutine(reproduce(to));
		}
		else if (distance == 2)
		{
			// move
			//this.board[from] = short.MaxValue;
			this.players[player_index].remove(from);
			yield return StartCoroutine(reproduce(to));
		}

		CPacket msg = CPacket.create((short)PROTOCOL.TURN_FINISHED_REQ);
		this.network_manager.send(msg);

		yield return 0;
	}
	
	void phase_end()
	{
		this.step = 0;
	}
	

	void refresh_available_cells(short cell)
	{
		
	}
	
	void clear_available_attacking_cells()
	{
		
	}
	
	IEnumerator reproduce(short cell)
	{
		CPlayer current_player = this.players[this.current_player_index];
		CPlayer other_player = this.players.Find(obj => obj.player_index != this.current_player_index);
		
		clear_available_attacking_cells();
		//yield return new WaitForSeconds(0.5f);
		
		current_player.add(cell);

		yield return new WaitForSeconds(0.5f);
		
		// eat.
		List<short> neighbors = CHelper.find_neighbor_cells(cell, other_player.cell_indexes, 1);
		foreach (short obj in neighbors)
		{
			current_player.add(obj);
			
			other_player.remove(obj);
			
			yield return new WaitForSeconds(0.2f);
		}
	}
	
	// �̰� ����?
	bool validate_begin_cell(short cell)
	{
		return this.players[this.current_player_index].cell_indexes.Exists(obj => obj == cell);
	}

	// ���⼭���� ��� �޼���
	public void ShotTarget()
    {

    }
}
