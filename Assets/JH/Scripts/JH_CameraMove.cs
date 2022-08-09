using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JH_CameraMove : MonoBehaviour
{
    [SerializeField]
    float speed = 5.0f;
    [SerializeField]
    float angle = 1.0f;
    public float Angle { get { return angle; } set { angle = value; } }
    float lerp = 0;
    float yLerp = 0;
    float camDist = 0;

    JH_PlayerMove pm;
    GameObject target;
    GameObject enemy;

    Vector3 delta;
    Vector3 targetDir;
    Vector3 enemyDir;
    float camAngle;
    // Start is called before the first frame update
    void Start()
    {
        pm = transform.parent.GetComponent<JH_PlayerMove>();
        target = GameObject.Find("Enemy Camera");
        enemy = GameObject.Find("Enemy");
        delta = transform.localPosition;
        camDist = delta.magnitude;
    }

    // Update is called once per frame
    void Update()
    {
        CamMove();
        CamBetweenWall();
        CamRot();
    }

    void CamMove()
    {
        enemyDir = enemy.transform.position - transform.parent.transform.position;
        // 좌우 방향키를 누를 때 카메라를 좌우로 이동시키기
        if (InputManager.Instance.Left && pm.IsCanMove())
            lerp = Mathf.Lerp(lerp, angle, Time.deltaTime * speed);
        else if (InputManager.Instance.Right && pm.IsCanMove())
            lerp = Mathf.Lerp(lerp, -angle, Time.deltaTime * speed);
        else
            lerp = Mathf.Lerp(lerp, 0, Time.deltaTime * speed);

        // 만약 상대와 y방향이 다르면 카메라를 위아래로 옮김
        if (enemyDir.y > 0.1 && (InputManager.Instance.Front || InputManager.Instance.Left || InputManager.Instance.Back ||
            InputManager.Instance.Right || InputManager.Instance.Jump || InputManager.Instance.Guard))
        {
            yLerp = Mathf.Lerp(yLerp, 2.5f - 10 / enemyDir.magnitude , Time.deltaTime * 3);
        }
        else if (enemyDir.y < -0.1 && (InputManager.Instance.Front || InputManager.Instance.Left || InputManager.Instance.Back ||
            InputManager.Instance.Right || InputManager.Instance.Jump || InputManager.Instance.Guard))
        {
            yLerp = Mathf.Lerp(yLerp, 2.5f + 10 / enemyDir.magnitude, Time.deltaTime * 3);
        }
        else
            yLerp = Mathf.Lerp(yLerp, 2.5f, Time.deltaTime * 3);
    }

    // 캠이 벽 뒤로 갈 때 벽 앞으로 위치시키기
    void CamBetweenWall()
    {
        delta = new Vector3(lerp, yLerp, delta.z);
        RaycastHit hit;
        Vector3 dir = transform.position - transform.parent.position;

        // 만약 캠과 플레이어 사이에 벽이 있다면
        if (Physics.Raycast(transform.parent.position, dir, out hit, delta.magnitude, LayerMask.GetMask("Wall")))
        {
            // 플레이어와 벽 사이의 거리 * 0.8 만큼의 위치에 카메라를 이동시킨다.
            float dist = (transform.parent.position - hit.point).magnitude * 0.8f;
            transform.localPosition = delta.normalized * dist;
        }
        // 아니라면
        else
        {
            // CamMove()에서 구한 위치로 캠을 이동시킨다.
            transform.localPosition = delta;
        }
    }

    void CamRot()
    {
        // 상대의 y축 위치를 바라보기 위해 벡터와 각도를 계산
        targetDir = target.transform.position - transform.parent.transform.position;
        // 플레이어는 항상 상대의 y축 좌표를 바라보기 때문에 카메라는 x축을 기준으로 회전해준다.
        camAngle = Vector3.Angle(targetDir, transform.rotation * Quaternion.AngleAxis(-20, Vector3.right) * Vector3.forward);

        // 구한 각도와 카메의라 up벡터와의 내적을 이용해 각도의 음양값을 구한다.
        float sign = Mathf.Sign(Vector3.Dot(targetDir, transform.rotation * Quaternion.AngleAxis(-20, Vector3.right) * Vector3.up));
        float finalAngle = sign * camAngle;
        transform.rotation *= Quaternion.AngleAxis(-finalAngle, Vector3.right);
    }

    public void CamHit()
    {
        iTween.MoveTo(gameObject, iTween.Hash("x", lerp, "y", yLerp + 0.1f, "z", delta.z, "islocal", true,
            "time", 0.01f, "easetype", iTween.EaseType.easeOutElastic));
        transform.localPosition -= Vector3.up * 0.1f;
    }
}
