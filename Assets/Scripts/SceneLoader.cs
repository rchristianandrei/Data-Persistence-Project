using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance;

    [SerializeField] private Animator anim;
    private const string LOADING = "isLoading";

    private bool isAnimating = false;

    private void Awake()
    {
        if (Instance != null) { return; }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        anim = gameObject.GetComponent<Animator>();
    }

    public void LoadScene(string sceneName)
    {
        StartCoroutine(LoadLevel(sceneName));
    }

    IEnumerator LoadLevel(string sceneName)
    {
        anim.SetBool(LOADING, true);
        isAnimating = true;

        // Check if animation is finished
        while (isAnimating)
        {
            yield return null;
        }

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);

        while (!operation.isDone)
        {
            yield return null;
        }

        GameManager.Instance.FindStartButton();

        anim.SetBool(LOADING, false);
    }

    public void AnimationFinished()
    {
        isAnimating = false;
    }
}
