using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class GlareEffect : MonoBehaviour
{
	PostProcessVolume volume;
	ColorGrading colorGrading;

	public GameObject MainCamera;

	[Header("Attributs")]
	[SerializeField] float exposureLerpSpeed;
	[SerializeField] float timeGlare;
	[SerializeField] float darknessIntensity;
	[SerializeField] float lightnessIntensity;


	float exposureSave;
	float exposureGoal;

	bool activated;

	IEnumerator inObscurity;
	IEnumerator inLight;

	private void Start()
	{
		volume = MainCamera.GetComponent<PostProcessVolume>();
		volume.profile.TryGetSettings<ColorGrading>(out colorGrading);
		exposureSave = colorGrading.postExposure.value;

		exposureGoal = exposureSave;
	}

	private void Update()
	{
		if(activated)
		{
			Debug.Log(exposureGoal);
			LerpExposure();
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if(other.gameObject.tag == "Character")
		{
			ResetCoroutines();
			StartCoroutine(inObscurity);
		}
	}
	private void OnTriggerExit(Collider other)
	{
		if (other.gameObject.tag == "Character")
		{
			ResetCoroutines();
			StartCoroutine(inLight);
		}
	}

	void LerpExposure()
	{
		colorGrading.postExposure.value = Mathf.Lerp(colorGrading.postExposure.value, exposureGoal, exposureLerpSpeed * Time.deltaTime);
	}

	IEnumerator InObscurity()
	{
		activated = true;

		exposureGoal = darknessIntensity;

		yield return new WaitForSeconds(timeGlare);

		exposureGoal = darknessIntensity/2;
	}

	IEnumerator InLight()
	{
		exposureGoal = lightnessIntensity;

		yield return new WaitForSeconds(timeGlare);

		exposureGoal = exposureSave;

		yield return new WaitForSeconds(3f);

		activated = false;
	}

	void ResetCoroutines()
	{
		StopAllCoroutines();

		inLight = InLight();
		inObscurity = InObscurity();
	}
}
