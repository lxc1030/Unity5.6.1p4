#undef SanWang
#undef GooglePlay

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

/// <summary>
/// 全局数据保存
/// </summary>
public class DataController : MonoBehaviour
{
    public static DataController instance;
    private static string XMLName = "EffectConfig";
    //public Dictionary<StoneEffectType, EffectConfig> config = new Dictionary<StoneEffectType, EffectConfig>();

    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(this);
    }

    // Use this for initialization
    void Start()
    {
        //FPS
        Application.targetFrameRate = 60;
    }


    private int maxScore = 0;
    public int maxScoreEver
    {
        get
        {
            if (maxScore == 0)
            {
                maxScore = PlayerPref.GetInt("MaxScoreEver", 0);
            }
            return maxScore;
        }
        set
        {
            PlayerPref.SetInt("MaxScoreEver", value);
            maxScore = value;
#if SanWang

#elif GooglePlay
            //更新排行榜
            PaymentManager.instance.ReflashRankData(score);
#endif
        }
    }
    public bool IsFristEnterGame
    {
        get { return PlayerPref.GetInt("IsFristEnterGame", 1) == 1 ? true : false; }
        set { PlayerPref.SetInt("IsFristEnterGame", value ? 1 : 0); }
    }






    //UI
    public const string iconPathSkill = "Image/";

    //prefab
    public const string prefPath_Control = "prefab/Controller";
    public const string prefabPath_Bullet = "prefab/Bullet";
    public const string prefabPath_Character = "prefab/Card_";
    public const string prefabPath_CardHurtParticle = "prefab/CardHurtParticle";
    public const string prefabPath_BoatHurtParticle = "prefab/BoatHurtParticle";
    public const string prefabPath_ShootParticle = "prefab/ShootParticle";
    public const string prefabPath_Boat = "prefab/BoatPrefab";
    public const string prefabPath_PeopleInfo = "prefab/PeopleInfo";



    //matrial
    public const string materialPathBox = "prefab/Material/Box/";
    public const string materialPathKillCount = "prefab/Material/KillCount/";

    public Vector2 orgPos;//主角生成位置
    public Vector2 endPos;//怪物生成位置

    public float moveSpeed;//主角遥杆移动速度
    public float shootRange;//主角普通距离


    public float limetControlTop;//主角上方限制位置
    public float limetControlBottom;//主角下方限制位置
    public float limetCameraYLength;
    public float limetCameraTop;
    public float limetCameraBottom;


    public float ZPow = 1;


    /// <summary>
    /// 结算加金币
    /// </summary>
    /// <param name="num"></param>
    public void SetCoinAdd(int num)
    {
        ItemManager.instance.AddItem(ItemID.Coin, num);
    }

    public void DealPay(ItemID id, int num, Action callback, Action backFail = null, bool isCalculate = true)
    {
        Item item = ItemManager.instance.GetItem(id);
        if (item.Count < num)
        {
            //if (LanguageManager.instance.CurType == LanguageType.中文)
            //{
            //    UIManager.instance.ShowAlertTip(item.name + "不足。");
            //}
            //else
            //{
            //    UIManager.instance.ShowAlertTip("Not enough to resurrect.");
            //}
            //ShopUI.Show();
            //return;
            if (backFail != null)
            {
                backFail();
            }
        }
        else
        {
            if (isCalculate)
            {
                ItemManager.instance.AddItem(id, -num);
            }
            callback();
        }
    }
    #region 材质

    private Dictionary<TeamType, Material> teamMaterials = new Dictionary<TeamType, Material>();
    /// <summary>
    /// 替换材质
    /// </summary>
    /// <param name="hp"></param>
    /// <returns></returns>
    public Material GetMaterialOfTeamType(TeamType type)
    {
        if (teamMaterials.ContainsKey(type))
        {
            if (teamMaterials[type] != null)
            {
                return teamMaterials[type];
            }
        }
        else
        {
            teamMaterials.Add(type, null);
        }
        teamMaterials[type] = (Material)Resources.Load(DataController.materialPathBox + type);
        return teamMaterials[type];
    }

    private Dictionary<int, Material> killMaterials = new Dictionary<int, Material>();

    public Material GetMaterialOfKillCount(int killCount)
    {
        if (killMaterials.ContainsKey(killCount))
        {
            if (killMaterials[killCount] != null)
            {
                return killMaterials[killCount];
            }
        }
        else
        {
            killMaterials.Add(killCount, null);
        }
        killMaterials[killCount] = (Material)Resources.Load(DataController.materialPathKillCount + killCount);
        return killMaterials[killCount];
    }

    #endregion



    #region 根据参数返回静态值

    public static float BackBulletSpeed(int index)
    {
        float speed = 0;
        if (index == 0)
        {
            speed = 50;
        }
        return speed;
    }
    public static float BackShootSpan(int index)
    {
        float span = 0;
        if (index == 0)
        {
            span = 0.4f;
        }
        return span;
    }
    #endregion

    

    public void Update()
    {
    
    }
}




/// <summary>
/// 对应的item.xml
/// 是ItemID的延伸
/// </summary>
public enum ItemId
{
    体力 = 4,
}
/// <summary>
/// 需要在Inspector上设置
/// </summary>
public enum Tag
{
    Bullet,
    Player,
    Enemy,
    Boat,
}

public enum BoatType
{
    NoBullet,
    NormalBullet,
    Max,
}
public enum RenderType
{
    Background,
    Island,
    Card,
    ParticleSystem,
}