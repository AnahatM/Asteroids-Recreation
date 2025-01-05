using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

/// <summary>
/// Used for all scene loading transitions
/// </summary>
public class SceneFader : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Image fadeImage;

    [Header("Transition Values")]
    [SerializeField] private AnimationCurve fadeAnimationCurve;

    private void Start()
    {
        // Smoothly transition in if starting from a scene
        StartCoroutine(FadeIn());
    }

    /// <summary>
    /// Fades to a specified scene, given the scene name
    /// </summary>
    /// <param name="targetScene">Exact string of scene name</param>
    public void FadeTo(string targetScene)
    {
        StartCoroutine(FadeOut(targetScene));
    }

    /// <summary>
    /// Fades to scene based on index in Unity build settings;
    /// Level scenes in a sequence should just be +1 index.
    /// </summary>
    /// <param name="targetSceneIndex">The scene's index in the build settings.</param>
    public void FadeTo(int targetSceneIndex)
    {
        StartCoroutine(FadeOut(targetSceneIndex));
    }

    private IEnumerator FadeOut(string targetScene)
    {
        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime;

            float a = fadeAnimationCurve.Evaluate(t);

            fadeImage.color = new Color(255, 255, 255, a);

            yield return 0;
        }

        SceneManager.LoadScene(targetScene);
    }

    private IEnumerator FadeOut(int targetSceneIndex)
    {
        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime;

            float a = fadeAnimationCurve.Evaluate(t);

            fadeImage.color = new Color(255, 255, 255, a);

            yield return 0;
        }

        SceneManager.LoadScene(targetSceneIndex);
    }

    private IEnumerator FadeIn()
    {
        float t = 1f;

        while (t > 0f)
        {
            t -= Time.deltaTime;

            float a = fadeAnimationCurve.Evaluate(t);

            fadeImage.color = new Color(255, 255, 255, a);

            yield return 0;
        }
    }
}
