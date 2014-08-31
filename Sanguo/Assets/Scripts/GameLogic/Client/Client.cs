using UnityEngine;
using System.Collections;

public class Client : Singleton<Client> 
{
	public override void Initialize()
	{
		XMLManager.Instance.LoadConfigs();

        TimerManager.Instance.Initialize();
        GamePublic.Instance.Initialize();

        EnterState();
	}

	public override void UnInitialize()
	{
        GamePublic.Instance.GameStatesManager.UnInitialize();
        TimerManager.Instance.UnInitialize();
	}

    public void Update()
    {
        GamePublic.Instance.GameStatesManager.Update();
    }

    private void EnterState()
    {
        GamePublic.Instance.GameStatesManager.ChangeState(typeof(StartMenuState).Name);
    }
}
