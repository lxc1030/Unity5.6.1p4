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
    public Transform transEnemy;
    public Transform transBullet;
    public Transform transHurt;
    public Transform transShoot;


    public Controller myControl;

    public List<int> cardIndexs;
    public List<int> enemyIndexs;
    public List<int> boatIndexs;


    public List<CharacterInfo> allModel;
    public List<GameObject> showEnemy;

    #region 相机相关参数
    public Camera gameCamera;//游戏相机
    private float cameraDistance = -720f;//相机距离
    /// <summary>
    /// X是否跟随，Y是否跟随，X跟随限制值，Y跟随限制值
    /// </summary>
    public Rect followLimet /*= new Rect(0, 1, 0, 10)*/;

    public float followFixTime = 0.3f;

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
        TestGenerateEnemy();

        ChangeState(GameState.开始战斗);
    }

    private void GenerateMyControl()
    {
        GameObject obj = null;
        //obj = PoolManager.instance.GetPoolObjByType(PreLoadType.Control, transMy);
        Vector3 pos = new Vector3(-20, 0, 0);
        //obj.transform.localPosition = pos;
        //myControl = obj.GetComponent<Controller>();
        //myControl.Init(cardIndexs, false);
        //myControl.transform.DOLocalMove(DataController.instance.orgPos, 0.5f);

        allModel = new List<CharacterInfo>();
        bool isEnemy = false;
        for (int i = 0; i < cardIndexs.Count; i++)
        {
            obj = Common.Generate(DataController.prefabPath_Character + cardIndexs[i], transMy);
            obj.transform.localPosition = pos;

            CharacterInfo info = obj.GetComponent<CharacterInfo>();
            info.Init(cardIndexs[i], isEnemy);
            if (isEnemy)
            {
                obj.tag = nameof(Tag.Enemy);
                info.AnimationObj.transform.localEulerAngles = new Vector3(0, 180, 0);
            }
            else
            {
                obj.tag = nameof(Tag.Player);
                info.AnimationObj.transform.localEulerAngles = new Vector3(0, 0, 0);
            }
            allModel.Add(info);
        }

        endPoses = new Vector2[] { DataController.instance.orgPos, DataController.instance.orgPos, DataController.instance.orgPos };
    }

    private void TestGenerateEnemy()
    {
        GameObject obj = null;
        Controller con = null;
        obj = PoolManager.instance.GetPoolObjByType(PreLoadType.Control, transEnemy);
        obj.tag = nameof(Tag.Enemy);
        con = obj.GetComponent<Controller>();
        con.Init(enemyIndexs);
        Vector3 pos = new Vector3(20, 0, 0);
        obj.transform.localPosition = pos;
        obj.transform.DOLocalMove(DataController.instance.endPos, 0.5f);
        showEnemy.Add(obj);
    }

    private void TestBoat()
    {
        GameObject obj = null;
        obj = PoolManager.instance.GetPoolObjByType(PreLoadType.Boat, transEnemy);
        obj.tag = nameof(Tag.Boat);
        BoatInfo info = obj.GetComponent<BoatInfo>();
        int typeIndex = UnityEngine.Random.Range((int)BoatType.NoBullet, (int)BoatType.Max);
        Vector3 pos = new Vector3(7, UnityEngine.Random.Range(DataController.instance.limetControlTop, DataController.instance.limetControlBottom), 0);
        float speed = 0;
        switch ((BoatType)typeIndex)
        {
            case BoatType.NoBullet:
                speed = 2f;
                break;
            case BoatType.NormalBullet:
                speed = 0.5f;
                break;
        }
        info.Init((BoatType)typeIndex, pos, speed);
        showEnemy.Add(obj);
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
                Atk = 1;
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
                Hp = 1000;
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
                Hp = 40;
                break;
            case 5:
                Hp = 200;
                break;
        }
        return Hp;
    }
    public static float BackBoatHp(BoatType type)
    {
        float hp = 0;
        switch (type)
        {
            case BoatType.NoBullet:
                hp = 40;
                break;
            case BoatType.NormalBullet:
                hp = 200;
                break;
        }
        return hp;
    }
    public static float BackBoatAtk(BoatType type)
    {
        float atk = 0;
        switch (type)
        {
            case BoatType.NoBullet:
                atk = 0;
                break;
            case BoatType.NormalBullet:
                atk = 10;
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

    }


    public Vector2[] endPoses;

    public float Vt;
    public Vector2 a;
    public float span = 0.1f;
    public void CharacterMove(Vector2 move)
    {
        float time = Time.deltaTime;


        Vector2 saveMove = -move;
        //Vector2 add = move * DataController.instance.moveSpeed * Time.deltaTime;
        //myControl.SetMove(add);

        Vector2 curPos;

        curPos = allModel[0].transform.localPosition;
        move *= span * Time.deltaTime;
        GameManager.instance.LimetControlY(curPos, ref move);
        endPoses = new Vector2[allModel.Count];

        for (int i = 0; i < endPoses.Length; i++)
        {
            endPoses[i] = BackVecByDirection(curPos, saveMove.normalized) * i;
        }
        //allModel[0].transform.position += (Vector3)move;

        CardMoveTowards();
    }

    public float speed = 1;
    private void CardMoveTowards()
    {
        for (int i = 0; i < allModel.Count; i++)
        {
            //allModel[i].transform.Translate((allModel[i].transform.localPosition - (Vector3)endPoses[i]).normalized * Time.deltaTime);

            if (Vector3.Distance(endPoses[i], allModel[i].transform.localPosition) > 0.05f)
            {
                Vector2 offSet = endPoses[i] - (Vector2)allModel[i].transform.localPosition;
                allModel[i].transform.localPosition += (Vector3)offSet.normalized * speed * Time.deltaTime;
            }
        }
    }


    private Vector2 BackVecByDirection(Vector2 pos, Vector2 dir)
    {
        return pos + dir;
    }


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

    public void ShootFree(CharacterInfo card)
    {

    }

    #region 限制移动区域

    private float distanceCCY;
    private void LimetCameraY()
    {
        if (myControl != null)
        {
            distanceCCY = myControl.transform.localPosition.y - gameCamera.transform.localPosition.y;
            if (Mathf.Abs(distanceCCY) > DataController.instance.limetCameraYLength)
            {
                float y = myControl.transform.localPosition.y + (distanceCCY > 0 ? -1 : 1) * DataController.instance.limetCameraYLength;
                if (y < DataController.instance.limetCameraTop && y > DataController.instance.limetCameraBottom)
                {
                    gameCamera.transform.localPosition = new Vector3(gameCamera.transform.localPosition.x, y, gameCamera.transform.localPosition.z);
                }
                else
                {
                    //超过相机设置限制上下区域，相机不移动
                }
            }
        }
    }

    public void LimetControlY(Vector2 pos, ref Vector2 add)
    {
        if (myControl != null)
        {
            Vector2 end = pos + add;
            if (end.y < DataController.instance.limetControlTop && end.y > DataController.instance.limetControlBottom)
            {
                //在限制区域内，可以移动
            }
            else
            {
                add = new Vector2(add.x, 0);
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



    public float BoatCD;
    public float BoatCdLimet = 3;

    private void Update()
    {
        LimetCameraY();
        CardMoveTowards();
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
        gameCamera.transform.localPosition = Vector3.forward * cameraDistance / 100;
        gameCamera.fieldOfView = 2 * Mathf.Atan(UIManager.instance.ScreenScale.y * 0.5f / Mathf.Abs(cameraDistance)) * Mathf.Rad2Deg;
    }

}
public enum GameState
{
    游戏未开始,
    游戏场景初始化,
    开始战斗,
}