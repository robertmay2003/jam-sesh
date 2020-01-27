using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerColorController : MonoBehaviour
{

	public SpriteRenderer sprite;

	public Color color
	{
		get => _color;
		set
		{
			_color = value;
			sprite.color = _color;
		}
	}

	private Color _color;

}
