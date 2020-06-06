using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundMenu : MonoBehaviour
{
	static SoundMenu _i;

	public static SoundMenu i
	{
		get
		{
			if (_i == null)
			{
				_i = new GameObject("SoundMenu").AddComponent<SoundMenu>().GetComponent<SoundMenu>();
			}
			return _i;
		}
	}


	public IEnumerator TestSoundMenu()
	{
		Debug.Log("UI APPEAR");
		UIAppear();
		yield return new WaitForSeconds(1);

		Debug.Log("UI DISAPPEAR");
		UIDisappear();
		yield return new WaitForSeconds(1);

		Debug.Log("UI MOVE");
		UIMove();
		yield return new WaitForSeconds(1);

		Debug.Log("UI VALIDATE");
		UIValidate();
		yield return new WaitForSeconds(1);

		Debug.Log("UI CANCEL");
		UICancel();
		yield return new WaitForSeconds(1);
	}

	public void UIMove()
	{
		SoundManager.PlaySound(SoundManager.Sound.UIMove);
	}

	public void UIAppear()
	{
		SoundManager.PlaySound(SoundManager.Sound.UIAppear);
	}

	public void UIDisappear()
	{
		SoundManager.PlaySound(SoundManager.Sound.UIDisappear);
	}

	public void UIValidate()
	{
		SoundManager.PlaySound(SoundManager.Sound.UIValidate);
	}

	public void UICancel()
	{
		SoundManager.PlaySound(SoundManager.Sound.UICancel);
	}
}
