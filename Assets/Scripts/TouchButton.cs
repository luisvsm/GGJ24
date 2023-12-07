using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class TouchButton : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
{
	private bool interactabe = true;
	[SerializeField] private Sprite buttonPressedImage, buttonUnPressedImage;

	[SerializeField] private UnityEvent onClickAction;
	[SerializeField] private TextMeshProUGUI butonText;
	[SerializeField] private Image background;
	[SerializeField] private AudioClip clickSound;
	private Vector3 textStartPos;
	private Vector3 TextDownPos => textStartPos + Vector3.down * 3;

	private void Awake()
	{
		textStartPos = butonText.transform.localPosition;
	}

	public bool Enabled
	{
		get { return interactabe; }
		set
		{
			if (value)
			{
				interactabe = true;
			}
			else
			{
				interactabe = false;
				background.color = Color.gray;
			}
		}
	}

	public void UpdateText(string newText)
	{
		butonText.text = newText;
	}

	public void UpdateBackgroundColour(Color color)
	{
		background.color = color;
	}

	public void OnPointerClick(PointerEventData eventData)
	{

	}

	public void OnPointerDown(PointerEventData eventData)
	{
		if (!interactabe)
		{
			return;
		}
		background.sprite = buttonPressedImage;
		AudioManager.instance.PlaySFX(clickSound);

		butonText.transform.localPosition = TextDownPos;
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		if (!interactabe)
		{
			return;
		}
		onClickAction?.Invoke();
		background.sprite = buttonUnPressedImage;

		butonText.transform.localPosition = textStartPos;
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		background.sprite = buttonUnPressedImage;

		butonText.transform.localPosition = textStartPos;
	}
}
