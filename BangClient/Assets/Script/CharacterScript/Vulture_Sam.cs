using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vulture_Sam : Characters
{
    void Start()
    {
        characterName = "벌처 샘";
        life = 4;
        coments = "게임에서 제거되는 인물이 생길 때마다, 그 사람의 모든 카드를 손으로 가져온다.";
    }

    public override void CharacterAbility()
    {

    }
}
