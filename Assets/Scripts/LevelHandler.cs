using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelHandler : MonoBehaviour
{
    public GameObject gameLoss;
    public GameObject gameWin;
    // Start is called before the first frame update
    void Start()
    {
        if (gameLoss) gameLoss.SetActive(false);
        if (gameWin) gameWin.SetActive(false);
    }

    public void deathUI()
    {
        gameLoss.SetActive(true);
    }

    public void winUI()
    {
        gameWin.SetActive(true);
    }

    public void reloadLevel()
    {
        SceneManager.LoadScene(1);
    }
}
