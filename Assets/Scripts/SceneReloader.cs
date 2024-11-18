using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

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

    public void Quit()
    {
        Application.Quit();
    }
}
