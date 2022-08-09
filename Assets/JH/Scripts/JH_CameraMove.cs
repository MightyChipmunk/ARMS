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
    Vector3 enemyDIr;
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
        enemyDIr = enemy.transform.position - transform.parent.transform.position;
        // �¿� ����Ű�� ���� �� ī�޶� �¿�� �̵���Ű��
        if (InputManager.Instance.Left && pm.IsCanMove())
            lerp = Mathf.Lerp(lerp, angle, Time.deltaTime * speed);
        else if (InputManager.Instance.Right && pm.IsCanMove())
            lerp = Mathf.Lerp(lerp, -angle, Time.deltaTime * speed);
        else
            lerp = Mathf.Lerp(lerp, 0, Time.deltaTime * speed);

        // ���� ���� y������ �ٸ��� ī�޶� ���Ʒ��� �ű�
        if (enemyDIr.y > 0.1 && (InputManager.Instance.Front || InputManager.Instance.Left || InputManager.Instance.Back ||
            InputManager.Instance.Right || InputManager.Instance.Jump || InputManager.Instance.Guard))
        {
            yLerp = Mathf.Lerp(yLerp, 2.5f - 10 / enemyDIr.magnitude , Time.deltaTime * 3);
        }
        else if (enemyDIr.y < -0.1 && (InputManager.Instance.Front || InputManager.Instance.Left || InputManager.Instance.Back ||
            InputManager.Instance.Right || InputManager.Instance.Jump || InputManager.Instance.Guard))
        {
            yLerp = Mathf.Lerp(yLerp, 2.5f + 10 / enemyDIr.magnitude, Time.deltaTime * 3);
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

        if (Physics.Raycast(transform.parent.position, dir, out hit, delta.magnitude, LayerMask.GetMask("Wall")))
        {
            float dist = (transform.parent.position - hit.point).magnitude * 0.8f;
            transform.localPosition = delta.normalized * dist;
        }
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

        // ���� ����� ������ ȸ������ �� ������ ������ �־����ٸ� ����� ������ ��������ŭ ȸ���Ѵ�. (�������� �������� ���� ������)
        // �Ʒ��� ȸ���ؾ� �� ��
        if (camAngle < Vector3.Angle(targetDir, transform.rotation * Quaternion.AngleAxis(-20, Vector3.right)
            * Quaternion.AngleAxis(-camAngle, Vector3.right) * Vector3.forward))
            transform.rotation *= Quaternion.AngleAxis(camAngle, Vector3.right);
        // ���� ȸ���ؾ� �� ��
        else
            transform.rotation *= Quaternion.AngleAxis(-camAngle, Vector3.right);
    }
}
