using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SY_Charge : MonoBehaviour
{

	[SerializeField]
	private SpriteRenderer playerRenderer;
	[SerializeField]
	private TextMeshProUGUI textHP;             // �÷��̾��� ü�� �ؽ�Ʈ (����ü��/�ִ�ü��)
	[SerializeField]
	private Slider sliderHP;            // �÷��̾��� ü�� ������
	[SerializeField]
	private Button buttonAttack;        // ���� ��ư

	private float maxHP = 100;      // �ִ� ü��
	private float currentHP;            // ���� ü��
	private float damage = 12;      // ���ݷ� (damage��ŭ ü�� ����)

	private void Awake()
	{
		currentHP = maxHP;
	}

	public void Update()
	{
		if (Input.GetButton("Jump"))
        {
			if (currentHP > 0)
			{
				currentHP += damage;
				currentHP = Mathf.Max(currentHP, 0);
				sliderHP.value = currentHP / maxHP;
				textHP.text = $"{currentHP}/{maxHP}";
				StartCoroutine("ColorAnimation");
			}

            if (currentHP <= 0)
            {
                buttonAttack.interactable = false;
            }
        }
	}

	private IEnumerator ColorAnimation()
	{
		playerRenderer.color = Color.red;

		yield return new WaitForSeconds(0.1f);

		playerRenderer.color = Color.white;
	}
}
