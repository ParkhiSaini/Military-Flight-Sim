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

    public void LoadLevel(int sceneIndex)
    {
        StartCoroutine(LoadAsynchronously(sceneIndex));
    }

    IEnumerator LoadAsynchronously(int sceneIndex){
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);
        loadingScreen.SetActive(true);

        while(!operation.isDone){
            float progress = Mathf.Clamp01(operation.progress / .9f);
            loadingBar.value = progress;
            progressPercent.text = progress * 100f + "%";
            if(progress < 0.2f){
                progressText.text = "Initializing Sequence...";
            }
            else if(progress < 0.5f && progress > 0.2f){
                progressText.text = "Refuling Drone...";
            }
            else if(progress < 0.9f && progress > 0.5f){
                progressText.text = "Collecting Data for Mission...";
            }
            yield return null;
        }
    }
}
