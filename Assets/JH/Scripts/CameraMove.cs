using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    [SerializeField]
    float speed = 5.0f;
    [SerializeField]
    float angle = 1.0f;
    public float Angle { get { return angle; } set { angle = value; } }
    float lerp = 0;

    Vector3 delta = new Vector3 (0, 2, -4);
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CamMove();
    }

    void CamMove()
    {
        if (Input.GetKey(KeyCode.A))
            lerp = Mathf.Lerp(lerp, angle, Time.deltaTime * speed);
        else if (Input.GetKey(KeyCode.D))
            lerp = Mathf.Lerp(lerp, -angle, Time.deltaTime * speed);
        else
            lerp = Mathf.Lerp(lerp, 0, Time.deltaTime * speed);

        CamBetweenWall();
    }

    void CamBetweenWall()
    {
        delta = new Vector3(lerp, delta.y, delta.z);
        RaycastHit hit;
        Vector3 dir = transform.position - transform.parent.position;

        if (Physics.Raycast(transform.parent.position, dir, out hit, delta.magnitude, LayerMask.GetMask("Wall")))
        {
            float dist = (transform.parent.position - hit.point).magnitude * 0.8f;
            transform.localPosition = delta.normalized * dist;
        }
        else
        {
            transform.localPosition = delta;
        }

        if (transform.localPosition.magnitude > delta.magnitude)
            transform.localPosition = delta;
    }
}
