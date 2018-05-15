using Spine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card_2 : CharacterInfo
{
    private Vector3 addVector = new Vector3(0, 0.25f, 0);
    public override void state_Event(TrackEntry trackEntry, Spine.Event e)
    {
        base.state_Event(trackEntry, e);
        if (e.Data.Name == "e_atk")
        {
            //打完发子弹
            if (A_Target != null)
            { //
                Vector3 point = A_Target.transform.position;
                float angle = Mathf.Atan2(point.y - shootPoint.position.y, point.x - shootPoint.position.x);
                angle = 180 / Mathf.PI * angle;

                GameManager.instance.SetParticle(PreLoadType.ShootParticle, shootPoint.position, true);
                for (int i = 0; i < bulletNum; i++)
                {
                    SetBullet(shootPoint.position + i * addVector, angle);
                }
            }
        }
    }

    // Use this for initialization
    void Start()
    {

    }

}
