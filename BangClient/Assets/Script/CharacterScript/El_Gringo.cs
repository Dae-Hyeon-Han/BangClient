using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class El_Gringo : Characters
{
    void Start()
    {
        characterName = "엘 그링고";
        life = 3;
        coments = "생명력 1을 잃을 때마다 공격한 사람의 손에서 카드 한 장을 가져온다.";
    }

    public override void CharacterAbility()
    {

    }
}
