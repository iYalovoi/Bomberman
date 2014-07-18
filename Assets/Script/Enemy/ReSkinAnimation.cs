using UnityEngine;
using System.Collections;
using Assets.Script.Level;
using System.IO;

namespace Assets.Script
{
	public class ReSkinAnimation : MonoBehaviour 
	{
		public EnemyTypes enemyType;

		void LateUpdate() 
		{
			var subSprites = Resources.LoadAll<Sprite>("Sprites/" + enemyType);
			var spriteRenderer = GetComponent<SpriteRenderer>();
			var spriteName = spriteRenderer.sprite.name;
			var spriteIndex = int.Parse(spriteName.Substring(spriteName.LastIndexOf("_", System.StringComparison.Ordinal) + 1));
			spriteRenderer.sprite = subSprites[spriteIndex];
		}
	}
}
