using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndPoint : MonoBehaviour
{
 
    public GameObject window;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Collider2D otherCollider = collision;
            if (otherCollider is BoxCollider2D)
            {
                string currentLevelName = GameManager.Instance.currentLevelName;

                if (currentLevelName.Equals("level 00"))
                {
                    // Accessing the timer from GameManager and logging the time
                    float elapsedTime = GameManager.Instance.timer;
                    Analytics.Instance.CollectDataCToCTime(elapsedTime, "Endpoint");
                    Debug.Log($"Time to reach endpoint: {elapsedTime} seconds");
                    Analytics.Instance.Send("CToCTimeEndpoint");
                    Analytics.Instance.ColloctDataCPPR("Endpoint");
                    Debug.Log("Endpoint reached");
                    Analytics.Instance.Send("CheckPointPassRateEndpoint");
                }
                else if (currentLevelName.Equals("level 01") || currentLevelName.Equals("level 02"))
                {
                    // Calculate time from last checkpoint to endpoint
                    float elapsedTimeFromCheckpoint = GameManager.Instance.timer - GameManager.Instance.checkpointTime;
                    Analytics.Instance.CollectDataCToCTime(elapsedTimeFromCheckpoint, "Endpoint");
                    Debug.Log($"Time to reach endpoint: {elapsedTimeFromCheckpoint} seconds");
                    Analytics.Instance.Send("CToCTimeEndpoint");
                    Analytics.Instance.ColloctDataCPPR("Endpoint");
                    Debug.Log("Endpoint reached");
                    Analytics.Instance.Send("CheckPointPassRateEndpoint");
                }
                

            }
            window.SetActive(true);
            Time.timeScale = 0;
        }
    }

}
