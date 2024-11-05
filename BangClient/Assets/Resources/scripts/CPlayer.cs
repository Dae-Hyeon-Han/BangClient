using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using System;

//using BangGameServer;
//using FreeNet;

public enum PLAYER_STATE
{
    HUMAN,
    AI
}

public class CPlayer : MonoBehaviour
{
    public byte player_index { get; private set; }
    public PLAYER_STATE state { get; private set; }
    CPlayerAgent agent;

    #region 뱅 용
    public string charName;             // 캐릭터 이름
    public string job;                  // 직업
    public int cardInHand;              // 손패
    public string weapon;               // 장착중인 무기
    public List<string> Equipment;      // 술통, 야생마, 조준경
    public int positionFlag;            // 게임 중 거리 계산용으로 사용할 것.
    public int myRange;                 // 내 사거리
    public int outRange;                // 내가 멀어질 경우(캐릭터 특성 or 조랑말 효과)
    public int maxLife;                 // 최대 체력
    public int extraLife;               // 현재 체력

    public Transform playerGroup;            //

    Dictionary<string, Transform> playerIndex = new Dictionary<string, Transform>();        // 숫자 출력용
    //Dictionary<string, Image> playerCharImage = new Dictionary<string, Image>();            // 그림 출력용
    TextMeshProUGUI myId;
    Image playerCharImage;
    Image jobImage;
    TextMeshProUGUI life;
    //Transform handsCard;
    Transform equips;
    Characters myCharacter;

    // 손에 든 카드
    List<Transform> handCardPool = new List<Transform>();
    Image card;

    // ui interaction
    TextMeshProUGUI explaneBox;

    public int MyRange
    {
        get { return myRange; }
        set { myRange = value; }
    }
    #endregion

    void Awake()
    {
        this.agent = new CPlayerAgent();
    }

    private void Start()
    {
        playerGroup = GameObject.Find("Players").transform;

        //foreach (Transform players in playerGroup)
        //{
        //    //playerCharImage[players.name] = players.GetChild(1).GetComponent<Image>();
        //    playerIndex[players.name] = players.GetChild(0);
        //}

        myId = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        playerCharImage = transform.GetChild(1).GetComponent<Image>();
        jobImage = transform.GetChild(2).GetComponent<Image>();
        life = transform.GetChild(3).GetComponent<TextMeshProUGUI>();
        //handsCard = transform.GetChild(4).GetComponent<Transform>();
        //equips = transform.GetChild(5).GetComponent<Transform>();


        //card = transform.GetChild(4).GetChild(0).GetComponent<Image>();
        //Debug.Log($"{card.transform.parent.name}, {gameObject.transform.name}");

        //Debug.Log($"{transform.GetChild(4).name}");

        foreach (Transform card in transform.GetChild(4))
        {
            card.gameObject.SetActive(false);
            handCardPool.Add(card);
        }

        // 카드 설명용
        explaneBox = GameObject.Find("ExplaneText").GetComponent<TextMeshProUGUI>();
    }

    // 이곳을 기준으로 플레이어 별 직업 및 캐릭터 별 옵션 셋팅은 완료하고, 플레이어에 대한 정보를 정리할 것.
    public void initialize(byte player_me_index, byte player_index, string charName, string job, int life)
    {
        this.player_index = player_index;
        this.charName = charName;
        this.job = job;
        this.maxLife = life;

        myId.text = this.player_index + "번 플레이어";
        playerCharImage.sprite = Resources.Load<Sprite>("Images/Char/Char_" + charName);
        jobImage.sprite = Resources.Load<Sprite>("Images/Job/" + job);
        this.life.text = "hp: " + this.maxLife;
        //handsCard

        //Debug.Log($"목록: {player_me_index}, {player_index}, {charName}, {job}, {life}");
        //AddCharacterComponent();

        for(int i=0; i<4; i++)
            PlusCard();
    }

    public void AddCharacterComponent()
    {
        //Type type = Type.GetType(charName);
        //Characters instance = Activator.CreateInstance(type) as Characters;
        ////Debug.Log($"캐릭터: {charName},{instance.characterName}");
        //instance.CharacterAbility();

        //gameObject.AddComponent<Characters>();
    }

    #region 추후 삭제 요망
    public void change_to_agent()
    {
        this.state = PLAYER_STATE.AI;
    }

    public void change_to_human()
    {
        this.state = PLAYER_STATE.HUMAN;
    }

    #endregion

    #region 여기서부터 뱅용. 이 플랜이 맞나...
    // 새로운 턴이 되어 카드 드로우 시
    public void PlusCard()
    {
        // 플레이어는 컨트롤러에서 따로 관리할 것
        if (gameObject.name == "player0")
            return;
        else
        {
            for(int i=0;i<4;i++)
            {
                handCardPool[i].gameObject.SetActive(true);
            }
        }

        //handCardPool.Dequeue().SetActive(true);
    }

    public void MinusCard()
    {
        //handCardPool.Enqueue().SetActive(false);
    }

    public void CharacterExplaine()
    {

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
