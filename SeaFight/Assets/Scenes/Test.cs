using UnityEngine;

public class Test : MonoBehaviour
{
    public const float g = 9.8f;

    public GameObject target;
    public float speed = 10;
    private float verticalSpeed;
    private Vector3 moveDirection;

    private float angleSpeed;
    private float angle;

    public bool isShoot;

    [ContextMenu("发射")]
    void Init()
    {
        isShoot = true;
        float tmepDistance = Vector3.Distance(transform.position, target.transform.position);
        float tempTime = tmepDistance / speed;
        float riseTime, downTime;
        riseTime = downTime = tempTime / 2;
        verticalSpeed = g * riseTime;
        transform.LookAt(target.transform.position);

        float tempTan = verticalSpeed / speed;
        double hu = Mathf.Atan(tempTan);
        angle = (float)(180 / Mathf.PI * hu);
        transform.eulerAngles = new Vector3(-angle, transform.eulerAngles.y, transform.eulerAngles.z);
        angleSpeed = angle / riseTime;

        moveDirection = target.transform.position - transform.position;
    }
    private float time;
    void Update()
    {
        if (!isShoot)
            return;
        //if (transform.position.y < target.transform.position.y)
        //{
        //    //finish  
        //    return;
        //}
        time += Time.deltaTime;
        float test = verticalSpeed - g * time;
        transform.Translate(moveDirection.normalized * speed * Time.deltaTime, Space.World);
        transform.Translate(Vector3.up * test * Time.deltaTime, Space.World);
        float testAngle = -angle + angleSpeed * time;
        transform.eulerAngles = new Vector3(testAngle, transform.eulerAngles.y, transform.eulerAngles.z);
    }
}