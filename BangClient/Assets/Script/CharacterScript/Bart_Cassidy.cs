using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bart_Cassidy : Characters
{
    // Start is called before the first frame update
    void Start()
    {
        characterName = "바트 캐시디";
        //life = 4;
        comments = "생명력을 잃을 때마다 카드 더미에서 카드 한 장을 가져온다.";
    }

    public override void CharacterAbility()
    {
        Debug.Log($"설명 {comments}");
    }
}
