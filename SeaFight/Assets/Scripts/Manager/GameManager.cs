using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using System;
using Newtonsoft.Json.Linq;
/// <summary>
/// 游戏运行中的相关逻辑
/// </summary>
public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private static string XMLName = "EndlessHardConfig";
    //public Dictionary<int, EndlessHardConfig> config = new Dictionary<int, EndlessHardConfig>();
    public GameState CurState;

    public Transform transMy;
    public Transform transSupport;
    public Transform transEnemy;
    public Transform transBullet;
    public Transform transHurt;
    public Transform transShoot;


    //public Controller myControl;

    public List<int> cardIndexs;
    public List<int> supportIndexs;
    public List<int> enemyIndexs;
    public List<int> boatIndexs;

    public List<SupportInfo> showSupport;

    public List<CardInfo> showEnemy;

    #region 相机相关参数
    public Camera gameCamera;//游戏相机
    public Camera seaCamera;

    #endregion



    private void Awake()
    {
        DontDestroyOnLoad(this);
        instance = this;
        //
        instance.Init();
    }


    //private void ReadConfig()
    //{
    //    List<EndlessHardConfig> temp = new List<EndlessHardConfig>();
    //    FileUtil.loadConfig(XMLName, ref temp, null, false);
    //    for (int i = 0; i < temp.Count; i++)
    //    {
    //        config.Add(temp[i].id, temp[i]);
    //    }
    //}

    public void Init()
    {
        ChangeState(GameState.游戏未开始);
    }
    public void ChangeState(GameState state)
    {
        CurState = state;
        switch (CurState)
        {
            case GameState.游戏未开始:
                gameCamera.gameObject.SetActive(false);
                seaCamera.gameObject.SetActive(false);
                break;
            case GameState.游戏场景初始化:
                SetGameInit();
                break;
            case GameState.开始战斗:
                SetGaming();
                break;
        }
    }



    private void SetGameInit()
    {
        SetCameraInit();

        GenerateMyControl();
        GenerateSupport();
        TestGenerateEnemy();

        //我方卡牌移动到限定区域
        DoMyCardAnimation();

        ChangeState(GameState.开始战斗);
    }

    private void GenerateMyControl()
    {
        for (int i = 0; i < cardIndexs.Count; i++)
        {
            GameObject obj = null;
            obj = Common.Generate(DataController.prefabPath_Character + cardIndexs[i], transMy);
            obj.transform.position = DataController.instance.playerPos - new Vector3(3, 0, 0);
            //
            CharacterInfo info = obj.GetComponent<CharacterInfo>();
            info.Init(cardIndexs[i], false);
            //
            CardInfo cardInfo = obj.GetComponent<CardInfo>();
            cardInfo.myTag = Tag.Player;
            cardInfo.isEnemy = false;
            cardInfo.Hp = GameManager.BackCardHp(info.myIndex);
            cardInfo.Atk = GameManager.BackCardAtk(info.myIndex);
            cardInfo.myName = "我方->" + (int)info.myIndex;
            cardInfo.SetInit();
            //
            //info.AnimationObj.transform.localEulerAngles = new Vector3(0, 0, 0);
            myCards.Add(info);
        }
        DataController.instance.cameraFollowIndex = 0;
    }
    private void GenerateSupport()
    {
        for (int i = 0; i < supportIndexs.Count; i++)
        {
            GameObject obj = null;
            obj = Common.Generate(DataController.prefabPath_Character + supportIndexs[i], transSupport);
            Vector3 pos = new Vector3(DataController.instance.supportPosX, 0, 0);
            obj.transform.position = pos;
            obj.transform.DOMove(new Vector3(DataController.instance.supportPosX, 0, DataController.instance.supportPosZ[i]), 0.5f);
            //
            SupportInfo info = obj.GetComponent<SupportInfo>();
            info.Init(supportIndexs[i], true);
            //
            CardInfo cardInfo = info.cardInfo;
            cardInfo.myTag = Tag.Support;
            cardInfo.isEnemy = false;
            cardInfo.Hp = GameManager.BackCardHp(info.myIndex);
            cardInfo.Atk = GameManager.BackCardAtk(info.myIndex);
            cardInfo.myName = "支援->" + (int)info.myIndex;
            cardInfo.SetInit();
            //
            showSupport.Add(info);
        }
    }
    private void TestGenerateEnemy()
    {
        for (int i = 0; i < enemyIndexs.Count; i++)
        {
            GameObject obj = null;
            obj = Common.Generate(DataController.prefabPath_Character + enemyIndexs[i], transEnemy);
            obj.transform.position = DataController.instance.bossPos + new Vector3(3, 0, 0);
            obj.transform.DOMove(DataController.instance.bossPos, 0.5f);
            //
            CharacterInfo info = obj.GetComponent<CharacterInfo>();
            info.Init(enemyIndexs[i], true);
            //
            CardInfo cardInfo = info.cardInfo;
            cardInfo.myTag = Tag.Enemy;
            cardInfo.isEnemy = false;
            cardInfo.Hp = GameManager.BackCardHp(info.myIndex);
            cardInfo.Atk = GameManager.BackCardAtk(info.myIndex);
            cardInfo.myName = "敌方->" + (int)info.myIndex;
            cardInfo.SetInit();
            //转向面向主角
            info.AnimationObj.localScale = new Vector3(-1, 1, 1);
            showEnemy.Add(cardInfo);
            //
            RandomMove erMove = obj.AddComponent<RandomMove>();
            float radius = 2f;
            erMove.center = new Vector3(DataController.instance.bossPos.x, 0, DataController.instance.playerPos.z) - new Vector3(radius, 0);
            erMove.radius = radius;
            erMove.timeRandom = new Vector2(2, 5);
            erMove.Init();
        }
    }

    private void TestBoat()
    {
        int typeIndex = -1;
        float select = UnityEngine.Random.Range(0, 100);
        if (select >= 0 && select < 70)
        {
            typeIndex = (int)BoatType.NormalAttack;
        }
        else if (select >= 70 && select < 100)
        {
            typeIndex = (int)BoatType.SuicideAttack;
        }

        //
        int index = UnityEngine.Random.Range(0, DataController.instance.boatPositions.Length);
        Vector3 pos = new Vector3(7, 0, DataController.instance.boatPositions[index]);

        float speed = 0;
        //
        GameObject obj = null;
        obj = Common.Generate(DataController.prefabPath_Boat + typeIndex, transEnemy);
        obj.transform.localPosition = pos;
        //
        obj.tag = nameof(Tag.Boat);
        BoatInfo info = obj.GetComponent<BoatInfo>();
        //
        CardInfo cardInfo = info.cardInfo;
        cardInfo.myTag = Tag.Boat;
        cardInfo.isEnemy = true;
        cardInfo.Hp = GameManager.BackBoatHp((BoatType)typeIndex);
        cardInfo.Atk = GameManager.BackBoatAtk((BoatType)typeIndex);
        cardInfo.myName = "船只->" + (int)typeIndex;
        cardInfo.SetInit();
        //
        switch ((BoatType)typeIndex)
        {
            case BoatType.NormalAttack:
                speed = 0.5f;
                break;
            case BoatType.SuicideAttack:
                speed = 6f;
                break;
        }
        info.Init(speed);
        //
        showEnemy.Add(cardInfo);
    }

    private void DoMyCardAnimation()
    {
        for (int i = 0; i < myCards.Count; i++)
        {
            myCards[i].transform.DOMove(DataController.instance.playerPos + new Vector3(0, 0, myCardEndLimetLength) * i, 0.5f);
        }
    }


    #region 静态方法

    public static float BackAngleOfTarget(CardInfo target, Vector3 shootPoint)
    {
        if (target == null)
            return 0;
        Vector3 targetPos = target.transform.position;
        float angle = Mathf.Atan2(targetPos.z - shootPoint.z, targetPos.x - shootPoint.x);
        angle = 180 / Mathf.PI * angle;
        return angle;
    }


    public static float BackCardAtk(int index)
    {
        float Atk = 0;
        switch (index)
        {
            case 0:
                Atk = 1;
                break;
            case 1:
                Atk = 10;
                break;
            case 2:
                Atk = 10;
                break;
            case 3:
                Atk = 10;
                break;
            case 4:
                Atk = 10;
                break;
            case 5:
                Atk = 10;
                break;
            case 6:
                Atk = 10;
                break;
            default:
                Atk = 10;
                break;
        }
        return Atk;
    }
    public static float BackCardHp(int index)
    {
        float Hp = 0;
        switch (index)
        {
            case 0:
                Hp = 100000;
                break;
            case 1:
                Hp = 200;
                break;
            case 2:
                Hp = 200;
                break;
            case 3:
                Hp = 200;
                break;
            case 4:
                Hp = 2000;
                break;
            case 5:
                Hp = 2000;
                break;
            case 6:
                Hp = 2000;
                break;
            default:
                Hp = 10000;
                break;
        }
        return Hp;
    }
    public static float BackBoatHp(BoatType type)
    {
        float hp = 0;
        switch (type)
        {
            case BoatType.NormalAttack:
                hp = 100;
                break;
            case BoatType.SuicideAttack:
                hp = 1000;
                break;
        }
        return hp;
    }
    public static float BackBoatAtk(BoatType type)
    {
        float atk = 0;
        switch (type)
        {
            case BoatType.NormalAttack:
                atk = 1;
                break;
            case BoatType.SuicideAttack:
                atk = 50;
                break;
        }
        return atk;
    }

    public static string BackHpSpName(bool isEnemy)
    {
        string name = "";
        if (isEnemy)
        {
            name = "Hp_Red";
        }
        else
        {
            name = "Hp_Green";
        }
        return name;
    }

    public static void SetRenderQueueByType(Renderer render, float y)
    {
        int queue = 0;
        render.sortingOrder = queue - (int)(Math.Round(y, 2) * 100);
    }


    #endregion



    private void SetGaming()
    {
        GameRunUI.Show();
        //敌人AI移动
        for (int i = 0; i < showEnemy.Count; i++)
        {
            showEnemy[i].GetComponent<RandomMove>().SetMove();
        }
        //后排计时开始
        for (int i = 0; i < showSupport.Count; i++)
        {
            showSupport[i].SetMark();
        }
    }



    #region 我方移动
    public List<CharacterInfo> myCards;
    public float myCardMoveSpeed;
    public float myCardLimetLength;
    public float myCardSpeedSpan;
    private Vector3 myCardMoveDir;

    public float myCardEndLimetLength;
    public float myCardEndTime;
    private int myCardLastAngle;

    //public void CharacterMove(Vector2 move)
    //{
    //    float time = Time.deltaTime;
    //    myControl.SetMove(new Vector3(move.x, 0, move.y) * time);
    //}


    /// <summary>
    /// 获取2个点在Z轴形成的角度
    /// </summary>
    /// <param name="leader"></param>
    /// <param name="follow"></param>
    /// <returns></returns>
    float GetAngle(Vector3 leader, Vector3 follow)
    {
        float angleOfLine = Mathf.Atan2((leader.z - follow.z), (leader.x - follow.x)) * 180 / Mathf.PI;
        return angleOfLine;
    }

    Vector3 GetMoveDirection(Vector3 direction, float angle)
    {
        float length = direction.magnitude;
        float x = length * Mathf.Cos(angle * Mathf.PI / 180);
        float z = length * Mathf.Sin(angle * Mathf.PI / 180);
        Vector3 result = new Vector3(x, 0, z);
        return result;
    }


    public void OnMove(Vector2 move)
    {
        myCardMoveDir = new Vector3(move.x, 0, move.y);
        for (int i = 0; i < myCards.Count; i++)
        {
            myCards[i].DOKill();
            Vector3 moveDir = myCardMoveDir;
            if (i >= 1)
            {
                //
                float distance = Vector3.Distance(myCards[i - 1].transform.position, myCards[i].transform.position);
                if (distance < myCardLimetLength)
                {
                    moveDir = moveDir * (1 - (i * myCardSpeedSpan));
                }
                else
                {
                    moveDir = GetMoveDirection(myCardMoveDir, GetAngle(myCards[i - 1].transform.position, myCards[i].transform.position));
                }
            }
            Vector3 add = moveDir * myCardMoveSpeed * Time.deltaTime;
            GameManager.instance.LimetControl(myCards[i].transform.position, ref add);
            //myCards[i].transform.position += (Vector3)add;
            myCards[i].transform.DOMove(myCards[i].transform.position + add, Time.deltaTime).SetEase(Ease.Linear);
        }
    }


    public void OnMoveEnd()
    {
        float tempAngle = GetAngle(Vector3.zero, myCardMoveDir);
        //int curAngle = Mathf.CeilToInt((tempAngle - 22.5f) / 45) * 45;
        //if (lastAngle == curAngle)
        //{
        //    return;
        //}
        //lastAngle = curAngle;
        //
        float x1 = myCardEndLimetLength * Mathf.Cos(tempAngle * Mathf.PI / 180);
        float z1 = myCardEndLimetLength * Mathf.Sin(tempAngle * Mathf.PI / 180);

        for (int i = 1; i < myCards.Count; i++)
        {
            myCards[i].DOKill();

            Vector3 endPos = myCards[0].transform.position + new Vector3(x1, 0, z1) * i;
            Vector3 fixDir = myCards[i].transform.position - endPos;
            float time = myCardEndTime * fixDir.magnitude / myCardEndLimetLength;
            time /= i;

            Vector3 add = fixDir;
            GameManager.instance.LimetControl(ref endPos);
            myCards[i].transform.DOMove(endPos, time).SetEase(Ease.Linear);
        }
    }



    #endregion



    #region 射击判断
    private float autoPlayCurTime;
    private float autoPlayMarkTime;
    public Vector2 autoPlayTimeRandom;
    private Vector2 autoDirection;

    public float autoSpeed;
    public void CheckPlayAuto()
    {
        if (DataController.instance.isAutoPlaying)
        {
            Vector2 fixDirection = Vector2.zero;
            autoPlayCurTime += Time.deltaTime;
            if (autoPlayCurTime > autoPlayMarkTime)
            {
                autoPlayMarkTime = UnityEngine.Random.Range(autoPlayTimeRandom.x, autoPlayTimeRandom.y);
                autoPlayCurTime = 0;
                float radius = 1;
                float tempAngle = UnityEngine.Random.Range(0, 360);
                float x1 = radius * Mathf.Cos(tempAngle * Mathf.PI / 180);
                float z1 = radius * Mathf.Sin(tempAngle * Mathf.PI / 180);
                autoDirection = new Vector2(x1, z1) * autoSpeed;
            }
            //靠近限制边界直接重新随机
            
            
            GameManager.instance.OnMove(autoDirection);
        }
    }


    public void ShootEnemy(CharacterInfo card)
    {
        bool isHave = false;
        int index = -1;
        float markDis = card.shootRange;
        for (int i = 0; i < showEnemy.Count; i++)
        {
            if (!showEnemy[i].isLive)
            {
                continue;
            }
            float distance = Vector2.Distance(card.transform.position, showEnemy[i].transform.position);
            if (distance <= markDis)
            {
                isHave = true;
                index = i;
                markDis = distance;
            }
        }
        if (isHave)
        {
            card.Shoot(showEnemy[index]);
        }
    }


    public void ShootController<T>(T t, Vector3 checkPos, float shootRange)
    {
        bool isHave = false; ;
        int index = -1;
        List<GameObject> objs = new List<GameObject>();
        for (int i = 0; i < myCards.Count; i++)
        {
            objs.Add(myCards[i].gameObject);
        }

        CharacterInfo cInfo = t as CharacterInfo;
        BoatInfo bInfo = t as BoatInfo;

        //switch (tag)
        //{
        //    case Tag.Enemy:
        //        cInfo = t as CharacterInfo;
        //        checkPos = cInfo.transform.position;
        //        break;
        //    case Tag.Boat:
        //        bInfo = t as BoatInfo;
        //        checkPos = bInfo.transform.position;
        //        break;
        //}
        GetShootInfo(objs, checkPos, shootRange, ref isHave, ref index);
        if (isHave)
        {
            if (cInfo != null)
            {
                cInfo.Shoot(myCards[index].cardInfo);
            }
            if (bInfo != null)
            {
                bInfo.Shoot(myCards[index].cardInfo);
            }
        }
    }
    private void GetShootInfo(List<GameObject> targets, Vector3 check, float checkDis, ref bool isHave, ref int index)
    {
        for (int i = 0; i < myCards.Count; i++)
        {
            float distance = Vector2.Distance(check, myCards[i].transform.position);

            if (distance <= checkDis)
            {
                isHave = true;
                index = i;
                checkDis = distance;
            }
        }
    }

    #endregion



    #region 相机
    public bool isPlayCameraAnimation;
    private Vector3 cameraAniPosition;

    public void DoCameraAnimation(Vector3 orgPos)
    {
        if (!isPlayCameraAnimation)
        {


        }
    }





    private void LimetCamera()
    {
        if (myCards.Count <= DataController.instance.cameraFollowIndex || myCards[DataController.instance.cameraFollowIndex] == null)
            return;

        Vector3 distance = myCards[DataController.instance.cameraFollowIndex].transform.position - gameCamera.transform.position;
        //减去原始差距
        distance -= DataController.instance.playerPos - DataController.instance.cameraPosition;
        //
        if (Mathf.Abs(distance.x) > DataController.instance.limetCameraFollowX)
        {
            //左右跟随
            distance.x = (distance.x / Mathf.Abs(distance.x)) * (Mathf.Abs(distance.x) - DataController.instance.limetCameraFollowX);
        }
        else
        {
            distance.x = 0;
        }
        distance.y = 0;
        if (Mathf.Abs(distance.z) > DataController.instance.limetCameraFollowZ)
        {
            //上下跟随
            distance.z = (distance.z / Mathf.Abs(distance.z)) * (Mathf.Abs(distance.z) - DataController.instance.limetCameraFollowZ);
        }
        else
        {
            distance.z = 0;
        }
        //
        if (distance.x != 0 || distance.z != 0)
        {
            Vector3 endPosition = gameCamera.transform.position + distance;
            if (endPosition.z > DataController.instance.limetCameraTop)
            {
                endPosition.z = DataController.instance.limetCameraTop;
            }
            if (endPosition.z < DataController.instance.limetCameraBottom)
            {
                endPosition.z = DataController.instance.limetCameraBottom;
            }
            gameCamera.transform.DOMove(endPosition, Time.deltaTime);
        }
    }


    public void LimetControl(Vector3 pos, ref Vector3 add)
    {
        Vector3 end = pos + add;
        //
        if (end.z < DataController.instance.limetControlTop && end.z > DataController.instance.limetControlBottom)
        {
            //在限制区域内，可以移动
        }
        else
        {
            add = new Vector3(add.x, add.y, 0);
        }
        if (end.x < DataController.instance.limetControlRight && end.x > DataController.instance.limetControlLeft)
        {
            //在限制区域内，可以移动
        }
        else
        {
            add = new Vector3(0, add.y, add.z);
        }
    }
    public bool LimetControl(ref Vector3 fixPos)
    {
        bool isLimet = false;
        if (fixPos.x > DataController.instance.limetControlRight)
        {
            isLimet = true;
            fixPos.x = DataController.instance.limetControlRight;
        }
        if (fixPos.x < DataController.instance.limetControlLeft)
        {
            isLimet = true;
            fixPos.x = DataController.instance.limetControlLeft;
        }
        if (fixPos.z > DataController.instance.limetControlTop)
        {
            isLimet = true;
            fixPos.z = DataController.instance.limetControlTop;
        }
        if (fixPos.z < DataController.instance.limetControlBottom)
        {
            isLimet = true;
            fixPos.z = DataController.instance.limetControlBottom;
        }
        return isLimet;
    }


    #endregion


    public void SetParticle(PreLoadType type, Vector3 pos, bool isWorld = false)
    {
        Transform trans = null;
        switch (type)
        {
            case PreLoadType.ShootParticle:
                trans = transShoot;
                break;
            case PreLoadType.BoatDeadParticle:
                trans = transHurt;
                break;
            default:
                Debug.LogError("type：" + type + "特效未设置节点");
                break;
        }
        GameObject obj = null;
        obj = PoolManager.instance.GetPoolObjByType(type, trans);
        if (!isWorld)
        {
            obj.transform.localPosition = pos;
        }
        else
        {
            obj.transform.position = pos;
        }
    }

    public void DeleteBoat(CardInfo obj)
    {
        DeleteEnemy(obj);
        SetCameraShake();
    }
    public void DeleteEnemy(CardInfo obj)
    {
        showEnemy.Remove(obj);
        Destroy(obj.gameObject);
    }

    public void SetCameraShake()
    {
        gameCamera.DOShakePosition(0.2f, 0.3f, 39, 90);
    }
    public void InitSkill(int type)
    {
        Debug.LogError("使用技能->" + type);
        switch (type)
        {
            case 0:
                break;
            case 1:
                break;
        }
    }


    public float BoatCD;
    public float BoatCdLimet = 3;

    private void Update()
    {
        LimetCamera();
        CheckPlayAuto();
        if (CurState == GameState.开始战斗)
        {
            BoatCD += Time.deltaTime;
            if (BoatCD > BoatCdLimet)
            {
                BoatCD -= BoatCdLimet;
                TestBoat();
            }
        }
    }


    [ContextMenu("角度")]
    void SetCameraInit()
    {
        gameCamera.transform.position = DataController.instance.cameraPosition;
        gameCamera.gameObject.SetActive(true);
        seaCamera.gameObject.SetActive(true);
    }

}
public enum GameState
{
    游戏未开始,
    游戏场景初始化,
    开始战斗,
}