using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NameInput : MonoBehaviour
{
	[SerializeField]
	private TMP_InputField _inputField;

	[SerializeField]
	private Button _continueButton;

	private const string PlayerPrefsNameKey = "PlayerName";

	void Start()
	{
		SetUpInputField();
	}

	private void SetUpInputField()
	{
		if (PlayerPrefs.HasKey(PlayerPrefsNameKey))
		{
			_inputField.text = PlayerPrefs.GetString(PlayerPrefsNameKey);

			SetPlayerName();
		}
	}

	public void SavePlayerName()
	{
		PlayerPrefs.SetString(PlayerPrefsNameKey, _inputField.text);
	}

	public void SetPlayerName()
	{
		_continueButton.interactable = !string.IsNullOrEmpty(_inputField.text);
	}
}
