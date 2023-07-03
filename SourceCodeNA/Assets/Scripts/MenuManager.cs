using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField] GameObject multiplayerMenu;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (multiplayerMenu.activeSelf)
            {
                multiplayerMenu.SetActive(false);
            }
        }
    }

    public void PlayButton()
    {
        SceneManager.LoadScene(1);
    }

    public void MultiplayerButton()
    {
        multiplayerMenu.SetActive(true);
    }

    public void ExitButton()
    {
        Application.Quit();
    }
}
