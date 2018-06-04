using Spine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card_3 : CharacterInfo
{
    private int bulletGroupCount;
    public int bulletGroupCountMax;

    private float addAngle = 5;
    public override void state_Event(TrackEntry trackEntry, Spine.Event e)
    {
        base.state_Event(trackEntry, e);
        //
        float angle = GameManager.BackAngleOfTarget(A_Target, shootPoint.position);
        //
        float addAll = addAngle * (bulletNum - 1);
        float startAngel = angle - (addAll / 2);
        //
        if (e.Data.Name == "e_atk")
        {
            if (bulletGroupCount >= bulletGroupCountMax)
            {
                bulletGroupCount = 0;
                //
                GameObject obj = Common.Generate(DataController.prefabPath_Bullet + nameof(BulletGroup_5), GameManager.instance.transBullet);
                obj.transform.position = shootPoint.position;
                BulletGroup_5 info = obj.GetComponent<BulletGroup_5>();
                info.parent = shootPoint;
                info.prefab = Common.PrefabLoad(DataController.prefabPath_Bullet + myIndex);//子弹类型
                info.startAngle = angle;
                info.cardInfo = cardInfo;
                info.Init();
            }
            else
            {
                bulletGroupCount++;
                //
                GameManager.instance.SetParticle(PreLoadType.ShootParticle, shootPoint.position, true);
                for (int i = 0; i < bulletNum; i++)
                {
                    Tag tag = isEnemy ? Tag.Enemy : Tag.Player;
                    CardInfo.SetBullet(tag, myIndex, myIndex, cardInfo.Atk, shootPoint.position, angle);
                }
            }
        }


    }


}
