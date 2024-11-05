using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseCard : MonoBehaviour
{
    public string cardName;
    public string shape;
    public string number;

    public void CardEvent()
    {
        if(cardName == "BANG")              // 뱅!
        {
            Debug.Log("뱅 사용");
        }
        else if(cardName == "MANCATO")      // 빗나감!
        {
            Debug.Log("빗나감 사용");
        }
        else if (cardName == "BIRRA")       // 맥주!
        {
            Debug.Log("맥주 사용");
        }
        else if (cardName == "GATLING")     // 기관총!
        {
            Debug.Log("기관총 사용");
        }
        else if (cardName == "DUELLO")      // 결투!
        {
            Debug.Log("결투 사용");
        }
        else if (cardName == "INDIANI")     // 인디언!
        {
            Debug.Log("인디언 사용");
        }
        else if (cardName == "SALOON")     // 주점!
        {
            Debug.Log("주점 사용");
        }
        else if (cardName == "PANICO")     // 강탈!
        {
            Debug.Log("강탈 사용");
        }
        else if (cardName == "CAT BALOU")     // 캣 벌로우!
        {
            Debug.Log("캣 벌로우 사용");
        }
        else if (cardName == "EMPORIO")     // 잡화점!
        {
            Debug.Log("잡화점 사용");
        }
        else if (cardName == "DILIGENZA")     // 역마차!
        {
            Debug.Log("역마차 사용");
        }
        else if (cardName == "WELLS FARGO")     // 웰스파고 은행
        {
            Debug.Log("웰스파고 은행 사용");
        }
        else if (cardName == "SCHOFIELD")     // 스코필드(여기서부터 장착 카드)
        {
            Debug.Log("스코필드 사용");
        }
        else if (cardName == "REMINGTON")     // 레밍턴
        {
            Debug.Log("레밍턴 사용");
        }
        else if (cardName == "CARABINE")     // 카빈
        {
            Debug.Log("카빈 사용");
        }
        else if (cardName == "WINCHESTER")     // 윈체스터
        {
            Debug.Log("윈체스터 사용");
        }
        else if (cardName == "VOLCANIC")     // 볼캐닉
        {
            Debug.Log("볼캐닉 사용");
        }
        else if (cardName == "MIRONO")     // 조준경
        {
            Debug.Log("조준경 사용");
        }
        else if (cardName == "MUSTANG")     // 야생마
        {
            Debug.Log("야생마 사용");
        }
        else if (cardName == "BARILE")     // 술통
        {
            Debug.Log("술통 사용");
        }
        else if (cardName == "PRIGIONE")     // 감옥
        {
            Debug.Log("감옥 사용");

        }
        else if (cardName == "DINAMITE")     // 다이너마이트
        {
            Debug.Log("다이너마이트 사용");
        }
    }
}
