using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jourdonnais : Characters
{
    void Start()
    {
        characterName = "주르도네";
        life = 4;
        coments = "<뱅!>의 목표가 될 때마다 (카드 펼치기!)를 할 수 있으며, 하트가 나오면 총알이 빗나간다.";
    }

    public override void CharacterAbility()
    {

    }
}
