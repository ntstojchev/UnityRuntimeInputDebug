using Code.InputActions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Code.Styles
{
	[CreateAssetMenu(fileName = "InputDebugVisualStyle", menuName = "Debug Styles/InputDebugVisualStyle", order = 1)]
	public class InputDebugVisualStyle : ScriptableObject
	{
		public List<KeyInputAction> Keys;
		public List<AxisInputAction> Axes;

		public List<string> GetAllAxes()
		{
			return Axes.Select(a => a.AxisName).ToList();
		}

		public List<KeyCode> GetAllKeys()
		{
			return Keys.Select(k => k.Key).ToList();
		}

		[ContextMenu("FillAllKeys")]
		private void FillAllKeys()
		{
			Keys.Clear();
			foreach (KeyCode key in Enum.GetValues(typeof(KeyCode))) {
				Keys.Add(new KeyInputAction
				{
					Name = key.ToString(),
					Key = key,
				});
			}
		}

		[ContextMenu("Export Sprite Assets")]
		private void ExportSpriteAssets()
		{
			var spritesToExport = new List<Sprite>();
			spritesToExport.AddRange(Keys.Select(k => k.Icon));
			spritesToExport.AddRange(Axes.Select(a => a.NegativeValueIcon));
			spritesToExport.AddRange(Axes.Select(a => a.PositiveValueIcon));
			spritesToExport = spritesToExport.Where(s => s != null).ToList();

			SpritesToPNGTextures(spritesToExport);
		}

		private void SpritesToPNGTextures(List<Sprite> sprites)
		{
			foreach (Sprite sprite in sprites) {
				SpriteToPNGExport(sprite);
			}

#if UNITY_EDITOR
			AssetDatabase.Refresh();
#endif
		}

		private void SpriteToPNGExport(Sprite sprite)
		{
			var texture = new Texture2D(
				(int)sprite.textureRect.width,
				(int)sprite.textureRect.height);

			Color[] pixels = sprite.texture.GetPixels(
				(int)sprite.textureRect.x,
				(int)sprite.textureRect.y,
				(int)sprite.textureRect.width,
				(int)sprite.textureRect.height);

			texture.SetPixels(pixels);
			texture.Apply();

			ExportTexture(sprite.name, texture);
		}

		public void ExportTexture(string name, Texture2D texture2D)
		{
			string directoryPath = Application.dataPath + "/SpritesExport";

			if (!Directory.Exists(directoryPath)) {
				Directory.CreateDirectory(directoryPath);
			}

			File.WriteAllBytes($"{directoryPath}/{name}.png", texture2D.EncodeToPNG());
		}
	}
}
