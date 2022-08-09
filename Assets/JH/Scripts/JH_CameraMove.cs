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
        // �¿� ����Ű�� ���� �� ī�޶� �¿�� �̵���Ű��
        if (InputManager.Instance.Left && pm.IsCanMove())
            lerp = Mathf.Lerp(lerp, angle, Time.deltaTime * speed);
        else if (InputManager.Instance.Right && pm.IsCanMove())
            lerp = Mathf.Lerp(lerp, -angle, Time.deltaTime * speed);
        else
            lerp = Mathf.Lerp(lerp, 0, Time.deltaTime * speed);

        // ���� ���� y������ �ٸ��� ī�޶� ���Ʒ��� �ű�
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

    // ķ�� �� �ڷ� �� �� �� ������ ��ġ��Ű��
    void CamBetweenWall()
    {
        delta = new Vector3(lerp, yLerp, delta.z);
        RaycastHit hit;
        Vector3 dir = transform.position - transform.parent.position;

        // ���� ķ�� �÷��̾� ���̿� ���� �ִٸ�
        if (Physics.Raycast(transform.parent.position, dir, out hit, delta.magnitude, LayerMask.GetMask("Wall")))
        {
            // �÷��̾�� �� ������ �Ÿ� * 0.8 ��ŭ�� ��ġ�� ī�޶� �̵���Ų��.
            float dist = (transform.parent.position - hit.point).magnitude * 0.8f;
            transform.localPosition = delta.normalized * dist;
        }
        // �ƴ϶��
        else
        {
            // CamMove()���� ���� ��ġ�� ķ�� �̵���Ų��.
            transform.localPosition = delta;
        }
    }

    void CamRot()
    {
        // ����� y�� ��ġ�� �ٶ󺸱� ���� ���Ϳ� ������ ���
        targetDir = target.transform.position - transform.parent.transform.position;
        // �÷��̾�� �׻� ����� y�� ��ǥ�� �ٶ󺸱� ������ ī�޶�� x���� �������� ȸ�����ش�.
        camAngle = Vector3.Angle(targetDir, transform.rotation * Quaternion.AngleAxis(-20, Vector3.right) * Vector3.forward);

        // ���� ������ ī���Ƕ� up���Ϳ��� ������ �̿��� ������ ���簪�� ���Ѵ�.
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
