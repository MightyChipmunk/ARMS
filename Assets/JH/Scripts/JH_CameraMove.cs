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
    float xLerp = 0;
    float yLerp = 2;
    float zLerp = -1.5f;
    float hitShake = 0;
    float hitShakeE = 0;

    Transform grapPos;
    Transform enemyGrapPos;
    Transform returnPos;
    JH_PlayerMove pm;
    YJ_Hand_left lh;
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
        lh = transform.parent.transform.Find("Left").GetComponent<YJ_Hand_left>();
        target = GameObject.Find("Enemy Camera");
        enemy = GameObject.Find("Enemy");
        delta = transform.localPosition;
        enemyGrapPos = GameObject.Find("EnemyGrapCamPos").transform;
        grapPos = GameObject.Find("GrapCamPos").transform;
        returnPos = GameObject.Find("GrapReturnPos").transform;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        delta = new Vector3(xLerp + hitShake + hitShakeE, yLerp + hitShake + hitShakeE, zLerp);
        CamMove();
        if (!pm.IsGrapped() && !enemy.GetComponent<JH_PlayerMove>().IsGrapped(true) 
            && !pm.CamReturn && !pm.Knocked && pm.State != JH_PlayerMove.PlayerState.Die)
        {
            //CamBetweenWall();
            CamRot(); 
            transform.localPosition = delta;
        }
        else if (pm.IsGrapped())
        {
            transform.position = Vector3.Lerp(transform.position, enemyGrapPos.position, Time.deltaTime * speed);
            transform.rotation = Quaternion.Slerp(transform.rotation, enemyGrapPos.rotation, Time.deltaTime * speed);
            
        }
        else if (enemy.GetComponent<JH_PlayerMove>().IsGrapped(true))
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, grapPos.localPosition
                + Vector3.up * hitShake * 30 + Vector3.up * hitShakeE * 30 + Vector3.right * hitShake * 30 + Vector3.right * hitShakeE * 30
                , Time.deltaTime * speed);
            transform.rotation = Quaternion.Slerp(transform.rotation, grapPos.rotation, Time.deltaTime * speed);
        }
        else if (pm.CamReturn)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, delta, Time.deltaTime * speed * 5);
            transform.rotation = Quaternion.Slerp(transform.rotation, returnPos.rotation, Time.deltaTime * speed * 10);
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
            yLerp = Mathf.Lerp(yLerp, 2.3f - 5 / enemyDir.magnitude , Time.deltaTime * speed);
        }
        else if (enemyDir.y < -0.1 && (InputManager.Instance.Front || InputManager.Instance.Left || InputManager.Instance.Back ||
            InputManager.Instance.Right || InputManager.Instance.Jump || InputManager.Instance.Guard))
        {
            yLerp = Mathf.Lerp(yLerp, 2.3f + 5 / enemyDir.magnitude, Time.deltaTime * speed);
        }
        else
            yLerp = Mathf.Lerp(yLerp, 2.3f, Time.deltaTime * speed);

        // 잡기 혹은 공격중이라면 카메라를 앞으로 옮김
        if (pm.State == JH_PlayerMove.PlayerState.Grap || pm.State == JH_PlayerMove.PlayerState.Attack || lh.yj_KillerGage.killerModeOn)
        {
            zLerp = Mathf.Lerp(zLerp, -1.2f, Time.deltaTime * speed);
        }
        // 점프 중이라면 카메라를 뒤로 옮김
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
        //delta = new Vector3(xLerp + hitShake + hitShakeE, yLerp + hitShake + hitShakeE, zLerp);
        //RaycastHit hit;
        //Vector3 dir = transform.position - transform.parent.position;

        // 만약 캠과 플레이어 사이에 벽이 있다면
        //if (Physics.Raycast(transform.parent.position, dir, out hit, delta.magnitude, LayerMask.GetMask("Wall")))
        //{
        //    // 플레이어와 벽 사이의 거리 * 0.8 만큼의 위치에 카메라를 이동시킨다.
        //    float dist = (transform.parent.position - hit.point).magnitude * 0.8f;
        //    transform.localPosition = delta.normalized * dist;
        //}
        //// 아니라면
        //else
        //{
        //    // CamMove()에서 구한 위치로 캠을 이동시킨다.
        //    transform.localPosition = delta;
        //}
        //transform.localPosition = delta;
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

    public IEnumerator CamHitted()
    {
        float currentTime = 0;
        hitShake = 0.015f;
        while (currentTime < 0.06f)
        {
            currentTime += Time.deltaTime;
            yield return new WaitForSeconds(0.03f);
            hitShake *= -1;
            yield return null;
        }
        hitShake = 0;
    }

    public IEnumerator CamHit()
    {
        float currentTime = 0;
        hitShakeE = 0.015f;
        while (currentTime < 0.06f)
        {
            currentTime += Time.deltaTime;
            yield return new WaitForSeconds(0.03f);
            hitShakeE *= -1;
            yield return null;
        }
        hitShakeE = 0;
    }

    public void StartCamHit()
    {
        StartCoroutine("CamHit");
    }
    public void StartCamHitted()
    {
        StartCoroutine("CamHitted");
    }
    public void StopCamHit()
    {
        StopCoroutine("CamHit");
    }
    public void StopCamHitted()
    {
        StopCoroutine("CamHitted");
    }
}
