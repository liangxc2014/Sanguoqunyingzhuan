using UnityEngine;
using System.Collections;

public class ResourcesManager : Singleton<ResourcesManager>
{


	public override void Initialize()
	{

	}

	public override void UnInitialize()
	{

	}

    public Object Load(string path)
    {

        return Resources.Load(path);
    }

    public T Load<T>(string path) where T : Object
    {
        
        return Resources.Load<T>(path);
    }

    public GameObject LoadUIView(string name, GameObject parent)
    {
        GameObject prefab = Resources.Load<GameObject>(name);

        GameObject go = GameObject.Instantiate(prefab);
		go.transform.parent = parent.transform;
		go.transform.localPosition = Vector3.zero;
		go.transform.localScale = Vector3.one;

        return go;
    }

	/// <summary>
	/// 创建一个实例，作为一个子物体;
	/// </summary>
	public static GameObject AddChild(GameObject parent, string resourcesPath)
	{
		//创建一个实例;
		string[] strSplit = resourcesPath.Split(',');
		GameObject obj = Resources.Load<GameObject>(strSplit[0]);
		if (obj == null)
		{
			Debug.LogError(string.Format("ResourcesManager AddChild ERROR! {1} is not exit!", strSplit[0]));
			return null;
		}
		
		GameObject go = GameObject.Instantiate(obj);
		go.transform.parent = parent.transform;
		go.transform.localPosition = Vector3.zero;
		go.transform.localScale = Vector3.one;

		//判断是否需要翻转;
		if (go != null && parent != null)
		{
			if (strSplit.Length >= 3)
			{
				int hFlip = strSplit[1].Equals("1") ? -1 : 1;
				int vFlip = strSplit[2].Equals("1") ? -1 : 1;

				go.transform.localScale = new Vector3(hFlip, vFlip, 1);
			}
		}

		return go;
	}
}
