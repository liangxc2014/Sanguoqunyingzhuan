using UnityEngine;
using System.Collections;
using System.Collections.Generic;

#region 属性类
// 属性类型
public enum PropertyType
{
	None,
	SingleValueType,        //单一值
	ArrayValueType,         //数组值
	DictionaryType,         //字典值
}

//属性变化的类型
public enum PropertyChangeType
{
	UPDATE, //更新
	ADD,    //增加
	REMOVE, //删除
	MOVE    //移动
}

public class PropertyChangeEventArgs : System.EventArgs
{
	public PropertyChangeType m_changeType;    //数据变化类型
	public object m_oldValue;                  //旧数值,如果是数组增加时这个值没用
	public object m_newValue;                  //新改变的数值，如果是数组减少时这个值没用
	public int m_index;                        //如果是数组类型时，改变的位置
	
	public PropertyChangeEventArgs(PropertyChangeType changeTpye, object oldValue, object newValue, int index)
	{
		m_changeType = changeTpye;
		m_oldValue = oldValue;
		m_newValue = newValue;
		m_index = index;
	}
}
#endregion

/// <summary>
/// 属性基类
/// </summary>
public class Property 
{
    private PropertyType m_propertyType;        // 类型
    private object m_value;                     // 单一数值
    private ArrayList m_arrayValue;             // 数组值
    private Dictionary<string, object> m_dictValue; //字典值

    private Dictionary<string, Property> m_childProperty;
    
    // 属性变化回调函数类型
    public delegate void PropertyChangeDelegate(Property changedProperty, PropertyChangeEventArgs msg);
    // 回调函数列表
    public event PropertyChangeDelegate m_changeEvent;

    /// <summary>
    /// 个数
    /// </summary>
    public int Count
    {
        get
        {
            if (m_propertyType == PropertyType.SingleValueType)
            {
                Debug.LogError("Function:GetArrayValue, PropertyType Error! Current type is" + m_propertyType.ToString());
                return 0;
            }

            if (m_propertyType == PropertyType.ArrayValueType)
            {
                if (m_arrayValue != null)
                {
                    return m_arrayValue.Count;
                }
            }
            else if (m_propertyType == PropertyType.DictionaryType)
            {
                if (m_dictValue != null)
                {
                    return m_dictValue.Count;
                }
            }

            return 0;
        }
    }

    #region Public
    /// <summary>
    /// 构造函数
    /// </summary>
    public Property(PropertyType propertyType)
    {
        m_propertyType = propertyType;
    }

    /// <summary>
    /// 增加一个事件回调函数
    /// </summary>
    public void AddValueChangeEvent(PropertyChangeDelegate callback)
    {
        m_changeEvent += callback;
    }

    /// <summary>
    /// 移除一个事件回调函数
    /// </summary>
    public void RemoveValueChangeEvent(PropertyChangeDelegate callback)
    {
        m_changeEvent -= callback;
    }

    public void AddChild(string name, Property pro)
    {
        if (m_childProperty == null)
        {
            m_childProperty = new Dictionary<string, Property>();
        }
        m_childProperty.Add(name, pro);
    }

    public Property GetChild(string childname)
    {
        if (m_childProperty != null && m_childProperty.ContainsKey(childname))
        {
            return m_childProperty[childname];
        }

        return null;
    }

    #endregion

    #region 单一值属性
    public object Value
    {
        get
        {
            if (m_propertyType != PropertyType.SingleValueType)
            {
                Debug.LogError("Function:GetValue, PropertyType Error! Current type is" + m_propertyType.ToString());
                return null;
            }

            return m_value;
        }

        set
        {
            if (m_propertyType != PropertyType.SingleValueType)
            {
                Debug.LogError("Function:SetValue, PropertyType Error! Current type is" + m_propertyType.ToString());
            }

            PropertyChangeEventArgs msg = new PropertyChangeEventArgs(PropertyChangeType.UPDATE, m_value, value, -1);

            m_value = value;

            TriggerChangeEvent(msg);
        }
    }

    #endregion 单一值属性

    #region 数组值属性

    public object this[int index]
    {
        get
        {
            if (m_propertyType != PropertyType.ArrayValueType)
            {
                Debug.LogError("Function:GetArrayValue, PropertyType Error! Current type is" + m_propertyType.ToString());
                return null;
            }

            if (m_arrayValue == null || (index < 0) || (index >= m_arrayValue.Count))
            {
                Debug.LogError("Function:GetArrayValue, ArgumentOutOfRangeException!");
                return null;
            }

            return m_arrayValue[index];
        }
        set
        {
            if (m_propertyType != PropertyType.ArrayValueType)
            {
                Debug.LogError("Function:SetArrayValue, PropertyType Error! Current type is" + m_propertyType.ToString());
            }

            if (m_arrayValue == null)
            {
                Debug.LogError("Function:SetArrayValue, array is empty!");
                return;
            }

            if (index < 0 || index >= m_arrayValue.Count)
            {
                Debug.LogError("Function:SetArrayValue, ArgumentOutOfRangeException!");
                return;
            }

            PropertyChangeEventArgs msg = new PropertyChangeEventArgs(PropertyChangeType.UPDATE, m_arrayValue[index], value, index);

            m_arrayValue[index] = value;

            TriggerChangeEvent(msg);
        }
    }

    public void Add(object arrayValue)
    {
        if (m_propertyType != PropertyType.ArrayValueType)
        {
            Debug.LogError("Function:Add, PropertyType Error! Current type is" + m_propertyType.ToString());
            return;
        }

        if (m_arrayValue == null)
        {
            m_arrayValue = new ArrayList();
        }

        PropertyChangeEventArgs msg = new PropertyChangeEventArgs(PropertyChangeType.ADD, null, arrayValue, this.Count);

        m_arrayValue.Add(arrayValue);

        TriggerChangeEvent(msg);
    }

    public void Insert(int index, object arrayValue)
    {
        if (m_propertyType != PropertyType.ArrayValueType)
        {
            Debug.LogError("Function:Add, PropertyType Error! Current type is" + m_propertyType.ToString());
            return;
        }

        if (index < 0 || (index > this.Count))
        {
            Debug.LogError("Function:Remove, index input ERROR!");
            return;
        }

        if (m_arrayValue == null)
        {
            m_arrayValue = new ArrayList();
        }

        PropertyChangeEventArgs msg = new PropertyChangeEventArgs(PropertyChangeType.ADD, null, arrayValue, index);

        m_arrayValue.Insert(index, arrayValue);

        TriggerChangeEvent(msg);
    }

    public void Remove(object obj)
    {
        if (m_propertyType != PropertyType.ArrayValueType)
        {
            Debug.LogError("Function:Remove, PropertyType Error! Current type is" + m_propertyType.ToString());
            return;
        }

        if (m_arrayValue == null)
        {
            Debug.LogError("Function:Remove, Array is empty!");
            return;
        }

        int index = m_arrayValue.IndexOf(obj);

        if (index < 0 || index >= m_arrayValue.Count)
        {
            Debug.LogError("Function:Remove, index input ERROR!");
            return;
        }

        PropertyChangeEventArgs msg = new PropertyChangeEventArgs(PropertyChangeType.REMOVE, m_arrayValue[index], null, index);

        m_arrayValue.RemoveAt(index);

        TriggerChangeEvent(msg);
    }

    public void RemoveAt(int index)
    {
        if (m_propertyType != PropertyType.ArrayValueType)
        {
            Debug.LogError("Function:RemoveAt, PropertyType Error! Current type is" + m_propertyType.ToString());
            return;
        }

        if ((m_arrayValue == null) || (index < 0 || index >= m_arrayValue.Count))
        {
            Debug.LogError("Function:RemoveAt, index input ERROR!");
            return;
        }

        PropertyChangeEventArgs msg = new PropertyChangeEventArgs(PropertyChangeType.REMOVE, m_arrayValue[index], null, index);

        m_arrayValue.RemoveAt(index);

        TriggerChangeEvent(msg);
    }

    public bool Contains(object value)
    {
        if (m_propertyType != PropertyType.ArrayValueType)
        {
            Debug.LogError("Function:Contains, PropertyType Error! Current type is" + m_propertyType.ToString());
            return false;
        }

        return m_arrayValue.Contains(value);
    }

    public int IndexOf(object value)
    {
        if (m_propertyType != PropertyType.ArrayValueType)
        {
            Debug.LogError("Function:IndexOf, PropertyType Error! Current type is" + m_propertyType.ToString());
            return -1;
        }
        
        return m_arrayValue.IndexOf(value);
    }

    #endregion 数组值属性

    #region 字典值属性

    public object this[string key]
    {
        get
        {
            if (m_propertyType != PropertyType.DictionaryType)
            {
                Debug.LogError("Function:GetDictionaryValue, PropertyType Error! Current type is" + m_propertyType.ToString());
                return null;
            }

            if (m_dictValue == null || !m_dictValue.ContainsKey(key))
            {
                Debug.LogError("Function:GetDictionaryValue, is not contain key!");
                return null;
            }

            return m_dictValue[key];
        }
        set
        {
            if (m_propertyType != PropertyType.DictionaryType)
            {
                Debug.LogError("Function:SetDictionaryValue, PropertyType Error! Current type is" + m_propertyType.ToString());
            }

            if (m_dictValue == null)
            {
                Debug.LogError("Function:SetDictionaryValue, Dictionary is empty!");
                return;
            }

            if (!m_dictValue.ContainsKey(key))
            {
                Debug.LogError("Function:SetDictionaryValue, is not contain key!");
                return;
            }

            PropertyChangeEventArgs msg = new PropertyChangeEventArgs(PropertyChangeType.UPDATE, m_dictValue[key], value, -1);

            m_dictValue[key] = value;

            TriggerChangeEvent(msg);
        }
    }

    public void Add(string key, object valueOf)
    {
        if (m_propertyType != PropertyType.DictionaryType)
        {
            Debug.LogError("Function:Add, PropertyType Error! Current type is" + m_propertyType.ToString());
            return;
        }

        if (m_dictValue == null)
        {
            m_dictValue = new Dictionary<string, object>();
        }

        PropertyChangeEventArgs msg = new PropertyChangeEventArgs(PropertyChangeType.ADD, null, valueOf, -1);

        m_dictValue.Add(key, valueOf);

        TriggerChangeEvent(msg);
    }

    public void Remove(string key)
    {
        if (m_propertyType != PropertyType.DictionaryType)
        {
            Debug.LogError("Function:Remove, PropertyType Error! Current type is" + m_propertyType.ToString());
            return;
        }

        if (m_dictValue == null)
        {
            Debug.LogError("Function:Remove, Dictionary is empty!");
            return;
        }

        if (!m_dictValue.ContainsKey(key))
        {
            Debug.LogError("Function:Remove, is not contains key!");
            return;
        }

        PropertyChangeEventArgs msg = new PropertyChangeEventArgs(PropertyChangeType.REMOVE, m_dictValue[key], null, -1);

        m_dictValue.Remove(key);

        TriggerChangeEvent(msg);
    }

    public bool ContainsKey(string key)
    {
        if (m_propertyType != PropertyType.DictionaryType)
        {
            Debug.LogError("Function:Contains, PropertyType Error! Current type is" + m_propertyType.ToString());
            return false;
        }

        return m_dictValue.ContainsKey(key);
    }

    #endregion 字典值属性

    public void Clear()
    {
        if (m_propertyType == PropertyType.SingleValueType)
        {
            Debug.LogError("Function:Clear, PropertyType Error! Current type is" + m_propertyType.ToString());
            return;
        }

        if (m_propertyType == PropertyType.ArrayValueType)
        {
            for (int i = Count - 1; i >= 0; i--)
            {
                RemoveAt(i);
            }
        }
        else if (m_propertyType == PropertyType.DictionaryType)
        {
            IEnumerator keyEnumerator = m_dictValue.Keys.GetEnumerator();
            while (keyEnumerator.MoveNext())
            {
                Remove(keyEnumerator.Current);
            }
        }
    }

    private void TriggerChangeEvent(PropertyChangeEventArgs msg)
    {
        if (m_changeEvent != null)
            m_changeEvent(this, msg);
    }
}