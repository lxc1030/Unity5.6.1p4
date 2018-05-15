using DG.Tweening;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatInfo : MonoBehaviour
{
    public Transform AnimationObj;
    private SkeletonAnimation skeletonAnimation;   //gameobject的component。
    public SkeletonAnimation skAnimation
    {
        get { return skeletonAnimation; }
        set { skeletonAnimation = value; }
    }
    private Renderer render;
    Spine.AnimationState spineAnimationState;

    public Transform transPart;
    public BoatType myType;
    public float speed;

    private BoxCollider myCollider;

    public CardInfo cardInfo;

    private void Awake()
    {
        myCollider = GetComponent<BoxCollider>();

        if (AnimationObj != null)
        {
            skeletonAnimation = AnimationObj.GetComponent<SkeletonAnimation>();
            render = AnimationObj.GetComponent<Renderer>();
        }
        cardInfo = GetComponent<CardInfo>();
    }

    public void Init(BoatType type, Vector3 pos, float sp)
    {
        myType = type;
        transform.localPosition = pos;
        speed = sp;

        spineAnimationState = skeletonAnimation.state;
        //初始动画
        string animationName = "chuan" + (int)(myType + 1);
        spineAnimationState.SetAnimation(0, animationName, true);


        switch (myType)
        {
            case BoatType.NoBullet:
                transPart.localPosition = new Vector3(0.7f, 0.348f, 0f);
                myCollider.size = new Vector3(1.57f, 1.22f, 1f);
                break;
            case BoatType.NormalBullet:
                transPart.localPosition = new Vector3(1.13f, 0.348f, 0f);
                myCollider.size = new Vector3(2.41f, 1.22f, 1f);
                break;
        }

        cardInfo.SetInit(true, GameManager.BackBoatHp(myType), GameManager.BackBoatAtk(myType), "名称->" + myType);

    }
    public void BeShoot(float demage)
    {
        if (cardInfo.Hp > 0)
        {
            cardInfo.SetHp(demage);
            render.material.DOFloat(1f, "_FillPhase", 0.1f).OnComplete(HitAnimationComplete);
            if (cardInfo.Hp <= 0)
            {
                PoolDestroy();
            }
        }

    }

    private void HitAnimationComplete()
    {
        render.material.SetFloat("_FillPhase", 0);
    }

    private void PoolDestroy()
    {
        PoolManager.instance.SetPoolObjByType(PreLoadType.Boat, gameObject);
        PoolManager.instance.SetPoolObjByType(PreLoadType.PeopleInfo, cardInfo.pInfo.gameObject);
    }



    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.left * speed * Time.deltaTime);
        GameManager.SetRenderQueueByType(render, transform.position.y);

        //if (skeletonUtility != null)
        //{
        //    skeletonUtility.skeletonRenderer.meshRenderer.sharedMaterial.renderQueue = 3500 - (int)(Math.Round(transform.localPosition.y, 2) * 100);
        //}
    }
    public void LateUpdate()
    {
        GameManager.SetZPosition(transform);
    }
}
