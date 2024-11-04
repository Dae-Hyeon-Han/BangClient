using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BangCharacter : MonoBehaviour
{
    Dictionary<string, string> charDictionary = new Dictionary<string, string>();
    Dictionary<string, string> playingCardDictionary = new Dictionary<string, string>();

    // Start is called before the first frame update
    void Start()
    {
        charDictionary["Willy_The_Kid"] = "<뱅!>을 원하는 만큼 사용할 수 있습니다.";
        charDictionary["Clamity_Janet"] = "<뱅!>을 <빗나감!>으로 사용할 수 있고, <빗나감!>을 <뱅!>으로 사용할 수 있습니다.";
        charDictionary["Kit_Carlson"] = "[카드 가져오기!] 단계에서 카드 더미 맨 위의 세 장을 보고 가져갈 두 장을 고릅니다.";
        charDictionary["Bart_Cassidy"] = "생명력 1을 잃을 때마다 카드 더미에서 카드 한 장을 가져옵니다.";
        charDictionary["Sid_Ketchum"] = "카드 두 장을 버려 생명력 1을 회복할 수 있습니다.";

        charDictionary["Lucky_Duke"] = "자신이 [카드 펼치기!]를 할 때마다, 두 장을 펼치고 그 중 한 장을 선택합니다.";
        charDictionary["Jourdonnais"] = "<뱅!>의 표적이 될 때마다 [카드 펼치기!]를 할 수 있으며, 하트가 나오면 총알이 빗나갑니다.";
        charDictionary["Black_Jack"] = "[카드 가져오기!] 단계에서 가져온 두 번째 카드를 보여줍니다. 그 카드가 하트나 다이아몬드라면 한 장 더 가져옵니다.";
        charDictionary["Vulture_Sam"] = "게임에서 제거되는 사람이 생길 때마다, 그 사람의 모든 카드를 가져와 손에 듭니다.";
        charDictionary["Jesse_Jones"] = "[카드 가져오기!] 단계에서 첫 번재 카드를 다른 사람의 손에서 가져올 수 있습니다.";

        charDictionary["Suzy_Lafayette"] = "손에 남은 카드가 한 장도 없다면 즉시 카드 더미에서 카드 한장을 가져옵니다.";
        charDictionary["Pedro_Ramirez"] = "[카드 가져오기!] 단계에서 첫 번재 카드를 버려진 카드 더미에서 가져올 수도 있습니다.";
        charDictionary["Slab_The_Killer"] = "<빗나감!> 두 장으로 막도록 <뱅!> 카드를 사용합니다.";
        charDictionary["Rose_Doolan"] = "다른 사람을 볼 때 거리 1이 가까워집니다.";
        charDictionary["Paul_Regret"] = "다른 사람이 볼 때 거리 1이 멀어집니다.";

        charDictionary["El_Gringo"] = "생명력 1을 잃을 때마다 공격한 사람의 손에서 카드 한 장을 가져옵니다.";

        //////////////////////////////////////////////////////////////////////////////////////

        playingCardDictionary["BANG"] = "[뱅!]/사거리 내의 한 사람을 공격한다.";
        playingCardDictionary["MANCATO"] = "[빗나감!]/상대가 <뱅!> 카드나 <기관총> 카드 등 빗나감으로 막을 수 있는 공격을 했을 때 회피한다.";
        playingCardDictionary["BIRRA"] = "[맥주]/자신의 생명력을 1 회복한다. 자신이 사망하는 순간에 맥주 카드를 보유하고 있다면 사용하여 생존할 수 있다.";
        playingCardDictionary["GATLING"] = "[기관총]/자신을 제외한 모든 플레이어를 공격한다.";
        playingCardDictionary["DUELLO"] = "[결투]/거리에 상관없이 아무나 한 사람을 지목한다. 그 플레이어부터 시작해 <뱅!> 카드를 번갈아 버리기 시작하여 먼저 <뱅!>을 버릴 수 없게 된 사람이 생명력을 1 잃는다.";
        playingCardDictionary["INDIANI"] = "[인디언!]/자신을 제외한 모든 플레이어는 <뱅!> 카드 1장을 버리거나 생명력을 1 잃는다.";
        playingCardDictionary["SALOON"] = "[주점]/모든 플레이어의 생명력을 1 회복한다.";
        playingCardDictionary["PANICO"] = "[강탈!]/거리 1의 플레이어에게서 카드 1장을 가져온다.";
        playingCardDictionary["CAT BALOU"] = "[캣 벌로우]/거리에 상관없이 아무 한 플레이어의 카드 한 장을 버리게 한다.";
        playingCardDictionary["EMPORIO"] = "[잡화점]/플레이어 수만큼 카드를 펼쳐 이 카드를 사용한 플레이어부터 시계방향으로 카드를 1장씩 가져간다.";
        playingCardDictionary["DILIGENZA"] = "[역마차]/카드 더미에서 카드 2장을 가져온다.";
        playingCardDictionary["WELLS FARGO"] = "[웰스 파고 은행] 카드 더미에서 카드 3장을 가져온다.";
        playingCardDictionary["SCHOFIELD"] = "[스코필드]/(2)";
        playingCardDictionary["REMINGTON"] = "[레밍턴]/(3)";
        playingCardDictionary["CARABINE"] = "[카빈]/(4)";
        playingCardDictionary["WINCHESTER"] = "[윈체스터]/(5)";
        playingCardDictionary["VOLCANIC"] = "[볼캐닉]/<뱅!>을 원하는만큼 사용할 수 있다.\n(1)";
        playingCardDictionary["MIRONO"] = "[조준경]/다른 사람을 보는 거리가 1 가까워진다.";
        playingCardDictionary["MUSTANG"] = "[야생마]/다른 사람이 볼 때 거리가 1 멀어진다.";
        playingCardDictionary["BARILE"] = "[술통]/<뱅!>의 목표가 될 때마다, '카드 펼치기!'를 할 수 있다. 하트가 나온다면 총알이 빗나간다.";
        playingCardDictionary["PRIGIONE"] = "[감옥]/보안관을 제외한 아무 플레이어에게 사용한다. 자기 차례를 시작할 때, 이 카드를 펼쳐 이 카드를 버리고 차례를 정상적으로 진행할지 차례를 건너 뛸지 결정한다.";
        playingCardDictionary["DINAMITE"] = "[다이너마이트]/자기 차례를 시작할 때, 카드를 펼쳐 생명력 3을 잃을 것인지 이 카드를 왼쪽 플레이어에게 넘길 것일지 결정한다.";
        playingCardDictionary["COLT45"] = "[콜트 45]/(1)";
    }
}
