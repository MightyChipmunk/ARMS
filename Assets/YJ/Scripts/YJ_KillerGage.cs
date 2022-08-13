using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// UI ũ�⸦ 1�ʸ��� Ű���
// 15�ʰ� �Ǹ�
// �ʻ�� ��밡�� ���°��ǰ�
// QŰ�� ������ 3�ʵ��� ���ݼӵ��� �ι辿 �����ϰ�ʹ�
public class YJ_KillerGage : MonoBehaviour
{
    // ���� ũ��
    Vector3 orizinSize;
    // �帣�½ð�
    float currentTime;
    // ����ð�
    float overTime;
    // �� �̹��� ������
    Transform imageScale;
    // ������ȯ�� ���� bool��
    public bool killerModeOn = false;

    // ������ ���̱�
    bool scaleDown = false;
    // ���� ������ ���� �� ��������
    Image imagecolor;
    // ���� �ð�
    float endTime = 0;
    // �ڿ� ���� ���� �̹���
    public RectTransform blur;

    void Start()
    {
        // �����Ҷ� ������ ����
        orizinSize = new Vector3(0.1f, 0.1f, 0.1f);
        // �̹��� �����Ͽ� �� Ʈ������ �̹��� ������ �־��ֱ�
        imageScale = GetComponent<Transform>();
        // �̹��� �÷��� �����ϱ����� �̹��� ��������
        imagecolor = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        ScaleUp();
    }

    void ScaleUp()
    {
        // 1�ʸ��� UI ũ�⸦ Ű���ʹ�
        // UI ũ�⸦ Ű���
        // ��... 13�� �ɸ���?
        currentTime += Time.deltaTime;
        overTime += Time.deltaTime;
        if( overTime < 13f )
        {
            if(overTime > 10f)
            {
                blur.localScale += new Vector3(0.002f, 0.002f, 0.002f);
            }
            if (currentTime > 1f)
            {
                imageScale.localScale += new Vector3(0.0015f, 0.0015f, 0.0015f);
                if (currentTime > 1.3f)
                {
                    currentTime = 0;
                }
            }
        }
        else
        {
            imagecolor.color = Color.yellow;
            if(Input.GetKeyDown(KeyCode.Q))
            {
                killerModeOn = true;
                scaleDown = true;
            }
        }

        // �ٽ��ٿ��ֱ�
        if (scaleDown)
        {
            endTime += Time.deltaTime;
            print("-------------�������� : " + endTime);
            imageScale.localScale -= new Vector3(0.0015f, 0.0015f, 0.0015f);
            if (endTime > 3f)
            {
                imageScale.localScale = orizinSize;
                imagecolor.color = Color.white;
                endTime = 0;
                overTime = 0;
                currentTime = 0;
                blur.localScale = new Vector3(0.7f, 0.7f, 0.7f);
                killerModeOn = false;
                scaleDown = false;
            }
        }
    }

}
