using UnityEngine;
using System.Collections;

/// <summary>
/// µ¥Àý
/// </summary>
public class Singleton<T> where T : new()
{
    protected object m_owner;

    static readonly object ms_padlock = new object();
    private static T ms_instance;
    public static T Instance
    {
        get
        {
            if (ms_instance == null)
            {
				lock (ms_padlock)
				{
                    ms_instance = new T();
                }
            }

            return ms_instance;
        }  
    }

	public virtual void Initialize() { }
    public virtual void Initialize(object owner)
    {
        m_owner = owner;
    }
	public virtual void UnInitialize() { }
}