using System;
using System.Collections;
using System.Text;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using UnityEngine;

using System.Xml;

public class XMLLoader<T> where T : class
{
	private const BindingFlags FieldBindingFlags = BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetField | BindingFlags.SetField;

	protected Dictionary<int, T> m_data;
	
	public Dictionary<int, T> Data { get { return m_data; } }
	
	public XMLLoader(string XmlName)
	{
		XmlDocument xmlDoc = new XmlDocument();
		TextAsset textAsset = (TextAsset)ResourcesManager.Instance.Load(XmlName);
        if (textAsset == null)
        {
            Debug.LogError("xml file is not exist! File:" + XmlName);
            return;
        }
        try
        {
            xmlDoc.LoadXml(textAsset.text.ToString().Trim());
        }
        catch (XmlException e)
        {
            Debug.LogError("Function:XMLLoader,File:" + XmlName + ",message:" + e.Message);
            return;
        }

        m_data = new Dictionary<int, T>();

		if (xmlDoc != null)
		{
			XmlNode root = xmlDoc.DocumentElement;
			XmlNodeList list = root.ChildNodes;
			if (list != null && list.Count > 0)
			{
				for (int i = 0; i < list.Count; i++)
				{
					XmlNode node = list[i];
                    if (node.NodeType != XmlNodeType.Element)
                    {
                        continue;
                    }
					XmlElement element = (XmlElement)node;
                    
					int id = 0;
					var info = ParseXmlParamsValue(element, ref id);
					if (m_data.ContainsKey(id))
                    {
                        Debug.LogError("XMLLoader: " + XmlName + ",重复ID:" + id);
                        continue;
                    }
					
					m_data.Add(id, info);
				}
			}
		}
	}
	
	public T GetInfoById(int id)
	{
		if (m_data.ContainsKey(id))
		{
			return m_data[id];
		}
		else
		{
            Debug.LogError("The key is not found,key=" + id);
			return null;
		}
	}

	private T ParseXmlParamsValue(XmlElement element, ref int id)
	{
		object obj = Activator.CreateInstance(typeof(T));
		IDictionary<string, FieldInfo> dicField = ReflectionFields (obj.GetType ());
		foreach (KeyValuePair<string, FieldInfo> pair in dicField)
		{
			FieldInfo field = pair.Value;
            if (!element.HasAttribute(field.Name))
            {

                continue;
            }
			if (null == field) continue;
			
			try
			{
				if(field.FieldType.IsArray)
				{
					continue;
				}
				
				object value = Convert.ChangeType(element.GetAttribute(field.Name), field.FieldType);
				field.SetValue(obj, value);
				
				if (field.Name == "ID")
					id = (int)value;
				
			}
			catch (Exception ex)
			{
				Debug.LogError(string.Format("ParseXmlParamsValue is failed! class={0}, prepertyName={1}, value={2}, targetType={3}, errorDetail:{4}", obj, field.Name, element.GetAttribute(field.Name), field.FieldType, ex.StackTrace));
			}
		}
		
		return obj as T;
	}

	/// <summary>
	/// 找出一个类型的属性;
	/// </summary>
	private IDictionary<string, FieldInfo> ReflectionFields(Type t)
	{
		FieldInfo[] fields = t.GetFields(FieldBindingFlags);
		IDictionary<string, FieldInfo> map = new Dictionary<string, FieldInfo>(fields.Length);
		for (int i = 0; i < fields.Length; i++)
		{
			FieldInfo f = fields[i];
			string name = f.Name;
			int pos;
			if (name[0] == '<' && (pos = name.IndexOf('>')) > 1)
				name = name.Substring(1, pos - 1); // Auto-Property (\<.+\>) <ab>
			map[name] = f;
		}
		
		return map;
	}
}
