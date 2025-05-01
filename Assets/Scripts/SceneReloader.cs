using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using static NetworkManagerHKK;
using TMPro;

public class SceneReloader: MonoBehaviour {

    public void ReloadScene()
    {
        IDFactory.ResetIDs();
        IDHolder.ClearIDHoldersList();
        Command.CommandQueue.Clear();
        Command.CommandExecutionComplete();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void LoadScene(string SceneName)
    {
        SceneManager.LoadScene(SceneName);
    }

    public void LoadSceneMultiPlayer(string SceneName)
    {
        //for further development
        SceneManager.LoadScene(SceneName);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
