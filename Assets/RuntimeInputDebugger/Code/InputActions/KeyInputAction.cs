// MIT License Copyright(c) 2022 Nedislav Stoychev, https://github.com/ntstojchev/UnityRuntimeInputDebug

using System;
using UnityEngine;

namespace Code.InputActions
{
	/// <summary>
	/// Handles input key mapping to display name and assets
	/// </summary>
	[Serializable]
	public class KeyInputAction
	{
		/// <summary>
		/// Action display name
		/// </summary>
		public string Name;
		public KeyCode Key;

		/// <summary>
		/// Custom sprite assigned to the KeyCode value
		/// </summary>
		public Sprite Icon;
	}
}
