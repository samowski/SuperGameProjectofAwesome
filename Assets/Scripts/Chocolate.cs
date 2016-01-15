using UnityEngine;

public class Chocolate : MonoBehaviour
{
	Collider2D grannyCollider;
	ItemUse itemUse;

	void OnTriggerStay2D(Collider2D other)
	{
		if (Input.GetButtonUp("Fire2") && other == grannyCollider)
		{
			bool pickedUp = itemUse.HoldChocolate();

			if (pickedUp)
			{
				Destroy(gameObject);
			}
		}
	}

	void Start()
	{
		var granny = GameObject.Find("Granny");

		if (granny != null)
		{
			grannyCollider = granny.GetComponent<Collider2D>();
			itemUse = granny.GetComponent<ItemUse>();
		}
	}
}