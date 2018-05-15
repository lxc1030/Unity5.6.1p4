using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowParticle : MonoBehaviour
{
    private float curTime = 0;
    public PreLoadType type;
    public float desTime;
    private bool isPool;

    public void Awake()
    {
        Renderer[] allR = GetComponentsInChildren<Renderer>();
        for (int i = 0; i < allR.Length; i++)
        {
            allR[i].sortingLayerName = nameof(RenderType.ParticleSystem);
        }
    }

    private void OnEnable()
    {
        curTime = 0;
        isPool = false;
    }


    void PoolDestroy()
    {
        PoolManager.instance.SetPoolObjByType(type, gameObject);
    }


    // Update is called once per frame
    void Update()
    {
        curTime += Time.deltaTime;
        if (curTime > desTime && !isPool)
        {
            isPool = true;
            PoolDestroy();
        }
    }
}
