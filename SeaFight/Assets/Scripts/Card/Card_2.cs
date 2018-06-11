using Spine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card_2 : CharacterInfo
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
                GameObject obj = Common.Generate(DataController.prefabPath_Bullet + nameof(BulletGroup_4), GameManager.instance.transBullet);
                obj.transform.position = new Vector3(shootPoint.position.x, 0, shootPoint.position.z);
                BulletGroup_4 info = obj.GetComponent<BulletGroup_4>();
                info.parent = shootPoint;
                info.target = A_Target;
                info.prefab = Common.PrefabLoad(DataController.prefabPath_Bullet + myIndex);//子弹类型
                info.startAngle = angle;
                info.cardInfo = cardInfo;
                info.Init();
            }
            else
            {
                if (A_Target == null || !A_Target.isLive)
                {
                    return;
                }
                bulletGroupCount++;
                //
                GameManager.instance.SetParticle(PreLoadType.ShootParticle, shootPoint.position, true);
                Tag tag = isEnemy ? Tag.Enemy : Tag.Player;
                Vector3 moDir = (A_Target.transform.position - shootPoint.position).normalized * 20;
                CardInfo.SetBullet(tag, myIndex, myIndex, cardInfo.Atk, shootPoint.position, angle, moDir);
            }
        }

    }


}
