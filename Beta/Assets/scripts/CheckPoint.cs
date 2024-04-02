using System.Collections;
using UnityEngine;
using TMPro;


public class CheckPoint : MonoBehaviour
{
    public TextMeshProUGUI checkpointText;
    private bool isNotReachedBefore = true;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // CheckPointManager.Instance.SetCheckpoint(transform.position, GameObject.FindGameObjectWithTag("Player").GetComponent<SpriteRenderer>().color);
            CheckPointManager.Instance.SetCheckpoint(transform.position);
            StartCoroutine(ShowAndFadeCheckpointMessage()); // Show and fade the checkpoint message
            if (isNotReachedBefore)
            {
                isNotReachedBefore=false;
                // Accessing the timer from GameManager and logging the time
                float elapsedTime = GameManager.Instance.SetCheckpointTime();
                Analytics.Instance.CollectDataCToCTime(elapsedTime, "Checkpoint");
                Debug.Log($"Time to reach checkpoint: {elapsedTime} seconds");
                Analytics.Instance.Send("CToCTimeCheckpoint");
            }
        }
    }

    IEnumerator ShowAndFadeCheckpointMessage()
    {
        if(!isNotReachedBefore) { yield break; }
        checkpointText.text = "Checkpoint reached!"; // Set the text
        checkpointText.alpha = 1; // Make sure the text is fully visible

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

        checkpointText.alpha = 0; // Ensure the text is fully transparent at the end
    }
}
