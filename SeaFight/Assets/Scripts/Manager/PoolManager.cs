﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class PoolManager : MonoBehaviour
{
    public static PoolManager instance;

    public Dictionary<PreLoadType, PoolInitInfo> poolInfo = new Dictionary<PreLoadType, PoolInitInfo>() {
        { PreLoadType.Control, new PoolInitInfo() { num = 2, path = DataController.prefPath_Control }},
        //{ PreLoadType.Bullet, new PoolInitInfo() { num = 10, path = DataController.prefabPath_Bullet }},
        //{ PreLoadType.Character, new PoolInitInfo() { num = 10, path = DataController.prefabPath_Character }},
        { PreLoadType.CardHurtParticle,new PoolInitInfo() { num = 10, path = DataController.prefabPath_CardHurtParticle }},
        { PreLoadType.BoatHurtParticle,new PoolInitInfo() { num = 10, path = DataController.prefabPath_BoatHurtParticle }},
        { PreLoadType.ShootParticle,new PoolInitInfo() { num = 10, path = DataController.prefabPath_ShootParticle }},
        //{ PreLoadType.Boat,new PoolInitInfo() { num = 10, path = DataController.prefabPath_Boat }},
        { PreLoadType.PeopleInfo,new PoolInitInfo() { num = 10, path = DataController.prefabPath_PeopleInfo }},
        { PreLoadType.BoatDeadParticle,new PoolInitInfo() { num = 10, path = DataController.prefabPath_BoatDeadParticle }},
        { PreLoadType.SelfHurtParticle,new PoolInitInfo() { num = 10, path = DataController.prefabPath_SelfHurtParticle }},
        { PreLoadType.BulletDesParticle,new PoolInitInfo() { num = 10, path = DataController.prefabPath_BulletDesParticle }},
        
        //add
    };

    public Dictionary<PreLoadType, List<GameObject>> poolObjs = new Dictionary<PreLoadType, List<GameObject>>();

    private int curLoadNum = 0;
    private int maxLoadNum = 1;

    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(this);
    }

    private void OnEnable()
    {
        foreach (var item in poolInfo)
        {
            maxLoadNum += item.Value.num;
        }
        //
        StartCoroutine(GeneratePool());
        LoadingScene.callback = LoadFinish;
    }

    IEnumerator GeneratePool()
    {
        yield return StartCoroutine(PreloadingLogic());
    }

    public IEnumerator PreloadingLogic()
    {
        AddProgress();
        GameObject obj = null;

        foreach (KeyValuePair<PreLoadType, PoolInitInfo> item in poolInfo)
        {
            poolObjs[item.Key] = new List<GameObject>();
            for (int i = 0; i < poolInfo[item.Key].num; i++)
            {
                obj = Common.Generate(item.Value.path, transform);
                obj.SetActive(false);
                poolObjs[item.Key].Add(obj);
                AddProgress();
                yield return obj;
            }
        }
    }

    void AddProgress()
    {
        curLoadNum++;
        LoadingScene.loadProgress = ((float)curLoadNum / maxLoadNum);
    }
    private void LoadFinish()
    {
        LoadingScene.instance.LoadFinish();
        MainUI.Show();
    }



    public GameObject GetPoolObjByType(PreLoadType type, Transform parent, UnityEngine.Vector3? scale = null)
    {
        GameObject obj = null;
        string path = "";
        if (poolObjs[type].Count > 0)
        {
            lock (poolObjs)
            {
                obj = poolObjs[type][0];
                poolObjs[type].RemoveAt(0);
            }
            //
            obj.transform.SetParent(parent);
        }
        else
        {
            path = poolInfo[type].path;
            obj = Common.Generate(path, parent);
        }
        if (scale != null)
        {
            obj.transform.localScale = (Vector3)scale;
        }
        else
        {
            obj.transform.localScale = Vector3.one;
        }
        obj.SetActive(true);
        return obj;
    }

    public void SetPoolObjByType(PreLoadType type, GameObject obj)
    {
        obj.SetActive(false);
        obj.transform.DOKill();
        obj.transform.parent = transform;
        obj.transform.localPosition = Vector3.zero;
        obj.transform.localScale = Vector3.one;
        lock (poolObjs)
        {
            if (!poolObjs[type].Contains(obj))
            {
                poolObjs[type].Add(obj);
            }
            else
            {
                //Debug.Log("E" + type.ToString() + "/已在对象池中，请检查！父物体为：" + obj.transform.parent.name);
            }
        }
    }
}

public class PoolInitInfo
{
    public int num;
    public string path;
}
public enum PreLoadType
{
    Control,
    //Bullet,
    //Character,
    CardHurtParticle,
    BoatHurtParticle,
    ShootParticle,
    //Boat,
    PeopleInfo,
    BoatDeadParticle,
    SelfHurtParticle,
    BulletDesParticle,
}