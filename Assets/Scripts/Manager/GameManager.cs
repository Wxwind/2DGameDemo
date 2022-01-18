using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

/// <summary>
/// 提供游戏的初始加载，退出，重新开始,场景切换等方法
/// </summary>
public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public int currentSceneIndex;
    public SaveData SaveData { private set; get; } = new SaveData();
    private string savePath;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
        savePath = Application.dataPath+ "/../save.txt";
        ReadDataByJson();
    }

    public void LoadNewScene(int index)
    {
        currentSceneIndex = index;
        SceneManager.LoadScene(index);
        
    }

    public void RestartCurrentScene()
    {
        int nowscene = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(nowscene);
        Time.timeScale = 1;
    }

    public void CreateSaveData()
    {
        SaveData.sceneIndex = currentSceneIndex;
        SaveData.switchstate = SwitchAbilityManager.instance.currentState;
    }

    public void CreateSaveData(int sceneIndex,SwitchState state=SwitchState.reality)
    {
        SaveData.sceneIndex = sceneIndex;
        SaveData.switchstate = SwitchAbilityManager.instance.currentState;
    }

    public void SaveDataByJson()
    {
        CreateSaveData();
        string saveJson = JsonUtility.ToJson(SaveData);
        if (!File.Exists(savePath))
        {
            File.Create(savePath).Close();
        }
        StreamWriter sw = new StreamWriter(savePath);
        sw.Write(saveJson);
        sw.Close();
    }

    public void SaveDataByJson(int sceneIndex)
    {
        CreateSaveData(sceneIndex);
        string saveJson = JsonUtility.ToJson(SaveData);
        if (!ExistSaveData())
        {
            File.Create(savePath).Close();
        }
        StreamWriter sw = new StreamWriter(savePath);
        sw.Write(saveJson);
        sw.Close();
    }

    public void ReadDataByJson()
    {
        if (!ExistSaveData())
        {
            return;
        }
        StreamReader sr = new StreamReader(savePath);
        string savaJson = sr.ReadToEnd();
        SaveData = JsonUtility.FromJson<SaveData>(savaJson);
        sr.Close();
    }

    

    public bool ExistSaveData()
    {
        return File.Exists(savePath);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
    
}



[System.Serializable]
public class SaveData{
    public int sceneIndex;
    public SwitchState switchstate;
}
