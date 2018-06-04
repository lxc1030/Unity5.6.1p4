using UnityEngine;

public class Test : MonoBehaviour
{
    public float g = 9.8f;

    public Vector3 orgPos;
    public Vector3 target;
    public float speed = 10;
    private float verticalSpeed;

    public bool isMove;

    private void Awake()
    {
        isMove = false;
    }
    [ContextMenu("发射")]
    public void Init()
    {
        time = 0;
        transform.position = orgPos;
        //
        float tmepDistance = Vector3.Distance(orgPos, target);
        float tempTime = tmepDistance / speed;
        float riseTime, downTime;
        riseTime = downTime = tempTime / 2;
        verticalSpeed = g * riseTime;
        transform.LookAt(target);

        isMove = true;
    }
    private float time;
    void Update()
    {
        if (isMove)
        {
            if (transform.position.y < target.y)
            {
                //finish
                return;
            }
            time += Time.deltaTime;
            float test = verticalSpeed - g * time;
            transform.Translate(transform.forward * speed * Time.deltaTime, Space.World);
            transform.Translate(transform.up * test * Time.deltaTime, Space.World);
        }
    }
}