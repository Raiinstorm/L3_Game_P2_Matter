using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchCharacters : MonoBehaviour
{

	[SerializeField] GameObject player_1;
	[SerializeField] GameObject player_2;

	[SerializeField] bool switchPlayer;
	// Start is called before the first frame update
	void Start()
    {
		Switching();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.P))
		{
			switchPlayer = !switchPlayer;
			Switching();
		}
    }

	void Switching()
	{
		if(switchPlayer)
		{
			player_1.SetActive(false);
			player_2.SetActive(true);
		}
		else
		{
			player_1.SetActive(true);
			player_2.SetActive(false);
		}
	}
}
