using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sid_Ketchum : Characters
{
    // Start is called before the first frame update
    void Start()
    {
        characterName = "시드 케첨";
        life = 4;
        coments = "카드 2장을 버려 생명력 1을 회복 할 수 있다.";
    }

    public override void CharacterAbility()
    {

    }
}
