using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SY_Charge : MonoBehaviour
{

	[SerializeField]
	private SpriteRenderer playerRenderer;
	[SerializeField]
	private TextMeshProUGUI textHP;             // 플레이어의 체력 텍스트 (현재체력/최대체력)
	[SerializeField]
	private Slider sliderHP;            // 플레이어의 체력 게이지
	[SerializeField]
	private Button buttonAttack;        // 공격 버튼

	private float maxHP = 100;      // 최대 체력
	private float currentHP;            // 현재 체력
	private float damage = 12;      // 공격력 (damage만큼 체력 감소)

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
