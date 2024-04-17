using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndOfGame : MonoBehaviour
{
    public GameObject window;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Collider2D otherCollider = collision;
            if (otherCollider is BoxCollider2D)
            {
                string currentLevelName = SceneManager.GetActiveScene().name;
                float elapsedTime = GameManager.Instance.timer;
                Analytics.Instance.CollectDataCToCTime(elapsedTime, "Endpoint");
                Debug.Log($"Time to reach endpoint: {elapsedTime} seconds");
                Analytics.Instance.Send("CToCTimeEndpoint");
                Analytics.Instance.ColloctDataCPPR("Endpoint");
                Debug.Log("Endpoint reached");
                Analytics.Instance.Send("CheckPointPassRateEndpoint");
            }
            GameManager.ComingFromLastLevel = true;
            window.SetActive(true);
            Time.timeScale = 0;
        }
    }

    
    
}
