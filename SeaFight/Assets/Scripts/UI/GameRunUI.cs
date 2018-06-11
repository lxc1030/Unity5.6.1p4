using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameRunUI : MonoBehaviour
{
    public static GameRunUI instance;
    public static string Name = "GameRunUI";


    private void Awake()
    {
        instance = this;
    }

    public ControlPartUI uiControl;

    public static void Show()
    {
        UIManager.instance.ShowPanel(Name);
        instance.Init();
    }
    private void Init()
    {
        uiControl.Show(true);
        //
        //SetNameUIPool();
    }
    public void OnClose()
    {
        Close();
    }
    public static void Close()
    {
        UIManager.instance.HidePanel(Name);
    }




    private string SecondToString(float time)
    {
        DateTime utcdt = DateTime.Parse(DateTime.UtcNow.ToString("1970-01-01 00:00:00")).AddSeconds(time);
        //转成本地时间  
        DateTime localdt = utcdt.ToLocalTime();
        string timeformat = localdt.ToString("mm:ss");
        return timeformat;
    }

    //private void SetNameUIPool()
    //{
    //    for (int i = transName.childCount - 1; i >= 0; i--)
    //    {
    //        PoolManager.instance.SetPoolObjByType(PreLoadType.PeopleInfo, transName.GetChild(i).gameObject);
    //    }
    //}

    #region 遥杆
    public void Open()
    {
        uiControl.Show(true);
    }
    public void BeShoot()
    {
        uiControl.Show(false);
    }

    public void OnMove(Vector2 move)
    {
        GameManager.instance.OnMove(move);
    }
    public void OnMoveEnd()
    {
        GameManager.instance.OnMoveEnd();
    }

    #endregion


    public void OnClickAuto()
    {
        DataController.instance.isAutoPlaying = !DataController.instance.isAutoPlaying;
        if (DataController.instance.isAutoPlaying)
        {
            uiControl.Show(false);
        }
        else
        {
            uiControl.Show(true);
        }
    }

    public void OnClickSkill1()
    {
        GameManager.instance.InitSkill(1);
    }


    public void Update()
    {

    }

}

[Serializable]
public class ControlPartUI
{
    public ETCJoystick etcMove;

    public void Show(bool isEnable)
    {
        etcMove.gameObject.SetActive(isEnable);
    }
}
