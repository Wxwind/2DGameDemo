using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 提供游戏的初始加载，退出，重新开始,场景切换等方法
/// </summary>
public class SceneManager : MonoBehaviour
{
    public static SceneManager instance;
    public int currentSceneIndex;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);

    }

    public void LoadNewScene(int index)
    {
        
    }
}
