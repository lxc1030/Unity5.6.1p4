using DG.Tweening;
using Spine;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatInfo : MonoBehaviour
{
    /// <summary>
    /// 船的类型
    /// </summary>
    public BoatType myType;

    public CardInfo A_Target;

    public Transform AnimationObj;
    private SkeletonAnimation skeletonAnimation;   //gameobject的component。
    public SkeletonAnimation skAnimation
    {
        get { return skeletonAnimation; }
        set { skeletonAnimation = value; }
    }
    private Renderer render;
    Spine.AnimationState spineAnimationState;
    private bool isEvent = false;

    /// <summary>
    /// 船只移动速度
    /// </summary>
    private float moveSpeed;
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

    public virtual void Init(float sp)
    {
        moveSpeed = sp;
        if (!isEvent)
        {
            isEvent = true;
            if (skeletonAnimation != null)
            {
                spineAnimationState = skeletonAnimation.state;
                skeletonAnimation.state.Event += state_Event;
                skeletonAnimation.state.Complete += AnimationComplete;
            }
        }

        spineAnimationState = skeletonAnimation.state;
        //初始动画
        string animationName = "idle";
        spineAnimationState.SetAnimation(0, animationName, true);
        
        DoAIByType();
    }
    public virtual void AnimationComplete(TrackEntry trackEntry)
    {
        string curAni = trackEntry.ToString();
        string animationName = "idle";
        if (curAni != animationName)
        {
            spineAnimationState.SetAnimation(0, animationName, true);
        }
    }

    public virtual void state_Event(TrackEntry trackEntry, Spine.Event e)
    {

    }


    public virtual void DoAIByType()
    {

    }

    public virtual void Shoot(CardInfo target)
    {
        A_Target = target;

        switch (target.tag)
        {
            case nameof(Tag.Player):
                string animationName = "atk";
                spineAnimationState.SetAnimation(0, animationName, false);
                break;
        }
    }


    public void BeShoot(float demage)
    {
        if (cardInfo.isLive)
        {
            cardInfo.UpdateHp(-demage);
            render.material.DOFloat(1f, "_FillPhase", 0.1f).OnComplete(HitAnimationComplete);
            if (!cardInfo.isLive)
            {
                PoolDestroy();
            }
        }

    }
    private void HitAnimationComplete()
    {
        render.material.SetFloat("_FillPhase", 0);
    }

    public virtual void PoolDestroy()
    {
        GameManager.instance.SetParticle(PreLoadType.BoatDeadParticle, transform.position, true);
        PoolManager.instance.SetPoolObjByType(PreLoadType.PeopleInfo, cardInfo.pInfo.gameObject);
        GameManager.instance.DeleteBoat(cardInfo);
    }


    // Update is called once per frame
    void Update()
    {
        if (transform.position.x >= DataController.instance.supportPosX)
        {
            transform.Translate(Vector3.left * moveSpeed * Time.deltaTime);
        }
        else
        {
            PoolDestroy();
        }
        GameManager.SetRenderQueueByType(render, transform.position.y);

        //if (skeletonUtility != null)
        //{
        //    skeletonUtility.skeletonRenderer.meshRenderer.sharedMaterial.renderQueue = 3500 - (int)(Math.Round(transform.localPosition.y, 2) * 100);
        //}
    }
   
}
