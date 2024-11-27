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
    [Header("Player Information")]
    public string charName;             // 캐릭터 이름
    public string job;                  // 직업
    //public int cardInHand;              // 손패
    [SerializeField] int cardCount;         // 손패 수
    //public string weapon;               // 장착중인 무기
    public List<string> Equipment;      // 술통, 야생마, 조준경
    public int positionFlag;            // 게임 중 거리 계산용으로 사용할 것.
    public int maxLife;                 // 최대 체력
    public int extraLife;               // 현재 체력
    public int range;                 // 내 사거리
    public int depth;                // 내가 멀어질 경우(캐릭터 특성 or 조랑말 효과)
    [SerializeField]private string gun;                  // 총
    [SerializeField]private string mirono;               // 조준경
    [SerializeField]private string mustang;              // 야생마
    [SerializeField]private string barile;               // 술통
    [SerializeField] string prigione;
    [SerializeField] string dinamite;
    [SerializeField] Transform equips;
    [SerializeField] Image gunImage;
    [SerializeField] List<Transform> equip;             // 총은 이미지 변경하고, 나머지는 on/off

    #region 손패 수 정리
    public int CardCount
    {
        get { return cardCount; }
        set 
        {
            cardCount = value;

            if (gameObject.name == "player0")
                return;
            else
            {
                for (int i = 0; i < cardCount; i++)
                {
                    handCardPool[i].gameObject.SetActive(true);
                }
            }
        }
    }
    #endregion

    #region 장비
    public string Gun
    {
        get { return gun; }
        set
        {
            gun = value;
            Debug.Log($"총: {gun}");

            gunImage.sprite = Resources.Load<Sprite>("Images/CardImage/" + gun);
            //equip[0].gameObject.SetActive(true);
        }
    }

    public string Mirono
    {
        get { return mirono; }
        set
        {
            mirono = value;

            // 대소문자 주의
            if (mirono == "true")
                equip[1].gameObject.SetActive(true);
            else if (mirono == "false")
                equip[1].gameObject.SetActive(false);
        }
    }

    public string Mustang
    {
        get { return mustang; }
        set
        {
            mustang = value;

            if (mirono == "true")
                equip[2].gameObject.SetActive(true);
            else if (mirono == "false")
                equip[2].gameObject.SetActive(false);
        }
    }

    public string Barile
    {
        get { return barile; }
        set
        {
            barile = value;

            if (mirono == "true")
                equip[3].gameObject.SetActive(true);
            else if (mirono == "false")
                equip[3].gameObject.SetActive(false);
        }
    }

    public string Prigione
    {
        get { return prigione; }
        set { prigione = value; }
    }

    public string Dinamite
    {
        get { return dinamite; }
        set { dinamite = value; }
    }

    #endregion

    [Header("Controller")]
    public PlayerController controller;

    [SerializeField] int indexNum;      // 인스펙터 확인용. 쓰진 않음

    public Transform playerGroup;            //

    Dictionary<string, Transform> playerIndex = new Dictionary<string, Transform>();        // 숫자 출력용
    //Dictionary<string, Image> playerCharImage = new Dictionary<string, Image>();            // 그림 출력용
    TextMeshProUGUI myId;
    Image playerCharImage;
    Image jobImage;
    TextMeshProUGUI life;
    //Transform handsCard;
    //Transform equips;
    Characters myCharacter;

    // 손에 든 카드
    List<Transform> handCardPool = new List<Transform>();
    Image card;

    // ui interaction
    TextMeshProUGUI explaneBox;

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

        #region 손패
        #endregion

        #region 장비
        equips = transform.GetChild(5);

        foreach(Transform equipment in equips)
        {
            equip.Add(equipment);
        }
        gunImage = equip[0].GetComponent<Image>();
        #endregion


        // 카드 설명용
        //explaneBox = GameObject.Find("ExplaneText").GetComponent<TextMeshProUGUI>();

        // 이벤트용
        controller = GameObject.Find("PlayerController").GetComponent<PlayerController>();
    }

    // 이곳을 기준으로 플레이어 별 직업 및 캐릭터 별 옵션 셋팅은 완료하고, 플레이어에 대한 정보를 정리할 것.
    public void initialize(byte player_me_index, byte player_index, string charName, string job, int life, int range, int depth)
    {
        this.player_index = player_index;
        this.charName = charName;
        this.job = job;
        this.maxLife = life;
        this.range = range;
        this.depth = depth;


        // 인스펙터 확인용
        indexNum = this.player_index;

        myId.text = this.player_index + "번 플레이어";
        playerCharImage.sprite = Resources.Load<Sprite>("Images/Char/Char_" + charName);
        jobImage.sprite = Resources.Load<Sprite>("Images/Job/" + job);
        this.life.text = "hp: " + this.maxLife;
        //handsCard

        //Debug.Log($"목록: {player_me_index}, {player_index}, {charName}, {job}, {life}");
        //AddCharacterComponent();

        for(int i=0; i<4; i++)
            PlusCard();

        // 카드 선택 이벤트 등록
        if(gameObject.name != "player0")
            gameObject.GetComponent<Button>().onClick.AddListener(SetTarget);
        //Debug.Log($"{player_index}");
    }

    public void RefreshInfo(byte player_index, int life, int cardCount, int range, int depth, string gun, string mirono, string mustang, string barile, string prigione, string dinamite)
    {
        this.player_index = player_index;
        this.maxLife = life;
        CardCount = cardCount;
        this.range = range;
        this.depth = depth;
        Gun = gun;
        Mirono = mirono;
        Mustang = mustang;
        Barile = barile;
        Prigione = prigione;
        Dinamite = dinamite;
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

    public void CharacterExplaine()
    {

    }

    public void SetTarget()
    {
        controller.Target = this.player_index;
        controller.targetCheck.text = $"타깃: {this.player_index}번 플레이어";
        Debug.Log($"타깃 인덱스: {controller.Target}");
    }
    #endregion
}
