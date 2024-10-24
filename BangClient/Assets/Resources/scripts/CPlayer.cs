using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//using BangGameServer;
//using FreeNet;

public enum PLAYER_STATE
{
	HUMAN,
	AI
}

public class CPlayer : MonoBehaviour {
	
	public List<short> cell_indexes { get; private set; }
	public byte player_index { get; private set; }
	public PLAYER_STATE state { get; private set; }
	CPlayerAgent agent;

	#region 뱅 용
	public string charName;             // 캐릭터 이름
	public string job;					// 직업
	public int cardInHand;				// 손패
	public string weapon;				// 장착중인 무기
	public List<string> Equipment;      // 술통, 야생마, 조준경
	public int positionFlag;            // 게임 중 거리 계산용으로 사용할 것.
	public int myRange;                 // 내 사거리
	public int outRange;                // 내가 멀어질 경우(캐릭터 특성 or 조랑말 효과)
	public int maxLife;					// 최대 체력
	public int extraLife;				// 현재 체력

	public Transform player;			//

	public int MyRange
    {
        get { return myRange; }
		set { myRange = value; }
    }
	#endregion

	void Awake()
	{
		this.cell_indexes = new List<short>();
		this.agent = new CPlayerAgent();
	}
	
	
	public void clear()
	{
		this.cell_indexes.Clear();
	}
	
	public void initialize(byte player_index, string charName, int life, string job)
	{
		this.player_index = player_index;
		this.charName = charName;
		this.maxLife = life;
		this.job = job;

		Debug.Log($"{gameObject.name},{this.player_index}. {charName}");
	}

    #region 추후 삭제 요망
    public void add(short cell)
	{
		if (this.cell_indexes.Contains(cell))
		{
			Debug.LogError(string.Format("Already have a cell. {0}", cell));
			return;
		}
		
		this.cell_indexes.Add(cell);
	}
	
	public void remove(short cell)
	{
		this.cell_indexes.Remove(cell);
	}
	
	public void change_to_agent()
	{
		this.state = PLAYER_STATE.AI;
	}
	
	public void change_to_human()
	{
		this.state = PLAYER_STATE.HUMAN;
	}
	
	public CellInfo run_agent(List<short> board, List<CPlayer> players, List<short> victim_cells)
	{
		return this.agent.run(board, players, this.cell_indexes, victim_cells);
	}

	public int get_virus_count()
	{
		return this.cell_indexes.Count;
	}
    #endregion

    #region 여기서부터 뱅용. 이 플랜이 맞나...
	// 새로운 턴이 되어 카드 드로우 시
	public void DrawCard()
    {
		//CPacket msg = CPacket.create((short)PROTOCOL.DRAWCARD);
    }

    // 뱅 사용
    public void UseBang()
	{ 

	}

	// 빗나감 사용
	public void UseMissed()
    {

    }

	public List<string> MyCard()
    {
		return null;
    }
    #endregion
}
