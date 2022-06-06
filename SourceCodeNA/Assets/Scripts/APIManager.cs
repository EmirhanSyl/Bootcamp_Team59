using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class APIManager : MonoBehaviour
{

    public string username;

    void Start()
    {
        
    }

    
    void Update()
    {
        
    }

    public void InsertNameButton()
    {
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
            }
            else
            {
                Debug.Log(www.downloadHandler.text);
                //GirisText = www.downloadHandler.text;
                //Text Animasyonu kodlarýný yaz!
            }
        }
    }
}
