using UnityEngine;
using System.Collections;

public class ClientManager : MonoBehaviour {

	void Awake()
	{
        DontDestroyOnLoad(gameObject);

		Client.Instance.Initialize();
	}
	
	// Update is called once per frame
	void LateUpdate () 
    {
        Client.Instance.Update();
	}
}
