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
		/// Costom sprite for when the axis returns positive values
		/// </summary>
		public Sprite PositiveValueIcon;

		/// <summary>
		/// Costom sprite for when the axis returns negative values
		/// </summary>
		public Sprite NegativeValueIcon;
	}
}
