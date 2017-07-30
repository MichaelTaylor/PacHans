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

    private void OnTriggerEnter2D(Collider2D collision)
    {
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
    }

    private void PowerUp()
    {
        GameplayManager.instance.PowerUp();
    }
}
