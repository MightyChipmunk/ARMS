using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SY_SliderExercise : MonoBehaviour
{
	[SerializeField]
	private	Slider			slider;
	[SerializeField]
	private	TextMeshProUGUI	text;

	private void Awake()
	{
		slider.onValueChanged.AddListener(OnSliderEvent);
	}

	public void OnSliderEvent(float value)
	{
		text.text = $"Charge {value*100:F1}%";
	}
}

