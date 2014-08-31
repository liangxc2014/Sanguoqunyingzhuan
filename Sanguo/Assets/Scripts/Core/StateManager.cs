using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class StateManager
{
    //状态基列表
    private Dictionary<string, IState> m_listState = new Dictionary<string, IState>();

    protected object m_owner;

    protected IState m_currentState;
    protected IState m_oldState;

    //当前状态
    public IState CurrentState
    {
        get
        {
            return m_currentState;
        }
        set
        {
            m_currentState = value;
        }
    }

    //前一个状态
    public IState OldState
    {
        get
        {
            return m_oldState;
        }
        set
        {
            m_oldState = value;
        }
    }

    public virtual void Initialize() 
    { 
    }

    public virtual void Initialize(object owner)
    {
        m_owner = owner;
    }

    public virtual void UnInitialize() 
    {
        CurrentState.OnExit();
    }

	//增加一个状态
    public virtual void AddState(IState state)
    {
        string name = state.Name;

        if (ContainsState(name))
        {
            m_listState[name] = state;
        }
        else
        {
            m_listState.Add(name, state);
        }
    }

    //是否存在某状态
    public virtual bool ContainsState(string name)
    {
        return m_listState.ContainsKey(name);
    }

    //删除一个状态
    public virtual void RemoveState(string name)
    {
        if (ContainsState(name))
        {
            m_listState.Remove(name);
        }
    }

    //返回一个状态
    public U GetState<U>(string stateName)
    {
        if (ContainsState(stateName))
        {
            return (U)m_listState[stateName];
        }
        
        return default(U);
    }


    /// <summary>
    /// 切换状态
    /// </summary>
    public virtual void ChangeState(string stateName)
    {
        if (!ContainsState(stateName))
        {
            Debug.LogError("Function:ChangeState, no this state!");
            return;
        }

        IState state = m_listState[stateName];

        if (m_currentState != null && !m_currentState.IfCanChangeToState(state))
        {
            return;
        }

        m_oldState = m_currentState;
        m_currentState = state;

        if (m_oldState != null)
            m_oldState.OnExit();

        m_currentState.OnEnter();
    }

    public virtual void ChangeState(IState state)
    {
        ChangeState(state.Name);
    }

    /// <summary>
    /// 更新函数
    /// </summary>
    public virtual void Update()
    {
        if (m_currentState != null)
        {
            m_currentState.Update();
        }
    }
}
