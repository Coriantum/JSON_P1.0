using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System;
using System.Collections.Generic;

// UnityWebRequest.Get example

// Access a website and use UnityWebRequest.Get to download a page.
// Also try to download a non-existing page. Display the error.

public class Main : MonoBehaviour
{

    public Response responseObject;
    void Start()
    {
        // A correct website page.
        StartCoroutine(GetRequest("https://opentdb.com/api.php?amount=10&category=15"));
    }
    

    IEnumerator GetRequest(string uri)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            string[] pages = uri.Split('/');
            int page = pages.Length - 1;

            switch (webRequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.LogError(pages[page] + ": Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    Debug.LogError(pages[page] + ": HTTP Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.Success:
                    Debug.Log(pages[page] + ":\nReceived: " + webRequest.downloadHandler.text);
                    break;

            }
            // Convertimos el formato JSON en objeto
            responseObject = JsonUtility.FromJson<Response>(webRequest.downloadHandler.text);

            Debug.Log(responseObject.results[1].category);
            Debug.Log(responseObject.results[3].type);
            Debug.Log(responseObject.results[5].correct_answer);
            Debug.Log(responseObject.results[4].question);
        }
    }

}

[Serializable]
public class Response
{
    public int response_code;
    public List<MyQuestion> results= new List<MyQuestion>();
}


[Serializable]
public class MyQuestion{
    public string category;
    public string type;
    public string dificulty;
    public string question;
    public string correct_answer;
    
    public List<string> incorrect_answers = new List<string>();
}

