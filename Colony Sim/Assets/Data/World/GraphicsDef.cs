using System.Collections.Generic;
using UnityEngine;

namespace ColonySim
{
	// Represent a grahpic in out game.
	[System.Serializable]
	public class GraphicDef : Def
	{
		/* Texture name in the Resources/Textures directory */
		public string textureName = string.Empty;

		/* Material name in the Resources/Materials directory */
		public string materialName = "tilables";

		/* Graphic size */
		public Vector2 size = Vector2.one;

		/* Graphic color */
		public Color color = Color.white;

		/* Is Instanced ? */
		public bool isInstanced = true;

		/* Draw Priority */
		public float drawPriority = 0f;

		/* Graphic pivot */
		public Vector2 pivot = Vector2.zero;
	}

}