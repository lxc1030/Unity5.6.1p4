using Spine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card_0 : CharacterInfo
{
    private int bulletGroupCount;
    public int bulletGroupCountMax;

    public override void state_Event(TrackEntry trackEntry, Spine.Event e)
    {
        base.state_Event(trackEntry, e);
        //
        float angle = GameManager.BackAngleOfTarget(A_Target, shootPoint.position);

        //
        if (e.Data.Name == "e_atk")
        {
            if (bulletGroupCount >= bulletGroupCountMax)
            {
                bulletGroupCount = 0;
                //
                GameObject obj = Common.Generate(DataController.prefabPath_Bullet + nameof(BulletGroup_2), GameManager.instance.transBullet);
                obj.transform.position = shootPoint.position;
                BulletGroup_2 info = obj.GetComponent<BulletGroup_2>();
                info.parent = shootPoint;
                info.prefab = Common.PrefabLoad(DataController.prefabPath_Bullet + 3);//子弹类型
                info.startAngle = angle;
                info.cardInfo = cardInfo;
                info.Init();
            }
            else
            {
                bulletGroupCount++;
                //
                GameManager.instance.SetParticle(PreLoadType.ShootParticle, shootPoint.position, true);
                Tag tag = isEnemy ? Tag.Enemy : Tag.Player;
                CardInfo.SetBullet(tag, myIndex, myIndex, cardInfo.Atk, shootPoint.position, angle);
            }
        }
    }




}
