using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    public Transform transCards;
    public List<CharacterInfo> allModel;

    public bool isEnemy;
    public Transform transBulletTarget;

    // Use this for initialization
    void Awake()
    {

    }


    public void Init(List<int> indexs, bool isenemy = true)
    {
        //isCanShoot = true;
        isEnemy = isenemy;
        allModel = new List<CharacterInfo>();
        for (int i = 0; i < indexs.Count; i++)
        {
            GameObject obj = null;
            obj = Common.Generate(DataController.prefabPath_Character + indexs[i], transCards);
            obj.transform.localPosition = Vector3.zero;

            CharacterInfo info = obj.GetComponent<CharacterInfo>();
            info.Init(indexs[i], isenemy);
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
    }


    public float span = 10f;
    public void SetMove(Vector2 add)
    {
        Vector3 saveAdd = add;
        Vector3 curPos = Vector3.zero;

        //Controller
        curPos = transform.position;
        GameManager.instance.LimetControlY(curPos, ref add);
        transform.position += (Vector3)add;

        for (int i = 0; i < allModel.Count; i++)
        {
            //Card
            Vector2 addSpan = (allModel.Count - 1 - i) * saveAdd * span / allModel.Count;
            curPos = allModel[i].transform.position;
            GameManager.instance.LimetControlY(curPos, ref addSpan);

            allModel[i].transform.position += (Vector3)addSpan;
        }
    }

    //private float BackDistanceLimet(int index)
    //{
    //    float distance = 2f;
    //    return distance * (float)(allModel.Count - (index + 1)) / allModel.Count;
    //}


    void OnCollisionEnter(Collision coll)
    {
        string info = "人物碰撞->";
        info += coll.gameObject.name;
        Debug.LogError(info);
    }


    private void CheckCardAttack()
    {
        for (int i = 0; i < allModel.Count; i++)
        {
            CharacterInfo info = allModel[i];
            if (info.isCanShoot)
            {
                if (!isEnemy)
                {
                    GameManager.instance.ShootEnemy(info);
                }
                else
                {
                    GameManager.instance.ShootFree(info);
                }
            }
        }

    }



    // Update is called once per frame
    void Update()
    {
        CheckCardAttack();
    }

}
