using UnityEngine;
using System.Collections;

public class SettingView : MonoBehaviour 
{


	// Use this for initialization
	void Start () 
    {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
	
	}

    public void OnConfirmButtonClick()
    {
        SettingControl.Instance.OnConfirmButtonClick();
    }

    public void OnReturnButtonClick()
    {
        SettingControl.Instance.OnReturnButtonClick();
    }
}
