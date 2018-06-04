using Spine;
using Spine.Unity;
using UnityEngine;

public class SupportInfo : MonoBehaviour
{
    public SupportType myType;
    /// <summary>
    /// 人物编号
    /// </summary>
    public int myIndex;

    public Transform shootPoint;

    public Transform AnimationObj;

    private SkeletonAnimation skeletonAnimation;   //gameobject的component。
    public SkeletonAnimation skAnimation
    {
        get { return skeletonAnimation; }
        set { skeletonAnimation = value; }
    }
    public Renderer render;
    private SkeletonUtility skeletonUtility;



    Spine.AnimationState spineAnimationState;
    private bool isEvent = false;
    public bool isEnemy;


    public CardInfo cardInfo;


    void Awake()
    {
        if (AnimationObj != null)
        {
            skeletonAnimation = AnimationObj.GetComponent<SkeletonAnimation>();
            render = AnimationObj.GetComponent<Renderer>();
            skeletonUtility = AnimationObj.GetComponent<SkeletonUtility>();
        }
        cardInfo = GetComponent<CardInfo>();
    }

    public virtual void Init(int index, bool _isEnemy)
    {
        isOnMark = false;
        myIndex = index;
        isEnemy = _isEnemy;
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

        if (spineAnimationState != null)
        {
            //初始动画
            string animationName = "idle";
            spineAnimationState.SetAnimation(0, animationName, true);
        }
        
    }

    public bool isOnMark;
    private float markTimeCount;
    public float markTimeCountMax;

    public virtual void SetMark()
    {
        isOnMark = true;
    }

    public virtual void state_Event(TrackEntry trackEntry, Spine.Event e)
    {

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


    public virtual void BeShoot(float demage)
    {
        cardInfo.UpdateHp(demage);
        if (cardInfo.Hp <= 0)
        {
            Debug.LogError(myType + "->" + myIndex + " 死亡");
            //render.material.DOFloat(1f, "_FillPhase", 0.1f).OnComplete(HitAnimationComplete);
        }
        else
        {
            //render.material.DOFloat(0.5f, "_FillPhase", 0.1f).OnComplete(HitAnimationComplete);
        }
    }

    public virtual void DoAIByType()
    {

    }

    public virtual void DoTimeLogic()
    {
        markTimeCount = 0;
    }


    public void Update()
    {
        if (isOnMark)
        {
            if (markTimeCount >= markTimeCountMax)
            {
                DoTimeLogic();
            }
            else
            {
                markTimeCount += Time.deltaTime;
            }
        }
    }

}


public enum SupportType
{
    Fly,//飞机
    Mortar,//迫击炮
    AddHp,
}