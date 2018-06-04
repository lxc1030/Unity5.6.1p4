using Spine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boat_0 : BoatInfo
{
    /// <summary>
    /// 射击点
    /// </summary>
    public Transform shootPoint;
    /// <summary>
    /// 检测可射击半径
    /// </summary>
    public float shootRange;
    /// <summary>
    /// 平A CD时间
    /// </summary>
    public float shootCD;


    public override void state_Event(TrackEntry trackEntry, Spine.Event e)
    {
        base.state_Event(trackEntry, e);

        float angle = GameManager.BackAngleOfTarget(A_Target, shootPoint.position);

        if (e.Data.Name == "e_atk")
        {
            //打完发子弹
            if (A_Target != null)
            {
                //
                GameManager.instance.SetParticle(PreLoadType.ShootParticle, shootPoint.position, true);
                //CardInfo.SetBullet(Tag.Boat, (int)myType, cardInfo.Atk, shootPoint.position, angle);


                GameObject obj = Common.Generate(DataController.prefabPath_Bullet + nameof(BulletGroup_1), GameManager.instance.transBullet);
                obj.transform.position = shootPoint.position;
                BulletGroup_1 info = obj.GetComponent<BulletGroup_1>();
                info.parent = shootPoint;
                info.prefab = Common.PrefabLoad(DataController.prefabPath_Bullet + 4);//子弹类型
                //info.startAngle = angle;
                info.startAngle = 180;
                info.cardInfo = cardInfo;
                info.Init();
            }
        }
    }








    public override void DoAIByType()
    {
        CheckToShoot();
    }

    #region AI

    private void CheckToShoot()
    {
        if (cardInfo.Hp > 0)
        {
            GameManager.instance.ShootController(this, transform.position, shootRange);
            Invoke("CheckToShoot", shootCD);
        }
    }



    #endregion
}
