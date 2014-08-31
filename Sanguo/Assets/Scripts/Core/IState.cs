using UnityEngine;
using System.Collections;

/// <summary>
/// 状态机接口
/// </summary>
public interface IState 
{
    string Name{ get; }

    void OnEnter();

    void OnExit();

    void Update();

    bool IfCanChangeToState(IState state);
}
 