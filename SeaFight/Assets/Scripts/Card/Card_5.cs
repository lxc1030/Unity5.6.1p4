using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card_5 : SupportInfo
{

    public override void DoTimeLogic()
    {
        base.DoTimeLogic();
        Debug.LogError("迫击炮特效");

        //GameManager.instance.DoCameraAnimation();
    }
}
