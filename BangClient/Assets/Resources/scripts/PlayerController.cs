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
            cards.gameObject.SetActive(false);              // �÷��� �� ���а� ����� Ȱ��ȭ ��ų ��
        }
    }

    // game room���� ó��?
    // �ڸ�Ʈ�� Ŭ���̾�Ʈ ������ ó��.
    public void SetMyChar(string charName, int life)
    {
        myChar.name = charName;
        myChar.life = life;
    }

    // �� �߰�
    public void AddCard(string cardName, string shape, string number)
    {

    }
}
