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

	#region �� ��
	public string charName;             // ĳ���� �̸�
	public string job;					// ����
	public int cardInHand;				// ����
	public string weapon;				// �������� ����
	public List<string> Equipment;      // ����, �߻���, ���ذ�
	public int positionFlag;            // ���� �� �Ÿ� �������� ����� ��.
	public int myRange;                 // �� ��Ÿ�
	public int outRange;                // ���� �־��� ���(ĳ���� Ư�� or ������ ȿ��)
	public int maxLife;					// �ִ� ü��
	public int extraLife;				// ���� ü��

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

    #region ���� ���� ���
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

    #region ���⼭���� ���. �� �÷��� �³�...
	// ���ο� ���� �Ǿ� ī�� ��ο� ��
	public void DrawCard()
    {
		//CPacket msg = CPacket.create((short)PROTOCOL.DRAWCARD);
    }

    // �� ���
    public void UseBang()
	{ 

	}

	// ������ ���
	public void UseMissed()
    {

    }

	public List<string> MyCard()
    {
		return null;
    }
    #endregion
}
