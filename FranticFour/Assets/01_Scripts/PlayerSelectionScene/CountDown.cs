using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CountDown : MonoBehaviour
{
    [SerializeField] private int coundownTimeSec = 2;
    [SerializeField] private TextMeshPro display;
    [SerializeField] private GameObject[] controllers = new GameObject[4];
    AudioController audioController;

    private void Start()
    {
        audioController = FindObjectOfType<AudioController>();
        StartCoroutine(Countdown());
    }

    private IEnumerator Countdown()
    {
        foreach (var t in controllers)
        {
            if (t)
                Destroy(t);
        }
        audioController.MusicFadeOut();
        for (int i = coundownTimeSec; i > 0; i--)
        {
            //display.text = i.ToString();
            yield return new WaitForSeconds(1);
        }

        SceneManager.LoadScene("GardenTest");
        yield return null;
    }
}