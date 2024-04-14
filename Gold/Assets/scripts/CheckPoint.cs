using System.Collections;
using UnityEngine;
using TMPro;


public class CheckPoint : MonoBehaviour
{
    public GameObject reachText;
    private bool isNotReachedBefore = true;
    public string text_;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Collider2D otherCollider = collision;
            if (otherCollider is BoxCollider2D)
            {
                CheckPointManager.Instance.SetCheckpoint(transform.position);
                reachText.SetActive(true); // Make the text visible
                StartCoroutine(ShowAndFadeCheckpointMessage()); // Show and fade the checkpoint message
                if (isNotReachedBefore)
                {
                    isNotReachedBefore = false;
                    // Accessing the timer from GameManager and logging the time
                    float elapsedTime = GameManager.Instance.SetCheckpointTime();
                    Analytics.Instance.CollectDataCToCTime(elapsedTime, "Checkpoint");
                    Debug.Log($"Time to reach checkpoint: {elapsedTime} seconds");
                    Analytics.Instance.Send("CToCTimeCheckpoint");
                    Analytics.Instance.ColloctDataCPPR("Checkpoint");
                    Debug.Log("Checkpoint reached");
                    Analytics.Instance.Send("CheckPointPassRateCheckpoint");
                }
            }
        }
    }

    IEnumerator ShowAndFadeCheckpointMessage()
    {

        TextMeshProUGUI checkpointText = reachText.GetComponent<TextMeshProUGUI>();
        if(!isNotReachedBefore) { yield break; }
        checkpointText.text = text_; // Set the text
        checkpointText.alpha = 1; // Make sure the text is fully visible
        checkpointText.fontSize = 20; // Set the font size
        // Wait for a brief moment before starting the fade
        yield return new WaitForSeconds(0.5f);

        float duration = 2f; // Duration over which to fade out
        float startAlpha = checkpointText.alpha;

        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            float normalizedTime = t / duration;
            checkpointText.alpha = Mathf.Lerp(startAlpha, 0, normalizedTime); // Linearly interpolate alpha value over time
            yield return null;
        }

        checkpointText.alpha = 0;// Ensure the text is fully transparent at the end
        reachText.SetActive(false);
    }
}
