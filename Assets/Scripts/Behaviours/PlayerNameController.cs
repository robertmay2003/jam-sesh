using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerNameController : MonoBehaviour
{
	public TMP_Text text;

	public string Name
	{
		get => _name;
		set
		{
			_name = value;
			text.text = _name;
		}
	}

	private string _name;
	private PlayerColorController _colorController;

	void Start()
	{
		_colorController = GetComponent<PlayerColorController>();
	}

	private void Update()
	{
		text.color = _colorController.color;
	}

	public void ShowName()
	{
		text.gameObject.SetActive(true);

	}

	public void HideName()
	{
		text.gameObject.SetActive(false);
	}

}
