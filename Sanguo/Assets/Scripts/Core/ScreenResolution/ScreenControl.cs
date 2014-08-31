using UnityEngine;
using System.Collections;

public class ScreenControl : Singleton<ScreenControl>
{
    private float m_screenAspectRatio;
    private float m_scaleRatio;
    private float m_functionRatio;

    /// <summary>
    /// 当前摄像机的屏幕宽高比
    /// </summary>
    public float ScreenAspectRatio
    {
        get { return m_screenAspectRatio; }
    }

    /// <summary>
    /// 当前摄像机宽高比 与 设计宽高比 之间的比例
    /// </summary>
    public float ScaleRatio
    {
        get { return m_scaleRatio; }
    }

    /// <summary>
    /// 功能界面缩放比例,以宽高比较小那一个为准
    /// </summary>
    public float FunctionRatio
    {
        get { return m_functionRatio; }
    }

	public void Initialize(float DesignScreenWidth, float DesignScreenHeight) 
    {
        m_screenAspectRatio = (float)Screen.width / (float)Screen.height;
        float designAspect = DesignScreenWidth / DesignScreenHeight;

        m_scaleRatio = m_screenAspectRatio / designAspect;

        if (m_screenAspectRatio < designAspect)
        {
            m_functionRatio = m_scaleRatio;
        }
        else
        {
            m_functionRatio = 1;
        }
    }
}
