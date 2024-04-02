using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class Analytics : MonoBehaviour
{
    public static Analytics Instance { get; private set; }
    // Death Analytics declare
    
    private string EKRLv2Ans;
    private string EKRLv3Ans;
    private string EKRLv2Entry = "entry.949278824";
    private string EKRLv3Entry = "entry.1014081727";
    

    // check point pass rate
    [HideInInspector] public int CPPRLv1Ans;
    [HideInInspector] public int CPPRLv2Ans1;
    [HideInInspector] public int CPPRLv2Ans2;
    [HideInInspector] public int CPPRLv3Ans1;
    [HideInInspector] public int CPPRLv3Ans2;
    [HideInInspector] public int CPPRLv4Ans;
    private string CPPRLv1Entry = "entry.1962806257";
    private string CPPRLv2Entry1 = "entry.373101319";
    private string CPPRLv2Entry2 = "entry.1818093458";
    private string CPPRLv3Entry1 = "entry.1119046907";
    private string CPPRLv3Entry2 = "entry.1211544911";
    private string CPPRLv4Entry = "entry.836668042";

    // time to beat each level
    private float timeLv1;
    private float time1Lv2;
    private float time2Lv2;
    private float time1Lv3;
    private float time2Lv3;
    private float timeLv4;
    private string level1Time = "entry.646782995";
    private string level2Time1 = "entry.991249842";
    private string level2Time2 = "entry.218118490";
    private string level3Time1 = "entry.1496015641";
    private string level3Time2 = "entry.214704749";
    private string level4Time = "entry.1592600164";

    // number of enemy kills
    private Vector2 DeathLocLv1;
    private Vector2 DeathLocLv2;
    private Vector2 DeathLocLv3;
    private Vector2 DeathLocLv4;
    private string level1DL = "entry.1104684653";
    private string level2DL = "entry.1332899930";
    private string level3DL = "entry.42495865";
    private string level4DL = "entry.1427656629";


    void Awake()
    {
        //Debug.Log("Analytics Awake called");
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

    public void Send(string s)
    {

        if (s.Equals("EnemykillingRate")) { StartCoroutine(EnemykillingRate()); }
        if (s.Equals("CheckPointPassRate")) { StartCoroutine(CheckPointPassRate()); }
        if (s.Equals("CToCTimeCheckpoint")) { StartCoroutine(CToCTime("Checkpoint")); }
        if (s.Equals("CToCTimeEndpoint")) { StartCoroutine(CToCTime("Endpoint")); }
        if (s.Equals("LocationOfDeath")) { StartCoroutine(LocationOfDeath()); }
    }
    //Death analytics
    IEnumerator EnemykillingRate()
    {
        string URL = "https://docs.google.com/forms/u/1/d/e/1FAIpQLSc3UW934jfnkVFyYoFKqwbAJ0U8d4Q2EVg-e5WUDymdTaH3bw/formResponse";
        WWWForm form = new WWWForm();

        switch (GameManager.Instance.currentLevelName)
        {
            case "level 01":
                form.AddField(EKRLv2Entry, EKRLv2Ans);
                //Debug.Log("add filed level 01: " + EKRLv2Ans);
                break;
            case "level 02":
                form.AddField(EKRLv3Entry, EKRLv3Ans);
                //Debug.Log("add filed level 02: " + EKRLv3Ans);
                break;
        }


        UnityWebRequest www = UnityWebRequest.Post(URL, form);
        //Debug.Log($"Sending POST request to {URL} with {form.data.Length} bytes of data.");
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Error_EnemykillingRate: " + www.error);
        }
        else
        {
            Debug.Log("EnemykillingRate form upload complete!");
        }
    }

    IEnumerator CheckPointPassRate()
    {
        string URL = "https://docs.google.com/forms/u/1/d/e/1FAIpQLScuDAbw4EtLU0jmL2cn434BSpVEnS2zJ9qJ4O2SJD-8Vl_tYQ/formResponse";
        WWWForm form = new WWWForm();
        
            
        form.AddField(CPPRLv1Entry, CPPRLv1Entry);
               
        form.AddField(CPPRLv2Entry1, CPPRLv2Ans1);
                
        form.AddField(CPPRLv2Entry2, CPPRLv2Ans2);

        form.AddField(CPPRLv3Entry1, CPPRLv3Entry1);

        form.AddField(CPPRLv3Entry2, CPPRLv3Entry2);

        form.AddField(CPPRLv4Entry, CPPRLv4Entry);


        UnityWebRequest www = UnityWebRequest.Post(URL, form);
        Debug.Log($"Sending POST request to {URL} with {form.data.Length} bytes of data.");
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Error_CheckPointPassRate: " + www.error);
        }
        else
        {
            Debug.Log("CheckPointPassRate form upload complete!");
        }
    }

    //  Record the time it takes between two checkpoints
    IEnumerator CToCTime(string checkpointOrEndpoint)
    {
        string URL = "https://docs.google.com/forms/u/1/d/e/1FAIpQLSdLXgnazr4zswDYlAIafn68uRogcjaqautxODUFKrlxjUgFmA/formResponse";
        WWWForm form = new WWWForm();

        switch (GameManager.Instance.currentLevelName)
        {
            case "level 00":
                form.AddField(level1Time, timeLv1.ToString());
                break;

            case "level 01":
                if(checkpointOrEndpoint.Equals("Checkpoint")) form.AddField(level2Time1, time1Lv2.ToString());
                if (checkpointOrEndpoint.Equals("Endpoint")) form.AddField(level2Time2, time2Lv2.ToString());
                break;

            case "level 02":
                if (checkpointOrEndpoint.Equals("Checkpoint")) form.AddField(level3Time1, time1Lv3.ToString());
                if (checkpointOrEndpoint.Equals("Endpoint")) form.AddField(level3Time2, time2Lv3.ToString());
                break;

            case "level 03":
                form.AddField(level4Time, timeLv4.ToString());
                break;
        }
        
        UnityWebRequest www = UnityWebRequest.Post(URL, form);
        Debug.Log($"Sending POST request to {URL} with {form.data.Length} bytes of data.");
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Error_CToCTime: " + www.error);
        }
        else
        {
            Debug.Log("CToCTime form upload complete!");
        }
    }

    // Location of Death
    IEnumerator LocationOfDeath()
    {
        string URL = "https://docs.google.com/forms/u/1/d/e/1FAIpQLScEbBZoqKhe0AU9aExqvH3hDHuOyfDd42FxqWNsSUooPdFfSQ/formResponse";
        WWWForm form = new WWWForm();
        switch (GameManager.Instance.currentLevelName)
        {
            case "level 00":
                form.AddField(level1DL, DeathLocLv1.ToString());
                break;

            case "level 01":
                form.AddField(level2DL, DeathLocLv2.ToString());
                break;

            case "level 02":
                form.AddField(level3DL, DeathLocLv3.ToString());
                break;

            case "level 03":
                form.AddField(level4DL, DeathLocLv4.ToString());
                break;
        }

        UnityWebRequest www = UnityWebRequest.Post(URL, form);
        Debug.Log($"Sending POST request to {URL} with {form.data.Length} bytes of data.");
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Error_LocationOfDeath: " + www.error);
        }
        else
        {
            Debug.Log("LocationOfDeath form upload complete!");
        }
    }

    public void CollectDataDeathLoc(Vector2 DeathLoc)
    {
        // Debug.Log("CollectDataDeathLoc started.");
        switch (GameManager.Instance.currentLevelName)
        {
            case "level 00":
                DeathLocLv1 = DeathLoc;
                Debug.Log("DeathLocLv1 recorded!");
                break;

            case "level 01":
                DeathLocLv2 = DeathLoc;
                Debug.Log("DeathLocLv2 recorded!");
                break;

            case "level 02":
                DeathLocLv3 = DeathLoc;
                Debug.Log("DeathLocLv3 recorded!");
                break;

            case "level 03":
                DeathLocLv4 = DeathLoc;
                Debug.Log("DeathLocLv4 recorded!");
                break;
        }
       // Debug.Log("CollectDataDeathLoc completed.");
    }

    public void CollectDataEnemyName(string Name)
    {
        //Debug.Log("CollectDataEnemyName started.");
        switch (GameManager.Instance.currentLevelName)
        {
            case "level 01":
                EKRLv2Ans = Name;
                Debug.Log($"EKRLv2Ans: {EKRLv2Ans}  recorded!");
                break;

            case "level 02":
                EKRLv3Ans = Name;
                Debug.Log($"EKRLv3Ans: {EKRLv3Ans} recorded!");
                break;
        }
        //Debug.Log("CollectDataEnemyName completed.");
    }

    public void CollectDataCToCTime(float time, string checkpointOrEndpoint)
    {
        switch (GameManager.Instance.currentLevelName)
        {
            case "level 00":
                if (checkpointOrEndpoint.Equals("Endpoint"))
                {
                    timeLv1 = time;
                }
                break;

            case "level 01":
                if (checkpointOrEndpoint.Equals("Checkpoint"))
                {
                    time1Lv2 = time;
                }
                else if (checkpointOrEndpoint.Equals("Endpoint"))
                {
                    time2Lv2 = time;
                }
                break;

            case "level 02":
                if (checkpointOrEndpoint.Equals("Checkpoint"))
                {
                    time1Lv3 = time;
                }
                else if (checkpointOrEndpoint.Equals("Endpoint"))
                {
                    time2Lv3 = time;
                }
                break;

            case "level 03":
                if (checkpointOrEndpoint.Equals("Endpoint"))
                {
                    timeLv4 = time;
                }
                break;

        }
    }
}
