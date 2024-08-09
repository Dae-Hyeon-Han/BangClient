using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Characters : MonoBehaviour
{
    public string characterName;        // 캐릭터 이름
    public int life;                    // 기본 목숨
    public string coments;              // 캐릭터에 대한 설명

    public abstract void CharacterAbility();
}

public enum Job
{
    SHERIFF = 0,
    VICE = 1,
    OUTLAW = 2,
    RENEGADE = 3
}

public enum Charater
{
    Willy_The_Kid = 0,
    Clamity_Janet = 1,
    Kit_Carlson = 2,
    Bart_Cassidy = 3,
    Sid_Ketchum = 4,
    Lucky_Duke = 5,
    Jourdonnais = 6,
    Black_Jack = 7,
    Vulture_Sam = 8,
    Jesse_Jones = 9,
    Suzy_Lafayette = 10,
    Pedro_Ramirez = 11,
    Slab_The_Killer = 12,
    Rose_Doolan = 13,
    Paul_Regret = 14,
    El_Gringo = 15
}