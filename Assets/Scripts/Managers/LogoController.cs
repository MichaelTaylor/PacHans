using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LogoController : MonoBehaviour {

    public GameObject _pixelPushersUnionLogo;
    public GameObject _realAleLogo;
    public float _logoDuration;

    public Color AleColor;

	// Use this for initialization
	private void Start ()
    {
        _pixelPushersUnionLogo.SetActive(true);
        _realAleLogo.SetActive(false);
        StartCoroutine(ShowAleLogo(_logoDuration));
    }
	
	private IEnumerator ShowAleLogo(float _seconds)
    {
        yield return new WaitForSeconds(_seconds);
        _pixelPushersUnionLogo.SetActive(false);
        _realAleLogo.SetActive(true);
        GetComponent<Camera>().backgroundColor = AleColor;
        StartCoroutine(ChangeScene(_logoDuration));
    }

    private IEnumerator ChangeScene(float _seconds)
    {
        yield return new WaitForSeconds(_seconds);
        SceneManager.LoadScene("Start Screen Scene");
    }
}
