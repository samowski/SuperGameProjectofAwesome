using UnityEngine;
using System;
using System.Collections;

public class Pill : MonoBehaviour
{
    static Material[] materials = null;

    public static Material getMaterial(Effect effect)
    {
        return materials[(int)effect];
    }

    public enum Effect
    {
        Nothing = 0,
        DamageUp,
        SpeedDown,
        SpeedUp
    }

    public Effect effect = Effect.Nothing;

    Collider2D grannyCollider;
    ItemUse itemUse;

    void OnTriggerStay2D(Collider2D other)
    {
        if (Input.GetButtonUp("Fire2") && other == grannyCollider)
        {
            bool pickedUp = itemUse.HoldPill(effect);
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

        var spriteRenderer = GetComponent<SpriteRenderer>();

        if (materials == null)
        {
            var effectsCount = Enum.GetNames(typeof(Effect)).Length;

            materials = new Material[effectsCount];

            materials[0] = spriteRenderer.material;

            for (int i = 1; i < effectsCount; i++)
            {
                var randomMaterial = new Material(spriteRenderer.material);

                randomMaterial.SetColor("_ColorR", Utils.randomHueColor());
                randomMaterial.SetColor("_ColorG", Utils.randomHueColor());
                randomMaterial.SetColor("_ColorB", Color.white);
                 
                materials[i] = randomMaterial;
            }
        }
            
        spriteRenderer.material = getMaterial(effect);
    }
		
    #if UNITY_EDITOR
    void OnDrawGizmos()
    {
		GUIContent textContent = new GUIContent(effect.ToString());

		GUIStyle style = (GUI.skin != null) ? new GUIStyle(GUI.skin.GetStyle("Label")) : new GUIStyle();

		style.normal.textColor = Color.yellow;
		style.fontSize = 12;

		Vector2 textSize = style.CalcSize(textContent);
		Vector3 screenPoint = Camera.current.WorldToScreenPoint(transform.position);

		if (screenPoint.z > 0)
		{
			var worldPosition = Camera.current.ScreenToWorldPoint(new Vector3(screenPoint.x - textSize.x * 0.5f, screenPoint.y + textSize.y * 0.5f, screenPoint.z));
		
			UnityEditor.Handles.Label(worldPosition, textContent, style);
		}
    }
    #endif
}