using UnityEngine;
using System.Collections;

public class TimerComponent : MonoBehaviour 
{

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
	
	}

    /// <summary>
    /// 等待若干秒调用回调函数
    /// </summary>
    public void WaitForSecondsCallback(float seconds, TimerManager.Callback callback)
    {
        StartCoroutine(CoroutineFunction(seconds, callback));
    }

    public void WaitForSecondsCallback<T>(float seconds, TimerManager.Callback<T> callback, T arg)
    {
        StartCoroutine(CoroutineFunction(seconds, callback, arg));
    }

    public void WaitForSecondsCallback(float seconds, TimerManager.CallbackWithArgs callback, params object[] args)
    {
        StartCoroutine(CoroutineFunction(seconds, callback, args));
    }

    /// <summary>
    /// 等待一帧之后调用回调函数
    /// </summary>
    public void WaitForEndOfFrameCallback(TimerManager.Callback callback)
    {
        StartCoroutine(CoroutineFunction(callback));
    }

    public void WaitForEndOfFrameCallback<T>(TimerManager.Callback<T> callback, T arg)
    {
        StartCoroutine(CoroutineFunction(callback, arg));
    }

    public void WaitForEndOfFrameCallback(TimerManager.CallbackWithArgs callback, params object[] args)
    {
        StartCoroutine(CoroutineFunction(callback, args));
    }

    /// <summary>
    /// 回调函数调用
    /// </summary>
    IEnumerator CoroutineFunction(float seconds, TimerManager.Callback callback)
    {
        yield return new WaitForSeconds(seconds);
        
        callback();
    }

    IEnumerator CoroutineFunction<T>(float seconds, TimerManager.Callback<T> callback, T arg)
    {
        yield return new WaitForSeconds(seconds);

        callback(arg);
    }

    IEnumerator CoroutineFunction(float seconds, TimerManager.CallbackWithArgs callback, params object[] args)
    {
        yield return new WaitForSeconds(seconds);

        callback(args);
    }

    IEnumerator CoroutineFunction(TimerManager.Callback callback)
    {
        yield return new WaitForEndOfFrame();

        callback();
    }

    IEnumerator CoroutineFunction<T>(TimerManager.Callback<T> callback, T arg)
    {
        yield return new WaitForEndOfFrame();

        callback(arg);
    }

    IEnumerator CoroutineFunction(TimerManager.CallbackWithArgs callback, params object[] args)
    {
        yield return new WaitForEndOfFrame();

        callback(args);
    }
}
