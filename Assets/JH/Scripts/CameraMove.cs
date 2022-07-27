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
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.A))
        {
            lerp = Mathf.Lerp(lerp, angle, Time.deltaTime * speed);
            transform.localPosition = new Vector3(lerp, transform.localPosition.y, transform.localPosition.z);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            lerp = Mathf.Lerp(lerp, -angle, Time.deltaTime * speed);
            transform.localPosition = new Vector3(lerp, transform.localPosition.y, transform.localPosition.z);
        }
        else
        {
            lerp = Mathf.Lerp(lerp, 0, Time.deltaTime * speed);
            transform.localPosition = new Vector3(lerp, transform.localPosition.y, transform.localPosition.z);
        }
    }
}
