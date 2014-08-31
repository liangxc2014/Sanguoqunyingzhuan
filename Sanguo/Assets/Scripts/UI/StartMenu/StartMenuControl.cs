using UnityEngine;
using System.Collections;

public class StartMenuControl : Singleton<StartMenuControl>
{
    private StartMenuView m_view;


    public override void Initialize(object owner)
    {
        base.Initialize(owner);


    }
    public override void UnInitialize() 
    {
        
        UIManager.Instance.DestroyView(Define.UINameConfig.StartMenuPanel);
    }

    public void OnSettingButtonClick()
    {
        UnInitialize();
        UIManager.Instance.ShowView(Define.UINameConfig.SettingPanel);
    }
}
