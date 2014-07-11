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
			var subSprites = Resources.LoadAll<Sprite>("Sprites/" + enemyType.ToString());
			var renderer = GetComponent<SpriteRenderer>();
			var spriteName = renderer.sprite.name;
			int spriteIndex = int.Parse(spriteName.Substring(spriteName.LastIndexOf("_") + 1));
			renderer.sprite = subSprites[spriteIndex];
		}
	}
}
