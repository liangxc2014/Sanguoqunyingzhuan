using UnityEngine;
using System.Collections;

public class TimerManager : Singleton<TimerManager> 
{
    private GameObject m_timerObject;
    private TimerComponent m_timerComponent;

    public delegate void Callback();
    public delegate void Callback<T>(T arg);
    public delegate void CallbackWithArgs(params object[] args);

    public override void Initialize() 
    {
        if (m_timerObject == null)
        {
            m_timerObject = new GameObject("_TimerManager");
            m_timerComponent = m_timerObject.AddComponent<TimerComponent>();
            m_timerObject.hideFlags = HideFlags.HideInHierarchy;
            MonoBehaviour.DontDestroyOnLoad(m_timerObject);
        }
    }

    public override void UnInitialize() 
    {
        if (m_timerObject != null)
        {
            Object.Destroy(m_timerObject);
        }
    }

    public void WaitForSeconds(float seconds, Callback callback)
    {
        m_timerComponent.WaitForSecondsCallback(seconds, callback);
    }

    public void WaitForSeconds<T>(float seconds, Callback<T> callback, T arg)
    {
        m_timerComponent.WaitForSecondsCallback(seconds, callback, arg);
    }

    public void WaitForSeconds(float seconds, CallbackWithArgs callback, params object[] args)
    {
        m_timerComponent.WaitForSecondsCallback(seconds, callback, args);
    }

    public void WaitForEndOfFrame(TimerManager.Callback callback)
    {
        m_timerComponent.WaitForEndOfFrameCallback(callback);
    }

    public void WaitForEndOfFrame<T>(TimerManager.Callback<T> callback, T arg)
    {
        m_timerComponent.WaitForEndOfFrameCallback(callback, arg);
    }

    public void WaitForEndOfFrame(TimerManager.CallbackWithArgs callback, params object[] args)
    {
        m_timerComponent.WaitForEndOfFrameCallback(callback, args);
    }
}
