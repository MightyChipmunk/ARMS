using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JH_CamReturn : MonoBehaviour
{
    [SerializeField]
    float speed = 5.0f;
    [SerializeField]
    float angle = 1.0f;
    public float Angle { get { return angle; } set { angle = value; } }
    float xLerp = 0;
    float yLerp = 2;
    float zLerp = -1.5f;
    float hitShake = 0;
    float hitShakeE = 0;

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
    }

    // Update is called once per frame
    void LateUpdate()
    {
        
        CamMove();
        CamBetweenWall();
        CamRot();

        if (pm.hittedp || pm.Knocked)
            CamHitted();
        else
        {
            totalTime = 0;
            hitShake = 0;
        }

        if (enemy.GetComponent<JH_PlayerMove>().hittedp || enemy.GetComponent<JH_PlayerMove>().Knocked)
            CamHit();
        else
        {
            totalTimeE = 0;
            hitShakeE = 0;
        }
        
    }

    void CamMove()
    {
        enemyDir = enemy.transform.position - transform.parent.transform.position;
        // 좌우 방향키를 누를 때 카메라를 좌우로 이동시키기
        if (InputManager.Instance.Left && pm.IsCanMove())
            xLerp = Mathf.Lerp(xLerp, angle, Time.deltaTime * speed);
        else if (InputManager.Instance.Right && pm.IsCanMove())
            xLerp = Mathf.Lerp(xLerp, -angle, Time.deltaTime * speed);
        else
            xLerp = Mathf.Lerp(xLerp, 0, Time.deltaTime * speed);

        // 만약 상대와 y방향이 다르면 카메라를 위아래로 옮김
        if (enemyDir.y > 0.1 && (InputManager.Instance.Front || InputManager.Instance.Left || InputManager.Instance.Back ||
            InputManager.Instance.Right || InputManager.Instance.Jump || InputManager.Instance.Guard))
        {
            yLerp = Mathf.Lerp(yLerp, 2.3f - 5 / enemyDir.magnitude, Time.deltaTime * speed);
        }
        else if (enemyDir.y < -0.1 && (InputManager.Instance.Front || InputManager.Instance.Left || InputManager.Instance.Back ||
            InputManager.Instance.Right || InputManager.Instance.Jump || InputManager.Instance.Guard))
        {
            yLerp = Mathf.Lerp(yLerp, 2.3f + 5 / enemyDir.magnitude, Time.deltaTime * speed);
        }
        else
            yLerp = Mathf.Lerp(yLerp, 2.3f, Time.deltaTime * speed);

        if (pm.State == JH_PlayerMove.PlayerState.Grap || pm.State == JH_PlayerMove.PlayerState.Attack)
        {
            zLerp = Mathf.Lerp(zLerp, -1.2f, Time.deltaTime * speed);
        }
        else if (pm.State == JH_PlayerMove.PlayerState.Fall)
        {
            zLerp = Mathf.Lerp(zLerp, -3f, Time.deltaTime * speed);
        }
        else
        {
            zLerp = Mathf.Lerp(zLerp, -1.5f, Time.deltaTime * speed);
        }
    }

    // 캠이 벽 뒤로 갈 때 벽 앞으로 위치시키기
    void CamBetweenWall()
    {
        // 카메라의 로컬포지션
        delta = new Vector3(xLerp + hitShake + hitShakeE, yLerp + hitShake + hitShakeE, zLerp);
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
        camAngle = Vector3.Angle(targetDir, transform.rotation * Quaternion.AngleAxis(-10, Vector3.right) * Vector3.forward);

        // 구한 각도와 카메의라 up벡터와의 내적을 이용해 각도의 음양값을 구한다.
        float sign = Mathf.Sign(Vector3.Dot(targetDir, transform.rotation * Quaternion.AngleAxis(-10, Vector3.right) * Vector3.up));
        float finalAngle = sign * camAngle;
        transform.rotation *= Quaternion.AngleAxis(-finalAngle, Vector3.right);
    }

    float currentTime = 0;
    float shakeTime = 0.06f;
    float totalTime = 0;
    public void CamHitted()
    {
        currentTime += Time.deltaTime;
        totalTime += Time.deltaTime;
        if (currentTime > shakeTime * 2 && totalTime < 0.2f)
        {
            hitShake = -0.03f;
            currentTime = 0;
        }
        else if (currentTime > shakeTime && totalTime < 0.2f)
        {
            hitShake = 0.03f;
        }
        else if (totalTime > 0.2f)
        {
            hitShake = 0;
        }
    }

    float currentTimeE = 0;
    float shakeTimeE = 0.06f;
    float totalTimeE = 0;
    public void CamHit()
    {
        currentTimeE += Time.deltaTime;
        totalTimeE += Time.deltaTime;

        if (currentTimeE > shakeTimeE * 2 && totalTimeE < 0.2f)
        {
            hitShakeE = -0.03f;
            currentTimeE = 0;
        }
        else if (currentTimeE > shakeTimeE && totalTimeE < 0.2f)
        {
            hitShakeE = 0.03f;
        }
        else if (totalTimeE > 0.2f)
        {
            hitShakeE = 0;
        }
    }
}
