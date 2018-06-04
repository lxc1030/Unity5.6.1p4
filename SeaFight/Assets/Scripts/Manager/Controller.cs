using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    public Transform transCards;
    public List<CharacterInfo> allModel;

    public Transform transBulletTarget;

    // Use this for initialization
    void Awake()
    {

    }


    public void Init(List<int> indexs)
    {
        allModel = new List<CharacterInfo>();
        for (int i = 0; i < indexs.Count; i++)
        {
            GameObject obj = null;
            obj = Common.Generate(DataController.prefabPath_Character + indexs[i], transCards);
            //obj.transform.localPosition = Vector3.zero;
            //
            CharacterInfo info = obj.GetComponent<CharacterInfo>();
            info.Init(indexs[i], false);
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
            allModel.Add(info);
        }
    }

    /// <summary>
    /// 卡牌限定间距
    /// </summary>
    private float cardDistance = 1f;
    /// <summary>
    /// 卡牌每帧移动间隔
    /// </summary>
    private float cardMoveMagnitude = 1f;
    public Vector3[] prePos;
    public Vector3[] moveDirection;
    


    public void SetMove(Vector3 add)
    {

        if (add != Vector3.zero)
        {
            //预算的位置
            prePos = new Vector3[allModel.Count];
            for (int i = 0; i < prePos.Length; i++)
            {
                prePos[i] = -add.normalized * cardDistance * i;
            }
            if (moveDirection.Length != allModel.Count)
            {
                moveDirection = new Vector3[allModel.Count];
            }
            for (int i = 0; i < moveDirection.Length; i++)
            {
                moveDirection[i] = prePos[i] - allModel[i].transform.localPosition;
            }

            //Controller
            Vector3 curPos = transform.position;
            GameManager.instance.LimetControlZ(curPos, ref add);
            transform.position += (Vector3)add;
        }
    }



    private void MoveByDirection()
    {
        for (int i = 0; i < moveDirection.Length; i++)
        {
            if (moveDirection[i] == Vector3.zero)
                continue;

            Vector3 add = (Vector3)moveDirection[i].normalized * cardMoveMagnitude * Time.deltaTime * i;
            Vector3 endPos = allModel[i].transform.localPosition + add;
            if ((prePos[i] - endPos).normalized == (Vector3)moveDirection[i].normalized)//未超过最终点
            {
                allModel[i].transform.localPosition += add;
            }
            else
            {
                allModel[i].transform.localPosition += prePos[i] - allModel[i].transform.localPosition;
            }
            Vector3 fixPos = (Vector3)allModel[i].transform.position;
            GameManager.instance.LimetControlZ(ref fixPos);
            allModel[i].transform.position = fixPos;
        }
    }



    void OnCollisionEnter(Collision coll)
    {
        string info = "人物碰撞->";
        info += coll.gameObject.name;
        Debug.LogError(info);
    }
    



    // Update is called once per frame
    void Update()
    {
        MoveByDirection();
    }

}
