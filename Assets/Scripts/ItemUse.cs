using UnityEngine;

public class ItemUse : MonoBehaviour
{
	public Pill.Effect PillEffect = Pill.Effect.Nothing;
	public bool IsHoldingChocolate = false;

	public GameObject ChocolatePrefab;
	public GameObject PillPrefab;

	public static uint PickupCooldown = 0;
	public static uint UsageCooldown = 0;

	public Collider2D SlowCollider;

	LevelMenu levelMenu;

	void Start()
	{
		var gameObject = GameObject.Find("LevelMenu");

		if (gameObject != null)
		{
			levelMenu = gameObject.GetComponent<LevelMenu>();
		}
	}

	void FixedUpdate()
	{
		if (Input.GetButton("Fire3") && IsHoldingChocolate && UsageCooldown == 0)
		{
			UseChocolate();
		}
		if (Input.GetButton("Fire4") && isHoldingPill() && UsageCooldown == 0)
		{
			UsePill();
		}
		if (PickupCooldown > 0)
		{
			PickupCooldown--;
		}
		if (UsageCooldown > 0)
		{
			UsageCooldown--;
		}
	}

	bool isHoldingPill()
	{
		return PillEffect != Pill.Effect.Nothing;
	}

	public bool HoldChocolate()
	{
		if (!IsHoldingChocolate)
		{
			IsHoldingChocolate = true;

			if (levelMenu != null)
			{
				levelMenu.SetHUDChocolate(true);
			}

			return true;
		}
		else
		{
			return false;
		}
	}

	public bool HoldPill(Pill.Effect effect)
	{
		if (PickupCooldown == 0)
		{
			PickupCooldown = 10;

			if (isHoldingPill())
			{
				Vector3 temppos = gameObject.transform.position;
				temppos.y += 2;

				Pill newPill = (Instantiate(PillPrefab, temppos, Quaternion.identity) as GameObject).GetComponent<Pill>();
				newPill.PillEffect = PillEffect;
			}

			PillEffect = effect;

			levelMenu.SetHUDPill(effect);

			return true;
		}
		else
		{
			return false;
		}     
	}

	public void UseChocolate()
	{
		UsageCooldown = 20;

		if (GetComponent<PlayerController>().IsSlowed && IsHoldingChocolate)
		{
			if (levelMenu != null)
			{
				levelMenu.SetHUDChocolate(false);
			}

			IsHoldingChocolate = false;

			Vector3 temppos = SlowCollider.transform.position;

			if (SlowCollider.GetComponent<KidController>().IsLookingRight)
			{
				temppos.x += 4;
			}
			else
			{
				temppos.x -= 4;
			}

			Instantiate(ChocolatePrefab, temppos, Quaternion.identity);

			SlowCollider.GetComponent<KidController>().HasChocolate = true;
		}
	}

	public void UsePill()
	{
		if (levelMenu != null)
		{
			levelMenu.SetHUDPill(Pill.Effect.Nothing);
		}

		UsageCooldown = 20;

		switch (PillEffect)
		{
		case Pill.Effect.Nothing:
			break;
		case Pill.Effect.DamageUp:
			GameObject.Find("Rollator").GetComponent<PunchDamage>().Damage += 2;
			break;
		case Pill.Effect.SpeedDown:
			gameObject.GetComponent<PlayerController>().SetPillSpeed(15);
			break;
		case Pill.Effect.SpeedUp:
			gameObject.GetComponent<PlayerController>().SetPillSpeed(45);
			break;
		}

		PillEffect = Pill.Effect.Nothing;
	}
}