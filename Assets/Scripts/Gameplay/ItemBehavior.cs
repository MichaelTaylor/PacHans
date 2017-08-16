using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBehavior : MonoBehaviour {

	public enum ItemType
	{
        pellets,
		powerUp
	};
	public ItemType itemType;

    public float numPoints;
    public AudioClip _pelletSFX;
    public AudioClip _powerUpSFX;

    private void Start()
    {
        Invoke("AddToList", 0.1f);
	}

	private void AddToList()
	{
		if (itemType == ItemType.pellets)
		{
			GameplayManager.instance.pellets.Add((gameObject));
		}
        else
        {
            GameplayManager.instance.powerUpTransform.Add((gameObject.transform));
        }
	}


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        gameObject.SetActive(false);
        if (itemType == ItemType.pellets)
        {
            AddPoints();
        }
        else
        {
            PowerUp();
        }
    }

    private void AddPoints()
    {
		GameplayManager.instance.UpdateScore(numPoints);
        AudioManager.instance.PlaySFX(_pelletSFX);
        GameplayManager.instance.pellets.Remove(gameObject);
    }

    private void PowerUp()
    {
        GameplayManager.instance.PowerUp();
        AudioManager.instance.PlaySFX(_powerUpSFX);
    }
}
