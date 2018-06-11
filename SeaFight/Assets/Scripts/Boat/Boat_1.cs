using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boat_1 : BoatInfo {

    private bool isHit;

    public override void Init(float sp)
    {
        base.Init(sp);
        isHit = false;

    }

    public void OnTriggerEnter(Collider other)
    {
        if (isHit)
            return;


        string info = "";
        GameObject obj = other.gameObject;
        switch (obj.tag)
        {
            case nameof(Tag.Player):
                isHit = true;
                info = "船只撞击->" + other.gameObject.name;
                CharacterInfo cInfo = obj.GetComponent<CharacterInfo>();
                cInfo.BeShoot(cardInfo.Atk);
                break;
            case nameof(Tag.Support):
                isHit = true;
                info = "船只撞击->" + other.gameObject.name;
                SupportInfo sInfo = obj.GetComponent<SupportInfo>();
                sInfo.BeShoot(cardInfo.Atk);
                break;
        }
        if (info != "")
        {
            Debug.LogError(info);
        }
        if (isHit)
        {
            PoolDestroy();
        }
    }
    public override void DoAIByType()
    {
        
    }

}
