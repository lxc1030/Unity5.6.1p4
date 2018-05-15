using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HomeUI : MonoBehaviour
{
    public static HomeUI instance;
    public static string Name = "HomeUI";
    private static Action callback;

    private void Awake()
    {
        instance = this;
    }

    private void OnDestroy()
    {
        GameEventDispatcher.removeListener(ItemCountChangeEvent.ITEM_COUNT_CHAGE_EVENT_TAG, ItemChangeEventLister);
    }
    private void OnEnable()
    {
        ADManager.instance.ShowAdByType(ADType.ADS_TYPE_RECOMMAND_BANNER, null);
    }

    private void ItemChangeEventLister(object sender, GameEvent evt)
    {
        ItemCountChangeEvent me = (ItemCountChangeEvent)evt;
        Item item = ItemManager.instance.GetItem(me.itemId);
        if (item.tag == ItemTag.currency.ToString())
        {
            ReflashShow();
        }
    }

    public static void Show()
    {
        UIManager.instance.ShowPanel(Name);
        instance.Init();
    }

    public static void Close()
    {
        UIManager.instance.HidePanel(Name);
    }



    public void Init()
    {
        ReflashShow();
        //
    }
    public void ReflashShow()
    {
        //imgCoin.Show(ItemManager.instance.GetItem(ItemID.Coin) + "");
    }


    #region 按钮点击方法

    public void OnClickPause()
    {
        AudioManager.instance.Play();
        GamePause.Show();
    }
    public void OnClickSetting()
    {
        AudioManager.instance.Play();
        Setting.Show();
    }


    public void OnClickCharacter()
    {
        UIManager.instance.ShowAlertTip("暂未开放");
    }
    public void OnClickItem()
    {
        UIManager.instance.ShowAlertTip("暂未开放");
    }

    public void OnClickMain()
    {
        AudioManager.instance.Play();
        BackToMain();
    }

    /// <summary>
    /// 返回主界面
    /// </summary>
    private void BackToMain()
    {
        //清除
        Close();
        MainUI.Show();
    }


    public void OnClickAD()
    {
        if (PaymentManager.instance.isOpenAD)
        {
            ADManager.instance.ShowAdByType(ADType.ADS_TYPE_UNITY_REWARD_VIDEO, CallBackCoin, ADFailPlay);
        }
    }


    public void OnClickSelectModel()
    {
        Close();
        GameManager.instance.ChangeState(GameState.游戏场景初始化);
    }

    #endregion





    /// <summary>
    /// 广告加金币
    /// </summary>
    private void CallBackCoin()
    {
        DataController.instance.SetCoinAdd(10);
    }
    /// <summary>
    /// 看广告失败
    /// </summary>
    private void ADFailPlay()
    {
        if (LanguageManager.instance.CurType == LanguageType.中文)
        {
            UIManager.instance.ShowAlertTip("暂时不能获得免费金币。");
        }
        else
        {
            UIManager.instance.ShowAlertTip("Unable to obtain free gold coins temporarily.");
        }
    }


    private void Update()
    {

    }

}