using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportLogic : MonoBehaviour {

    public float _xPositionOffset;
    public Transform _oppositeTeleporter;

    public TeleportLogic _teleporterLogic;

    private void Start()
    {
        _teleporterLogic = _oppositeTeleporter.gameObject.GetComponent<TeleportLogic>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        TeleportPlayer(collision.gameObject); 
    }

    public void TeleportPlayer(GameObject Player)
    {
        Player.transform.position = new Vector2(_oppositeTeleporter.position.x + _xPositionOffset, _oppositeTeleporter.position.y);
    }
}
