using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class MainMenu : MonoBehaviour
{
    private void Start()
    {
        if (!GameManager.ComingFromLastLevel)
        {
            // The game is starting fresh, send data to Google Form
            // StartCoroutine(SendDataToGoogleForm());
        }
        else
        {
            // Coming back from the last level, reset the flag and do not send data
            GameManager.ComingFromLastLevel = false;
        }
    }

    //IEnumerator SendDataToGoogleForm()
    //{
    //    // This is the URL you got from the "Get pre-filled link" step, with your entry IDs
    //    string formUrl = "https://docs.google.com/forms/u/1/d/e/1FAIpQLSdy_pb5Bk-VKgeZo-MWq2G4pYQeeVTLHUY2FN_Hi5pBQILj6A/formResponse";
    //    WWWForm form = new WWWForm();

    //    // Assuming 'entry.xxxxxx' is the field name for your "Game Started" question
    //    // You might simply record a "1" for each game start, or a timestamp
    //    form.AddField("entry.413337513", "1");

    //    using (UnityWebRequest www = UnityWebRequest.Post(formUrl, form))
    //    {
    //        yield return www.SendWebRequest();

    //        if (www.result != UnityWebRequest.Result.Success)
    //        {
    //            Debug.LogError("Form upload failed: " + www.error);
    //        }
    //        else
    //        {
    //            Debug.Log("Game start tracked successfully.");
    //        }
    //    }
    //}
}
