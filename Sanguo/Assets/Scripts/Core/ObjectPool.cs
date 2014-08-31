using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 对象池管理器
/// </summary>
public class ObjectPool
{
    private int m_minObjCount;      //最小数量
    private int m_maxObjCount;      //最大数量

    public List<GameObject> m_listObject;   //物体列表缓存
    public List<bool> m_listState;          //状态标志是否正在使用

    public delegate GameObject CreateObjectDelegate();  //创建一个新物体时的回调函数
    private CreateObjectDelegate m_createObjectFunc;    

    /// <summary>
    /// 对象池大小
    /// </summary>
    public int PoolObjectCount
    {
        get 
        {
            return m_listObject.Count;
        }
    }

    /// <summary>
    /// 当前使用物体的个数
    /// </summary>
    public int CurrentUsedCount
    {
        get
        {
            int count = 0;
            for (int i = 0; i < m_listState.Count; i++)
            {
                if (m_listState[i])
                {
                    count++;
                }
            }

            return count;
        }
    }

    /// <summary>
    /// 初始化
    /// </summary>
    public virtual void Initialize(int min, int max, CreateObjectDelegate createObjectFunc)
    {
        if (m_listObject != null)
        {
            DestroyAll();
        }

        m_listObject = new List<GameObject>();
        m_listState = new List<bool>();

        m_minObjCount = min;
        m_maxObjCount = max;
        m_createObjectFunc = createObjectFunc;

        // 初始时需要创建
        for (int i = 0; i < m_minObjCount; i++)
        {
            CreateOneObject();
        }
    }

    /// <summary>
    /// 反初始化
    /// </summary>
    public virtual void UnInitialize()
    {
        DestroyAll();
    }

    /// <summary>
    /// 创建新物体
    /// </summary>
    public virtual int CreateOneObject()
    {
        if (m_createObjectFunc == null)
        {
            Debug.LogError("The m_createObjectFunc is null");
            return 0;
        }

        GameObject go = m_createObjectFunc();

        go.SetActive(false);

        m_listObject.Add(go);
        m_listState.Add(false);

        return (PoolObjectCount - 1);
    }

    /// <summary>
    /// 销毁一个物体
    /// </summary>
    public virtual void DestroyOneObject(GameObject go)
    {
        if (go == null)
        {
            Debug.LogError("The object is null");
            return;
        }

        int index = m_listObject.IndexOf(go);

        if (index < 0)
        {
            Debug.LogError("The object is not in the list");
            return;
        }

        m_listObject.Remove(go);
        m_listState.RemoveAt(index);
        GameObject.Destroy(go);
    }

    /// <summary>
    /// 清空
    /// </summary>
    public virtual void DestroyAll()
    {
        int count = PoolObjectCount;
        for (int i = count - 1; i >= 0; i--)
        {
            GameObject go = m_listObject[i];
            if (go != null)
            {
                DestroyOneObject(go);
            }
        }

        m_listObject.Clear();
        m_listState.Clear();

        m_listObject = null;
        m_listState = null;
    }

    /// <summary>
    /// 从对象池里取一个物体出来
    /// </summary>
    public virtual GameObject GetObject()
    {
        if (PoolObjectCount == 0)
        {
            Debug.LogError("The pool is null;");
            return null;
        }
        
        int index = -1;

        //把没有用过的物体存在一个列表里
        List<int> listIndex = new List<int>();

        //寻找没有正在使用的物体
        for (int i = 0; i < m_listState.Count; i++)
        {
            if (m_listState[i] == false)
            {
                listIndex.Add(i);
            }
        }

        //如果没有找到则新创建一个
        if (listIndex.Count == 0)
        {
            if (PoolObjectCount < m_maxObjCount)
            {
                index = CreateOneObject();
            }
            else
            {
                //如果物体数量超过了最大数,则随机返回一个
                index = Random.Range(0, PoolObjectCount);
            }
        }
        else
        {
            //从列表里面随机挑一个出来
            index = listIndex[Random.Range(0, listIndex.Count)];
        }

        m_listState[index] = true;

        GameObject go = m_listObject[index];
        go.SetActive(true);

        return go;
    }

    /// <summary>
    /// 归还一个物体给对象池
    /// </summary>
    public virtual void GiveBackObject(GameObject go)
    {
        if (PoolObjectCount > m_minObjCount)
        {
            DestroyOneObject(go);
        }
        else
        {
            int index = m_listObject.IndexOf(go);
            if (index < 0)
            {
                Debug.LogError("The object is not in the list");
                return;
            }

            go.SetActive(false);
            m_listState[index] = false;
        }
    }

    /// <summary>
    /// 全部重置
    /// </summary>
    public virtual void GiveBackAllObjects()
    {
        for (int i = 0; i < PoolObjectCount; i++)
        {
            if (m_listState[i])
            {
                GiveBackObject(m_listObject[i]);
            }
        }
    }
}
