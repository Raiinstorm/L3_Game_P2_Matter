using System.Collections.Generic;
using UnityEngine;

public class ZoneController : MonoBehaviour
{
	public Transform _feedbackSelection;
	public Animator _animator;

	Dictionary<ElementType, GenericElement> _elementsByType = new Dictionary<ElementType, GenericElement> ();
	[HideInInspector] public List<GenericElement> _activatedElements = new List<GenericElement> ();

	[SerializeField] GenericElement[] _zoneElements = null;
	[SerializeField] private bool _activedZone = true;

	public bool ActivedZone => _activedZone;


	/// <summary>
	/// Inverse le change de mode d'activation. Si l'élement est activé alors il sera désactivé. vise vers ça.
	/// </summary>
	public bool ChangedModeActivated()
	{
		_activedZone = !_activedZone;	
		return ActivedZone;
	}

	/// <summary>
	/// Génére le dictionnaire à partir de tous les éléments renseignés dans l'inspector.
	/// Si 2 éléments du même type existent alors un seul sera pris en compte.
	/// </summary>
	void InitializeDictionary ()
	{
		foreach (GenericElement element in _zoneElements)
		{
			if (_elementsByType.ContainsKey (element.Type))
			{
				Debug.LogError ("ZoneController ERROR: Le dictionnaire d'une zone contient déjà un élément du même type!", gameObject);
				continue;
			}

			_elementsByType.Add (element.Type, element);
		}

		Debug.Log ("ZoneController INFO: Dictionnaire généré avec " + _elementsByType.Count + " éléments.", gameObject);
	}

	/// <summary>
	/// Cette méthode renvoie l'état d'un élément. Renvoie false si l'élément n'est pas dans le dictionnaire.
	/// </summary>
	public bool CheckIfElementIsActive (ElementType type)
	{
		if (!_elementsByType.ContainsKey (type))
		{
			Debug.LogError ("ZoneController ERROR: La zone ne contient aucun élément du type " + type.ToString (), gameObject);
			return false;
		}

		return _elementsByType[type].Activated;
	}

	/// <summary>
	/// Récupère l'élément de la zone qui correspond au type donné en paramètre.
	/// Si le dictionnaire ne contient pas d'élément de ce type ou si l'élément est déjà actif, alors on ne fait rien.
	/// Sinon, on appelle la méthode Apply sur l'élément et on l'ajoute à la pile des éléments activés.
	/// </summary>
	public void ActivateElementOfType (ElementType type)
	{
		if (!_elementsByType.ContainsKey (type))
		{
			Debug.LogError ("ZoneController ERROR: La zone ne contient aucun élément du type " + type.ToString (), gameObject);
			return;
		}

		var elementToActivate = _elementsByType[type];
		
		if (elementToActivate.Activated)
		{
			Debug.Log ("ZoneController INFO: L'élément voulu est déjà activé.", gameObject);
			return;
		}

		elementToActivate.apply();

		CallGameMaster(elementToActivate);

		_activatedElements.Add (elementToActivate);
	}

	/// <summary>
	/// On regarde s'il reste des éléments actifs et si oui, on désactive le plus récent.
	/// </summary>
	public void Cancel ()
	{
		if (_activatedElements.Count == 0)
		{
			Debug.Log ("ZoneController INFO: Tous les éléments de la zone sont déjà inactifs.", gameObject);
			return;
		}
		var elementToDeactivate = _activatedElements[_activatedElements.Count - 1];

		CallGameMaster(elementToDeactivate);

		elementToDeactivate.apply();
		_activatedElements.Remove(elementToDeactivate);

	}

	/// <summary>
	/// Désactive tous les éléments actifs. Peut être utilisé pour reset la partie en cas de game over par exemple.
	/// </summary>
	public void CancelAll ()
	{
		/*while (_activatedElements.Count > 0)
			_activatedElements.Pop ().apply();*/
	}

	void Awake ()
	{
		InitializeDictionary ();
	}

	void CallGameMaster(GenericElement elementToInterract)
	{
		Extrude extrude = null;

		foreach (GenericElement element in _zoneElements)
		{
			if (element is Extrude)
			{
				extrude = (Extrude)element;
			}
		}

		if (extrude.AutoSwitch)
		{
			GameMaster.i.Exception = elementToInterract;
			GameMaster.i.Activate();
		}
	}

}