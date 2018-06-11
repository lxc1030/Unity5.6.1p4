using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card_6 : SupportInfo
{
    public Vector2 addHp;
    public override void DoTimeLogic()
    {
        base.DoTimeLogic();

        Debug.LogError("加血");
        for (int i = 0; i < GameManager.instance.myCards.Count; i++)
        {
            CardInfo cInfo = GameManager.instance.myCards[i].cardInfo;
            if (!cInfo.isLive)
            {
                continue;
            }
            float add = Random.Range(addHp.x, addHp.y);
            cInfo.UpdateHp(add);
        }
    }
}
