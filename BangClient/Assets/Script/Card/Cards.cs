using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BangGameServer;
using FreeNet;

public abstract class Cards : MonoBehaviour
{
    public string cardName;
    public string funcText;
    public int cardIndex;
    public string shape;
    public string number;


    public abstract void UseCard();
    public abstract void SetCard(int index, string cardName, string shape, string number);
    public abstract void MouseIn_ViewCardFunc();
    public abstract void MouseOut_ViewCardFunc();
}
