using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class APIManager : MonoBehaviour
{
    public Text headerText;
    public Text nameText;
    public Text catchedNameText;
    public GameObject resultsPanel;
    public GameObject multiplayerPanel;
    public string username;
    bool workForOneTime;
    void Update()
    {
        if (!workForOneTime && resultsPanel.activeSelf)
        {
            StartCoroutine(GetNames());
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (resultsPanel.activeSelf)
            {
                resultsPanel.SetActive(false);
            }
            else if (multiplayerPanel.activeSelf)
            {
                multiplayerPanel.SetActive(false);
            }

        }
    }

    public void InsertNameButton()
    {
        if (nameText.text == "")
        {
            headerText.text = "Please Give Us Name Warrior!";
            return;
        }
        username = nameText.text;
        StartCoroutine(SetName(username));
    }

    IEnumerator SetName(string username)
    {
        WWWForm form = new WWWForm();
        form.AddField("username", username);

        using (UnityWebRequest www = UnityWebRequest.Post("http://api.blackflowergames.com/BootcampLogin.php", form))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
                headerText.text = "Something Went Wrong";
            }
            else
            {
                Debug.Log(www.downloadHandler.text);
                resultsPanel.SetActive(true);
            }
        }
    }

    IEnumerator GetNames()
    {
        WWWForm form = new WWWForm();

        using (UnityWebRequest www = UnityWebRequest.Get("http://api.blackflowergames.com/BootcampGameUsers.php"))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
                catchedNameText.text = "Something Went Wrong";
            }
            else
            {
                Debug.Log(www.downloadHandler.text);
                catchedNameText.text = www.downloadHandler.text;
                workForOneTime = true;
            }
        }
    }
}
