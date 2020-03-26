using System.Collections.Generic;
using UnityEngine;

public class ZoneController : MonoBehaviour
{
	Dictionary<ElementType, GenericElement> _elementsByType = new Dictionary<ElementType, GenericElement> ();
	Stack<GenericElement> _activatedElements = new Stack<GenericElement> ();

	[SerializeField] GenericElement[] _zoneElements = null;
	
	//static bool _activedZone = true;
	public  bool _activedZone = true;

//	public static bool ActivedZone => _activedZone;

	/// <summary>
	/// Inverse le change de mode d'activation. Si l'élement est activé alors il sera désactivé. vise vers ça.
	/// </summary>
	public void ChangedModeActivated()
	{
		_activedZone = true ? _activedZone = true : _activedZone = false;
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

		elementToActivate.Activate ();
		_activatedElements.Push (elementToActivate);
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

		_activatedElements.Pop ().Deactivate ();
	}

	/// <summary>
	/// Désactive tous les éléments actifs. Peut être utilisé pour reset la partie en cas de game over par exemple.
	/// </summary>
	public void CancelAll ()
	{
		while (_activatedElements.Count > 0)
			_activatedElements.Pop ().Deactivate ();
	}

	void Awake ()
	{
		InitializeDictionary ();
	}
}