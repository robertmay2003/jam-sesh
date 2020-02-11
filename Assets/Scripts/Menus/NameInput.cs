using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NameInput : MonoBehaviour
{
	public TMP_InputField inputField;
	public Button continueButton;

	private const string PlayerPrefsNameKey = "PlayerName";

	void Start()
	{
		SetUpInputField();
	}

	private void SetUpInputField()
	{
		if (PlayerPrefs.HasKey(PlayerPrefsNameKey))
		{
			inputField.text = PlayerPrefs.GetString(PlayerPrefsNameKey);

			SetPlayerName();
		}
	}

	public void SavePlayerName()
	{
		PlayerPrefs.SetString(PlayerPrefsNameKey, inputField.text);
	}

	public void SetPlayerName()
	{
		continueButton.interactable = !string.IsNullOrEmpty(inputField.text);
	}
}
