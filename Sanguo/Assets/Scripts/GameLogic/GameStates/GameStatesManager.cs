using UnityEngine;
using System.Collections;

public class GameStatesManager : StateManager
{
    public override void Initialize() 
    {
        LoadingState loadingState = new LoadingState();
        StartMenuState startMenuState = new StartMenuState();

        AddState(loadingState);
        AddState(startMenuState);
    }
}
