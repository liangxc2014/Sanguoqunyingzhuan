using UnityEngine;
using System.Collections;

public static class GlobalConfig 
{
    /// <summary>
    /// 设计像素分辨率规格
    /// </summary>
    public static float DesignScreenWidth { get { return 960; } }
    public static float DesignScreenHeight { get { return 640; } }

    /// <summary>
    /// 是否打开 Debug log
    /// </summary>
    public static bool IsDebuggingOpen { get { return true; } }

    /// <summary>
    /// 是否是编辑器模式
    /// </summary>
    public static bool IsMapEditorMode { get { return true; } }
}
