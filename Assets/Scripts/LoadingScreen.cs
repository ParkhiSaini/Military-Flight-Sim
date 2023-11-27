using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class LoadingScreen : MonoBehaviour
{
    public GameObject loadingScreen;
    public Slider loadingBar;
    public TextMeshProUGUI progressText;
    public TextMeshProUGUI progressPercent;
    public float minLoadTime = 3f; // Set the minimum load time to 3 seconds
    public GameManager gameManager;

    private bool loadingStarted = false;

    public void LoadLevel(int sceneIndex)
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        if (!loadingStarted)
        {
            StartCoroutine(LoadWithMinimumTime(sceneIndex));
        }
    }

    IEnumerator LoadWithMinimumTime(int sceneIndex)
    {
        loadingStarted = true;

        // Show the loading screen
        loadingScreen.SetActive(true);

        // Start loading the scene asynchronously
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);

        // Wait until the minimum load time has passed
        float startTime = Time.time;
        while (Time.time - startTime < minLoadTime)
        {
            yield return null;
        }

        // Continue loading while updating the loading progress
        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            loadingBar.value = progress;
            progressPercent.text = (progress * 100f).ToString("F0") + "%";

            if (progress < 0.2f)
            {
                progressText.text = "Initializing Sequence...";
            }
            else if (progress < 0.5f && progress >= 0.2f)
            {
                progressText.text = "Refueling Drone...";
            }
            else if (progress < 0.9f && progress >= 0.5f)
            {
                progressText.text = "Collecting Data for Mission...";
            }

            yield return null;
        }

        operation.completed += (x) =>
        {
            gameManager.InitializeMissionsForLevel(sceneIndex);
        };

        // Hide the loading screen after the operation is complete
        loadingScreen.SetActive(false);
    }
}
