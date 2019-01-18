using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour {

    public Parameters GM;

    // Use this for initialization
    public void newGame()
    {
        //Setup GameManager
        System.IO.DirectoryInfo df = new System.IO.DirectoryInfo(Application.streamingAssetsPath + "/saves");
        foreach (System.IO.FileInfo f in df.GetFiles())
        {
            System.IO.File.Delete(f.FullName);
        }
        GM.LaunchGame();
        SceneManager.LoadScene(1);//load universe
    }
    public void continueGame()
    {
        //Setup GameManager
        GM.LaunchGame();
        SceneManager.LoadScene(1);//load universe
    }

}
