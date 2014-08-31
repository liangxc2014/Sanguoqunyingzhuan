using UnityEngine;
using System.Collections;

public class SettingControl : Singleton<SettingControl>
{

    private StartMenuView m_view;

    public override void Initialize(object owner)
    {
        base.Initialize(owner);


    }
    public override void UnInitialize()
    {
        UIManager.Instance.DestroyView(Define.UINameConfig.SettingPanel);
    }

    public void OnConfirmButtonClick()
    {
        UIManager.Instance.ShowView(Define.UINameConfig.StartMenuPanel);

        UnInitialize();
    }

    public void OnReturnButtonClick()
    {
        UIManager.Instance.ShowView(Define.UINameConfig.StartMenuPanel);

        UnInitialize();
    }
}
