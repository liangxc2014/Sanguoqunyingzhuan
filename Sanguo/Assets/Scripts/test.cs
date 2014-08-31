using UnityEngine;
using System.Collections;

public class test : MonoBehaviour {

	// Use this for initialization
	void Start () 
    {
        GameObject prefab = ResourcesManager.Instance.Load<GameObject>("Prefabs/UI/GameStartMenuPanel");
        GameObject go = GameObject.Instantiate(prefab);
        go.transform.parent = transform;
        
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
