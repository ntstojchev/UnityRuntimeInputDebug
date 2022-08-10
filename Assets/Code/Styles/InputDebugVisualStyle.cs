using Code.InputActions;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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
	}
}
