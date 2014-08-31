using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// UI管理器, 用于管理所有的游戏ui的加载,显示,查找.
/// </summary>
public class UIManager : Singleton<UIManager>
{

    /// <summary>
    /// 视图保存字典
    /// </summary>
    public Dictionary<string, GameObject> m_dicUIView = new Dictionary<string, GameObject>();

    private string m_uiRootName = "UIRoot";

    // UI摄像机物体
    private GameObject m_uiRoot;
    public GameObject UIRoot
    {
        get
        {
            if (m_uiRoot == null)
            {
                m_uiRoot = GameObject.Find(m_uiRootName);
            }

            return m_uiRoot;
        }
    }

    /// <summary>
    /// 显示一个界面，如果不在字典中就创建一个
    /// </summary>
    public GameObject ShowView(string name)
    {
        if (m_dicUIView.ContainsKey(name))
        {
            m_dicUIView[name].SetActive(true);
            return m_dicUIView[name];
        }

        GameObject go = ResourcesManager.Instance.LoadUIView(name, UIRoot);
        if (go == null)
        {
            Debug.LogError("The view is not exist! name = " + name);
            return null;
        }

        go.SetActive(true);
        m_dicUIView.Add(name, go);
        return go;
    }

    /// <summary>
    /// 隐藏但不销毁
    /// </summary>
    public void HideView(string name)
    {
        if (!m_dicUIView.ContainsKey(name))
        {
            return;
        }

        m_dicUIView[name].SetActive(false);
    }

    /// <summary>
    /// 销毁一个对象
    /// </summary>
    public void DestroyView(string name)
    {
        if (!m_dicUIView.ContainsKey(name))
        {
            return;
        }

        GameObject go = m_dicUIView[name];
        m_dicUIView.Remove(name);
        GameObject.Destroy(go);

        Resources.UnloadUnusedAssets();
    }

    public void DestroyView(GameObject view)
    {
        bool flag = false;

        IEnumerator<string> keyEnumerator = m_dicUIView.Keys.GetEnumerator();
        while (keyEnumerator.MoveNext())
        {
            if (view == m_dicUIView[keyEnumerator.Current])
            {
                flag = true;
                break;
            }
        }

        if (flag == false)
        {
            Debug.LogError("The view is not exist! name = " + view.name);
            return;
        }

        m_dicUIView.Remove(keyEnumerator.Current);
        GameObject.Destroy(view);
    }

    /// <summary>
    /// 销毁所有UI
    /// </summary>
    public void DestroyAllView()
    {
        IEnumerator<string> keyEnumerator = m_dicUIView.Keys.GetEnumerator();
        while (keyEnumerator.MoveNext())
        {
            GameObject go = m_dicUIView[keyEnumerator.Current];
            GameObject.Destroy(go);
        }

        m_dicUIView.Clear();
    }
}