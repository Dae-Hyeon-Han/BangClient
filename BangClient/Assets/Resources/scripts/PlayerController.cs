using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerController : MonoBehaviour
{
    List<Card> handCardList = new List<Card>();
    List<Card> equipCardList = new List<Card>();

    Characters myChar;
    TextMeshProUGUI charExplaneBox;

    public Characters MyChar
    {
        get { return myChar; }
        set { myChar = value; }
    }

    public Transform player;
    public List<Transform> equips;
    Dictionary<string, Transform> handCard = new Dictionary<string, Transform>();

    void Start()
    {
        foreach (Transform cards in player)
        {
            handCard[cards.name] = cards;
            cards.gameObject.SetActive(false);              // 플레이 중 손패가 생기면 활성화 시킬 것
        }
    }

    // game room에서 처리?
    // 코멘트는 클라이언트 측에서 처리.
    public void SetMyChar(string charName, int life)
    {
        myChar.name = charName;
        myChar.life = life;
    }

    // 패 추가
    public void AddCard(string cardName, string shape, string number)
    {

    }
}
