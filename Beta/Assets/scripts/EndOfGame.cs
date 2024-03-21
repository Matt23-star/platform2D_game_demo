using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndOfGame : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collectData();
            Analytics.Instance.Send();
            GameManager.Instance.LoadScene("menu");
        }
    }

    private void collectData()
    {
        switch (GameManager.Instance.currentLevelName)
        {
            case "level 00":
                Debug.Log("level 00 death count: " + GameManager.Instance.numberOfDeath);
                Analytics.Instance.numberOfDeathLv0 = GameManager.Instance.numberOfDeath;
                Debug.Log("level 00 HP Remain: " + GameManager.Instance.HPLost);
                Analytics.Instance.HPRemainLv0 = GameManager.Instance.HPLost;
                Debug.Log("level 00 enemy kills: " + GameManager.Instance.numberOfKills);
                Analytics.Instance.killsLv0 = GameManager.Instance.numberOfKills;
                Debug.Log("level 00 timer: " + GameManager.Instance.timer);
                Analytics.Instance.timeLv0 = GameManager.Instance.timer;
                break;

            case "level 01":
                Debug.Log("level 01 death count: " + GameManager.Instance.numberOfDeath);
                Analytics.Instance.numberOfDeathLv1 = GameManager.Instance.numberOfDeath;
                Debug.Log("level 01 HP Remain: " + GameManager.Instance.HPLost);
                Analytics.Instance.HPRemainLv1 = GameManager.Instance.HPLost;
                Debug.Log("level 01 enemy kills: " + GameManager.Instance.numberOfKills);
                Analytics.Instance.killsLv1 = GameManager.Instance.numberOfKills;
                Debug.Log("level 01 timer: " + GameManager.Instance.timer);
                Analytics.Instance.timeLv1 = GameManager.Instance.timer;
                break;

            case "level 02":
                Debug.Log("level 02 death count: " + GameManager.Instance.numberOfDeath);
                Analytics.Instance.numberOfDeathLv2 = GameManager.Instance.numberOfDeath;
                Debug.Log("level 02 HP Remain: " + GameManager.Instance.HPLost);
                Analytics.Instance.HPRemainLv2 = GameManager.Instance.HPLost;
                Debug.Log("level 02 enemy kills: " + GameManager.Instance.numberOfKills);
                Analytics.Instance.killsLv2 = GameManager.Instance.numberOfKills;
                Debug.Log("level 02 timer: " + GameManager.Instance.timer);
                Analytics.Instance.timeLv2 = GameManager.Instance.timer;
                break;
        }
    }
}
