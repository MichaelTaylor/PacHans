using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Flicker : MonoBehaviour {

    public Text _image;
    private float _flickerTime;

    private void Start()
    {
        _image = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update ()
    {
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
