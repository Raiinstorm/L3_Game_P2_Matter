using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputSoundMenu : MonoBehaviour
{
	float _xAxis;
	float _yAxis;
	bool _antispamMove;
	bool _switch;

	private void Update()
	{
		_xAxis = Input.GetAxisRaw("Horizontal");
		_yAxis = Input.GetAxisRaw("Vertical");

		if(_xAxis !=0 || _yAxis !=0)
		{
			if(!_antispamMove)
			{
				_antispamMove = true;
				SoundMenu.i.UIMove();
			}
		}
		else
		{
			_antispamMove = false;
		}

		if(Input.GetButtonDown("Cancel"))
		{
			SoundMenu.i.UICancel();
		}
		if(Input.GetButtonDown("Submit"))
		{
			SoundMenu.i.UIValidate();
		}
		if(Input.GetButtonDown("PauseMenu"))
		{
			if(!_switch)
			{
				SoundMenu.i.UIAppear();
			}
			else
			{
				SoundMenu.i.UIDisappear();
			}

			_switch = !_switch;
		}
	}
}
