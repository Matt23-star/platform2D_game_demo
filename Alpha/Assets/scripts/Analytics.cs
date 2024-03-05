using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class Analytics : MonoBehaviour
{
    
    public static Analytics Instance { get; private set; }
    // Death Analytics declare
    
    [HideInInspector] public int numberOfDeathLv0;
    [HideInInspector] public int numberOfDeathLv1;
    [HideInInspector] public int numberOfDeathLv2;
    private string level1Death = "entry.366340186";
    private string level2Death = "entry.1882149854";
    private string level3Death = "entry.2135360581";

    // the number of HP lost
    [HideInInspector] public int HPRemainLv0;
    [HideInInspector] public int HPRemainLv1;
    [HideInInspector] public int HPRemainLv2;
    private string level1HP = "entry.636490887";
    private string level2HP = "entry.431031045";
    private string level3HP = "entry.384606130";

    // time to beat each level
    [HideInInspector] public float timeLv0;
    [HideInInspector] public float timeLv1;
    [HideInInspector] public float timeLv2;
    private string level1Time = "entry.1788654737";
    private string level2Time = "entry.1743989579";
    private string level3Time = "entry.1318131020";

    // number of enemy kills
    [HideInInspector] public int killsLv0;
    [HideInInspector] public int killsLv1;
    [HideInInspector] public int killsLv2;
    private string level1Kills = "entry.440537020";
    private string level2Kills = "entry.649278256";
    private string level3Kills = "entry.1912742675";


    void Awake()
    {
        Debug.Log("Analytics Awake called");
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Debug.Log("Another instance of Analytics found. Destroying this one.");
            Destroy(gameObject);
        }
        
    }

    public void Send()
    {
        StartCoroutine(DeathCountPost());
        StartCoroutine(HPLostPost());
        StartCoroutine(TimeTakenPost());
        StartCoroutine(EnemyKillPost());
    }
    //Death analytics
    IEnumerator DeathCountPost()
    {
        string URL = "https://docs.google.com/forms/u/1/d/e/1FAIpQLSeMOVbuwS7jL4UVSTNgvSCYh1fuFIj6KttcarSuFoFlaFVspg/formResponse";
        WWWForm form = new WWWForm();

       
        form.AddField(level1Death, numberOfDeathLv0);
        //Debug.Log("add filed level 00: " + numberOfDeathLv0);
        form.AddField(level2Death, numberOfDeathLv1);
        //Debug.Log("add filed level 01: " + numberOfDeathLv1);
        form.AddField(level3Death, numberOfDeathLv2);
        //Debug.Log("add filed level 02: " + numberOfDeathLv0);
              
        UnityWebRequest www = UnityWebRequest.Post(URL, form);
        Debug.Log($"Sending POST request to {URL} with {form.data.Length} bytes of data.");
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Error_DeathCount: " + www.error);
        }
        else
        {
            Debug.Log("DeathCount form upload complete!");
        }
    }

    IEnumerator HPLostPost()
    {
        string URL = "https://docs.google.com/forms/u/1/d/e/1FAIpQLSdCUHqznxeiZD_xkhBbUHr91ewkYHY5ac8PoqspBYsPjZjMAw/formResponse";
        WWWForm form = new WWWForm();
        
            
        form.AddField(level1HP, HPRemainLv0);
               
        form.AddField(level2HP, HPRemainLv1);
                
        form.AddField(level3HP, HPRemainLv2);
                
        
        UnityWebRequest www = UnityWebRequest.Post(URL, form);
        Debug.Log($"Sending POST request to {URL} with {form.data.Length} bytes of data.");
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Error_HPLost: " + www.error);
        }
        else
        {
            Debug.Log("HPLost form upload complete!");
        }
    }

    //  Average time it takes to beat each level
    IEnumerator TimeTakenPost()
    {
        string URL = "https://docs.google.com/forms/u/1/d/e/1FAIpQLSclflhBXU_eAxt7Bt4h0YjIPfsRmB8H-QINzgtHTcf96qcZOQ/formResponse";
        WWWForm form = new WWWForm();
        
            
        form.AddField(level1Time, timeLv0.ToString());
            
        form.AddField(level2Time, timeLv1.ToString());
                
        form.AddField(level3Time, timeLv2.ToString());
                
        
        UnityWebRequest www = UnityWebRequest.Post(URL, form);
        Debug.Log($"Sending POST request to {URL} with {form.data.Length} bytes of data.");
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Error_TimeTaken: " + www.error);
        }
        else
        {
            Debug.Log("TimeTaken form upload complete!");
        }
    }

    // Enemy kills of each level
    IEnumerator EnemyKillPost()
    {
        string URL = "https://docs.google.com/forms/u/1/d/e/1FAIpQLSf8TemkiAOiiiTUzsMa-J0LepZgPKdKHl1IB7gwsTOT-4CVaA/formResponse";
        WWWForm form = new WWWForm();

        form.AddField(level1Kills, killsLv0);

        form.AddField(level2Kills, killsLv1);

        form.AddField(level3Kills, killsLv2);

        UnityWebRequest www = UnityWebRequest.Post(URL, form);
        Debug.Log($"Sending POST request to {URL} with {form.data.Length} bytes of data.");
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Error_EnemyKill: " + www.error);
        }
        else
        {
            Debug.Log("EnemyKill form upload complete!");
        }
    }
}
