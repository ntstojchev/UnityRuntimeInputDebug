// MIT License Copyright(c) 2022 Nedislav Stoychev, https://github.com/ntstojchev/UnityRuntimeInputDebug

using Code.InputActions;
using System.Collections.Generic;
using UnityEngine;

namespace Code.Styles
{
	/// <summary>
	/// Handles data for all input keys and axes that should be captured during a frame
	/// </summary>
	[CreateAssetMenu(fileName = "InputDebugCaptureActions", menuName = "Debug Styles/InputDebugCaptureActions", order = 1)]
	public class InputDebugCaptureActions : ScriptableObject
	{
		/// <summary>
		/// Keys that should be captured during one frame
		/// </summary>
		[Tooltip("Keys that should be captured during the frame")]
		public List<KeyInputAction> Keys;

		/// <summary>
		/// Axes that should be captured during one frame
		/// </summary>
		[Tooltip("Axes that should be captured during the frame")]
		public List<AxisInputAction> Axes;
	}
}
