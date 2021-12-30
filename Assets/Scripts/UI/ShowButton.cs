using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ShowButton : MonoBehaviour
{
    public GameObject newGameButton;
    public GameObject ResumeGameButton;

    #region used only for button in mainMenu
    private void Start()
    {
        if (GameManager.instance.ExistSaveData())
        {
            ResumeGameButton.SetActive(true);
        }
        else ResumeGameButton.SetActive(false);
    }
    public void NewGame()
    {
        if (GameManager.instance.ExistSaveData())
        {
            //弹出提示框是否覆盖现有存档进行新游戏？
        }
        GameManager.instance.SaveDataByJson(1);
        GameManager.instance.LoadNewScene(1);
    }

    public void ResumeGame()
    {
        GameManager.instance.ReadDataByJson();
        GameManager.instance.LoadNewScene(GameManager.instance.SaveData.sceneIndex);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
    #endregion
}
