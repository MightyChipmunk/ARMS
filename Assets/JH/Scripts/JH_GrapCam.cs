using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JH_GrapCam : MonoBehaviour
{
    Transform enemy;
    Transform player;
    Vector3 delta;
    [SerializeField]
    float angle = 30f;
    // Start is called before the first frame update
    void Start()
    {
        enemy = GameObject.Find("Enemy").transform;
        player = transform.parent.transform;
        delta = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        CamBetweenWall();
    }
    void CamBetweenWall()
    {
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

}
