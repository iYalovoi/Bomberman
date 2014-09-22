using UnityEngine;
using System.Collections;
using System.IO;

namespace Assets.Script
{
    public class ReSkinAnimation : MonoBehaviour
    {
        public EnemyTypes enemyType;

        void LateUpdate()
        {
            var spriteRenderer = GetComponent<SpriteRenderer>();
            var spriteName = spriteRenderer.sprite.name;
            var spritesFile = enemyType.ToString();
            if (spriteName.IndexOf("dead") != -1)
                spritesFile += "_dead";
            var subSprites = Resources.LoadAll<Sprite>("Sprites/" + spritesFile);
            var spriteIndex = int.Parse(spriteName.Substring(spriteName.LastIndexOf("_", System.StringComparison.Ordinal) + 1));
            spriteRenderer.sprite = subSprites[spriteIndex];
        }
    }
}
