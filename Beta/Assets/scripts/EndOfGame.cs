using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndOfGame : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            string currentLevelName = SceneManager.GetActiveScene().name;
            float elapsedTime = GameManager.Instance.timer;
            Analytics.Instance.CollectDataCToCTime(elapsedTime, "Endpoint");
            Debug.Log($"Time to reach endpoint: {elapsedTime} seconds");
            Analytics.Instance.Send("CToCTimeEndpoint");
            GameManager.Instance.LoadScene("menu");
        }
    }

    
    
}
