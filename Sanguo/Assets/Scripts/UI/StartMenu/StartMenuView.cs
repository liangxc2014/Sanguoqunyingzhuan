using UnityEngine;
using System.Collections;

public class StartMenuView : MonoBehaviour 
{

    

	// Use this for initialization
	void Start () 
    {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
	
	}

    public void OnStartButtonClick()
    {

    }

    public void OnReadRecordButtonClick()
    {

    }

    public void OnLoadGeneralButtonClick()
    {

    }

    public void OnSettingButtonClick()
    {
        StartMenuControl.Instance.OnSettingButtonClick();
    }

    public void OnExitButtonClick()
    {
        Application.Quit();
    }
}
