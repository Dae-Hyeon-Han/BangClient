using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Characters : MonoBehaviour
{
    public string characterName;        // 캐릭터 이름
    public int life;                    // 기본 목숨. 서버에서 수신할 것(추가 예정)
    public string coments;              // 캐릭터에 대한 설명

    public abstract void CharacterAbility();
}