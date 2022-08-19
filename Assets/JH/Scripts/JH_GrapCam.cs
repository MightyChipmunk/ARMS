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

}
