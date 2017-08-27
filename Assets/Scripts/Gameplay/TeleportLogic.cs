using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportLogic : MonoBehaviour {

    //public float _xPositionOffset;
    public Transform _oppositeTeleporter;
    public TeleportLogic _teleporterLogic;
    public GameObject Player;
    public float _Threshold;
    private bool _isOccupied;

    private void Start()
    {
        _teleporterLogic = _oppositeTeleporter.gameObject.GetComponent<TeleportLogic>();
        Invoke("AddPlayer", 0.5f);
    }

    private void AddPlayer()
    {
        Player = GameplayManager.instance.player;
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
        Player.transform.position = new Vector3(_oppositeTeleporter.position.x + _teleporterLogic._Threshold, _oppositeTeleporter.position.y, _oppositeTeleporter.position.z); ;
        //_teleporterLogic._isOccupied = true;
    }
}
