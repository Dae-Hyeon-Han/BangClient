using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Black_Jack : Characters
{
    void Start()
    {
        characterName = "블랙 잭";
        //life = 4;
        coments = "(카드 가져오기!) 단계에서 가져온 두 번째 카드를 보여준다. 그 카드가 하트나 다이아몬드라면 한장 더 가져온다.";
    }

    public override void CharacterAbility()
    {

    }
}
