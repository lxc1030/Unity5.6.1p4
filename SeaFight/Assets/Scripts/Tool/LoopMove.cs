using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class LoopMove : MonoBehaviour
{
    public Vector3 orgPos;
    public Vector3 endPos;
    public float time;

    Tweener tw;

    // Use this for initialization
    void Start()
    {
        //tw = transform.DOLocalMove(endPos, time).SetLoops(-1, LoopType.Restart).SetEase(Ease.Linear).OnComplete(Complete);
        //tw.OnStart(Complete);
    }

    // Update is called once per frame
    void Complete()
    {
        tw.ChangeStartValue(orgPos);
        transform.localPosition = (orgPos);
    }
}
