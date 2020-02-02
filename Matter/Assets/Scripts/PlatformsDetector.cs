using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformsDetector : MonoBehaviour
{
    public LayerMask PlatformMask;

    PlatformeController m_detect;

    public List<GameObject> Platforms = new List<GameObject>();

    public Vector3 boxRadius;

    private void Update()
    {
        GetInput();
    }

    public void GetInput()
    {
        Detector();
       // CheckList();

        if (Input.GetButtonDown("CallPlateform"))
            GetExtrude();

    }

    public void Detector() // Permet de detecter les plateforme entrant dans la zone et de les ajouter à une liste
    {
        RaycastHit hit;
        if (Physics.BoxCast(transform.position + new Vector3(0, 0, boxRadius.z / 3), boxRadius, transform.forward, out hit, transform.rotation, PlatformMask))
        {
            if (hit.transform.gameObject.TryGetComponent(out m_detect) && (!Platforms.Contains(hit.transform.gameObject)))
                Platforms.Add(hit.transform.gameObject);
        }
    }
    /*
    private void CheckList() //Check La list pour supprimer les plateformes inutiles
    {
        RaycastHit hit;
        if (!Physics.BoxCast(transform.position + new Vector3(0, 0, boxRadius.z / 3), boxRadius, transform.forward, out hit, transform.rotation, PlatformMask))
        {
            if(Platforms.Count > 0)
            {
                foreach (var valeur in Platforms)
                {
                    if (valeur != hit.transform.gameObject)
                        RemoveList(valeur.transform.gameObject);
                }
            }
        }
    }
    */

    private void RemoveList(GameObject valeur) // remove les éléments de la list
    {
        Platforms.Remove(valeur);
    }

    public void GetExtrude()
    {
        // position du joueur
        var playerPosition = transform.position;
        // on créé une variable "cache" de plateforme la plus proche
        GameObject closestPlatform = null;
        // on itère sur toutes les 
        foreach (var platform in Platforms)
        {
            // pour chaque plateforme, on regarde ça distance avec le player
            var distance = Vector3.Distance(playerPosition, platform.transform.position);
            // si le cache est vide OU que la distance de la plateforme lue est plus petite que la distance stockée
            if (closestPlatform == null || distance < Vector3.Distance(playerPosition, closestPlatform.transform.position))
                closestPlatform = platform; // on remplace la plateforme stockée
        }
        PlatformeController Detector = closestPlatform.GetComponent<PlatformeController>();
        //La platforme la plus proche appel la méthode d'extrude
        Detector.Detected();
    }
}
