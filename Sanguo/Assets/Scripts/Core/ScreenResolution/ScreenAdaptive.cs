using UnityEngine;
using System.Collections;

public class ScreenAdaptive : MonoBehaviour 
{

    public enum Type
    {
        Background,
        Function
    }

    public Type m_type = Type.Background;

	// Use this for initialization
	void Awake () 
    {
	    switch (m_type)
        {
            case Type.Background:
                {
                    Vector3 scale = transform.localScale;
                    scale.x = scale.x * ScreenControl.Instance.ScaleRatio;
                    transform.localScale = scale;
                }
                break;
            case Type.Function:
                {
                    Vector3 scale = transform.localScale;
                    scale.x = scale.x * ScreenControl.Instance.FunctionRatio;
                    scale.y = scale.y * ScreenControl.Instance.FunctionRatio;
                    scale.z = scale.y;
                    transform.localScale = scale;
                }
                break;
        }
	}
	
	// Update is called once per frame
	void Update () 
    {
	    
	}
}
