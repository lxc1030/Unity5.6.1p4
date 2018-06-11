using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card_4 : SupportInfo
{

    public override void DoTimeLogic()
    {
        base.DoTimeLogic();

        GameObject obj = Common.Generate(DataController.prefabPath_Bullet + nameof(BulletGroup_6), GameManager.instance.transBullet);
        obj.transform.position = shootPoint.position;
        BulletGroup_6 info = obj.GetComponent<BulletGroup_6>();
        info.parent = shootPoint;
        info.prefab = Common.PrefabLoad(DataController.prefabPath_Bullet + 6);//子弹类型
        info.cardInfo = cardInfo;
        info.showNum = 4;
        info.Init();

    }


}
