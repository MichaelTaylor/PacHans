using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Flicker : MonoBehaviour {

    public Text _image;
    public bool _isFlickering;
    private float _flickerTime;

    private void Start()
    {
        _image = GetComponent<Text>();
        _isFlickering = true;
    }

    public void ReturnToNormal()
    {
        _isFlickering = false;
        _image.color = new Color(_image.color.r, _image.color.g, _image.color.b, 1);
    }

    // Update is called once per frame
    void Update ()
    {
        if (!_isFlickering) return;
        _flickerTime += 0.05f;

        if (_flickerTime % 0.2f == 0)
        {
            _flickerTime = 0;
            _image.color = new Color(_image.color.r, _image.color.g, _image.color.b, 0);
        }
        else
        {
            _image.color = new Color(_image.color.r, _image.color.g, _image.color.b, 1);
        }
	}
}
