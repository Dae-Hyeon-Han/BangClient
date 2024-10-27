using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BangCharacter : MonoBehaviour
{
    Dictionary<string, string> charDictionary = new Dictionary<string, string>();

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
    }
}
