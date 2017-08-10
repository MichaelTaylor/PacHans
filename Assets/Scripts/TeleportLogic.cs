using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportLogic : MonoBehaviour {

    //public float _xPositionOffset;
    public Transform _oppositeTeleporter;
    public TeleportLogic _teleporterLogic;
    private bool _isOccupied;

    private void Start()
    {
        _teleporterLogic = _oppositeTeleporter.gameObject.GetComponent<TeleportLogic>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (_isOccupied) return;
        TeleportPlayer(collision.gameObject); 
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!_isOccupied) return;
        _isOccupied = false;
        //Debug.Log("Not Occupied");
    }

    public void TeleportPlayer(GameObject Player)
    {
        Player.transform.position = _oppositeTeleporter.position;
        _teleporterLogic._isOccupied = true;
    }
}
