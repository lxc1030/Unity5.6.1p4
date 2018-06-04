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


    public Controller myControl;

    public List<int> cardIndexs;
    public List<int> supportIndexs;
    public List<int> enemyIndexs;
    public List<int> boatIndexs;

    public List<SupportInfo> showSupport;

    public List<GameObject> showEnemy;

    #region 相机相关参数
    public Camera gameCamera;//游戏相机
    private float cameraDistance = -720f;//相机距离
    /// <summary>
    /// X是否跟随，Y是否跟随，X跟随限制值，Y跟随限制值
    /// </summary>
    public Rect followLimet /*= new Rect(0, 1, 0, 10)*/;

    public float followFixTime = 0.3f;


    public float cameraAngle;

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
        cameraAngle = gameCamera.transform.localEulerAngles.x;
    }
    public void ChangeState(GameState state)
    {
        CurState = state;
        switch (CurState)
        {
            case GameState.游戏未开始:
                gameCamera.gameObject.SetActive(false);
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

        ChangeState(GameState.开始战斗);
    }

    private void GenerateMyControl()
    {
        GameObject obj = null;
        obj = PoolManager.instance.GetPoolObjByType(PreLoadType.Control, transMy);
        obj.transform.position = DataController.instance.playerPos - new Vector3(3, 0, 0);
        myControl = obj.GetComponent<Controller>();
        myControl.Init(cardIndexs);
        myControl.transform.DOMove(DataController.instance.playerPos, 0.5f);
    }
    private void GenerateSupport()
    {
        for (int i = 0; i < supportIndexs.Count; i++)
        {
            GameObject obj = null;
            obj = Common.Generate(DataController.prefabPath_Character + supportIndexs[i], transSupport);
            Vector3 pos = new Vector3(DataController.instance.supportPosX, 0, 0);
            obj.transform.position = pos;
            obj.transform.DOMove(new Vector2(DataController.instance.supportPosX, DataController.instance.supportPosY[i]), 0.5f);
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
        //Controller con = null;
        //obj = PoolManager.instance.GetPoolObjByType(PreLoadType.Control, transEnemy);
        //obj.tag = nameof(Tag.Enemy);
        //con = obj.GetComponent<Controller>();
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
            showEnemy.Add(obj);
            //
            RandomMove erMove = obj.AddComponent<RandomMove>();
            float radius = 1.5f;
            erMove.center = new Vector3(DataController.instance.bossPos.x, 0, myControl.transform.position.z) + new Vector3(radius, 0);
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

        GameObject obj = null;
        obj = Common.Generate(DataController.prefabPath_Boat + typeIndex, transEnemy);
        obj.tag = nameof(Tag.Boat);
        BoatInfo info = obj.GetComponent<BoatInfo>();
        //
        Vector3 pos = new Vector3(7, 0, UnityEngine.Random.Range(DataController.instance.limetControlTop, DataController.instance.limetControlBottom));
        float speed = 0;
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
        info.Init(pos, speed);
        //
        showEnemy.Add(obj);
    }


    public static float BackAngleOfTarget(Vector3 targetPos, Vector3 shootPoint)
    {
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
    public static void SetZPosition(Transform trans)
    {
        return;
        float endZ = trans.position.y * DataController.instance.ZPow / (DataController.instance.limetControlTop - DataController.instance.limetControlBottom);
        endZ -= 0.6f;
        trans.position = new Vector3(trans.position.x, trans.position.y, endZ);
    }

    public static void SetRenderQueueByType(Renderer render, float y)
    {
        int queue = 0;
        render.sortingOrder = queue - (int)(Math.Round(y, 2) * 100);
    }


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

    public void CharacterMove(Vector2 move)
    {
        float time = Time.deltaTime;
        myControl.SetMove(new Vector3(move.x, 0, move.y) * time);
    }



    #region 射击判断

    public void ShootEnemy(CharacterInfo card)
    {
        bool isHave = false; ;
        int index = -1;
        float markDis = DataController.instance.shootRange;
        for (int i = 0; i < showEnemy.Count; i++)
        {
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
        for (int i = 0; i < myControl.allModel.Count; i++)
        {
            objs.Add(myControl.allModel[i].gameObject);
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
                cInfo.Shoot(myControl.allModel[index].gameObject);
            }
            if (bInfo != null)
            {
                bInfo.Shoot(myControl.allModel[index].gameObject);
            }
        }
    }
    private void GetShootInfo(List<GameObject> targets, Vector3 check, float checkDis, ref bool isHave, ref int index)
    {
        for (int i = 0; i < myControl.allModel.Count; i++)
        {
            float distance = Vector2.Distance(check, myControl.allModel[i].transform.position);

            if (distance <= checkDis)
            {
                isHave = true;
                index = i;
                checkDis = distance;
            }
        }
    }

    #endregion



    #region 限制移动区域

    private float distanceCCY;
    private void LimetCameraY()
    {
        //if (myControl != null)
        //{
        //    distanceCCY = myControl.transform.localPosition.y - gameCamera.transform.localPosition.y;
        //    if (Mathf.Abs(distanceCCY) > DataController.instance.limetCameraYLength)
        //    {
        //        float y = myControl.transform.localPosition.y + (distanceCCY > 0 ? -1 : 1) * DataController.instance.limetCameraYLength;
        //        if (y < DataController.instance.limetCameraTop && y > DataController.instance.limetCameraBottom)
        //        {
        //            gameCamera.transform.localPosition = new Vector3(gameCamera.transform.localPosition.x, y, gameCamera.transform.localPosition.z);
        //        }
        //        else
        //        {
        //            //超过相机设置限制上下区域，相机不移动
        //        }
        //    }
        //}
    }

    public void LimetControlZ(Vector3 pos, ref Vector3 add)
    {
        if (myControl != null)
        {
            Vector3 end = pos + add;
            if (end.z < DataController.instance.limetControlTop && end.z > DataController.instance.limetControlBottom)
            {
                //在限制区域内，可以移动
            }
            else
            {
                add = new Vector3(add.x, 0);
            }
        }
    }
    public void LimetControlZ(ref Vector3 fixPos)
    {
        if (myControl != null)
        {
            if (fixPos.z > DataController.instance.limetControlTop)
            {
                fixPos.z = DataController.instance.limetControlTop;
            }
            if (fixPos.z < DataController.instance.limetControlBottom)
            {
                fixPos.z = DataController.instance.limetControlBottom;
            }
        }
    }


    #endregion


    public void SetParticle(PreLoadType type, Vector3 pos, bool isWorld = false)
    {
        Transform trans = null;
        switch (type)
        {
            case PreLoadType.CardHurtParticle:
                trans = transHurt;
                break;
            case PreLoadType.BoatHurtParticle:
                trans = transHurt;
                break;
            case PreLoadType.ShootParticle:
                trans = transShoot;
                break;
            case PreLoadType.BoatDeadParticle:
                trans = transHurt;
                break;
            case PreLoadType.SelfHurtParticle:
                trans = transHurt;
                break;
            case PreLoadType.BulletDesParticle:
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

    public void DeleteBoat(GameObject obj)
    {
        DeleteEnemy(obj);
        SetCameraShake();
    }
    public void DeleteEnemy(GameObject obj)
    {
        GameObject remove = obj;
        showEnemy.Remove(obj);
        Destroy(remove);
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
        LimetCameraY();
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
        gameCamera.gameObject.SetActive(true);
    }

}
public enum GameState
{
    游戏未开始,
    游戏场景初始化,
    开始战斗,
}