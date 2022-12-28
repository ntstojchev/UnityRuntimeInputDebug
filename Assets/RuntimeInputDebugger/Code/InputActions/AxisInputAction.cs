// MIT License Copyright(c) 2022 Nedislav Stoychev, https://github.com/ntstojchev/UnityRuntimeInputDebug

using System;
using UnityEngine;

namespace Code.InputActions
{
	/// <summary>
	/// Handles Axis input mapping to display name and assets
	/// </summary>
	[Serializable]
	public class AxisInputAction
	{
		/// <summary>
		/// Axis display name
		/// </summary>
		public string AxisName;

		/// <summary>
		/// Custom sprite for when the axis returns positive values
		/// </summary>
		public Sprite PositiveValueIcon;

		/// <summary>
		/// Custom sprite for when the axis returns negative values
		/// </summary>
		public Sprite NegativeValueIcon;
	}
}
