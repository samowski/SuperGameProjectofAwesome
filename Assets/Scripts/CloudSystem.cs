using UnityEngine;
using System.Collections.Generic;

public class CloudSystem : MonoBehaviour
{
	List<GameObject> clouds;
	List<SpriteRenderer> spriteRenderers;
	public Sprite[] CloudSprites;

	GameObject baseCloud;

	int leftIndex;
	int rightIndex;

	float maxSpriteWidth;
	float maxSpriteHeight;

	float cellWidth = 1.0f;

	int randomIndexOffset;
	float moveDistance = 0;

	public float MaxHeight;
	public float MinHeight;
	public float Speed = 0;
	public float CloudsPerUnit = 1.0f;

	void Awake()
	{
		clouds = new List<GameObject>();
		spriteRenderers = new List<SpriteRenderer>();

		randomIndexOffset = Random.Range(int.MinValue, int.MaxValue);
	}

	void Start()
	{
		baseCloud = GameObject.Find("BaseCloud");
		baseCloud.SetActive(false);

		getSpriteDimensions();
	}

	void getSpriteDimensions()
	{
		maxSpriteWidth = 0;
		maxSpriteHeight = 0;

		foreach (var sprite in CloudSprites)
		{
			maxSpriteWidth = Mathf.Max(maxSpriteWidth, Mathf.CeilToInt(sprite.bounds.size.x * baseCloud.transform.localScale.x));
			maxSpriteHeight = Mathf.Max(maxSpriteHeight, sprite.bounds.size.y * baseCloud.transform.localScale.y);
		}
	}

	void updateArrays(int newCloudCount)
	{
		int count = clouds.Count;

		if (newCloudCount < count - 1) // 1 buffer before deleting to counter float to int blinking
		{ 
			for (int i = count - 1; i >= newCloudCount; i--)
			{
				GameObject.Destroy(clouds[i]);
			}

			clouds.RemoveRange(newCloudCount, count - newCloudCount);
			spriteRenderers.RemoveRange(newCloudCount, count - newCloudCount);
		}
		else if (newCloudCount > count)
		{
			for (int i = count; i < newCloudCount; i++)
			{
				var clone = GameObject.Instantiate(baseCloud);
				clone.transform.parent = gameObject.transform;
				clone.SetActive(true);
				clouds.Add(clone);
				spriteRenderers.Add(clone.GetComponent<SpriteRenderer>());
			}
		}
	}

	void getBorderIndices()
	{
		var camera = Camera.main;

		float cameraPosition = camera.transform.position.x;
		float halfCameraWidth = camera.orthographicSize * camera.aspect;

		float left = cameraPosition - halfCameraWidth;
		float right = cameraPosition + halfCameraWidth;

		leftIndex = Mathf.FloorToInt((left - maxSpriteWidth) / cellWidth) - 1;
		rightIndex = Mathf.FloorToInt((right + maxSpriteWidth) / cellWidth) + 1;
	}

	void generateClouds()
	{
		foreach (var cloud in clouds)
		{
			cloud.SetActive(false);	
		}

		for (int i = leftIndex; i < rightIndex; i++)
		{
			Random.seed = i + randomIndexOffset - Mathf.FloorToInt(moveDistance / cellWidth);

			float xOffset = (i + Random.value) * cellWidth + moveDistance - Mathf.Floor(moveDistance / cellWidth) * cellWidth;
			float yOffset = -Random.value * (MinHeight - MaxHeight + maxSpriteHeight) + MinHeight + maxSpriteHeight * 0.5f;

			var cloud = clouds[i - leftIndex];

			cloud.transform.position = new Vector3(xOffset, yOffset, 0.0f);
			cloud.SetActive(true);

			var spriteRenderer = spriteRenderers[i - leftIndex];

			spriteRenderer.sprite = CloudSprites[Random.Range(0, CloudSprites.Length)];
			spriteRenderer.sortingOrder = Random.Range(int.MinValue, int.MaxValue);
		}
	}

	void Update()
	{
		moveDistance += Time.deltaTime * Speed;
		moveDistance %= 50000.0f;

		cellWidth = CloudsPerUnit == 0.0f ? 0.0f : Mathf.Abs(1.0f / CloudsPerUnit);

		getBorderIndices();

		updateArrays(rightIndex - leftIndex);

		generateClouds();
	}
}