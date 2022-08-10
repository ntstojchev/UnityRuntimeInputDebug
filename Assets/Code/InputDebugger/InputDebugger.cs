using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Code.InputDebugger
{
	public class InputDebugger : MonoBehaviour
	{
		[Header("Capture settings")]
		public List<KeyCode> IgnoreKeys = new List<KeyCode>();
		public List<string> CaptureAxes = new List<string>();

		[Header("Position")]
		public DebugStyleAnchorType DrawAnchor = DebugStyleAnchorType.TopLeft;
		public int DrawXOffset = 0;
		public int DrawYOffset = 0;

		private DebugStyleAnchorType _drawAnchorInternal;

		public int labelCount = 5;

		private List<KeyCode> _displayActions = new List<KeyCode>();
		private List<string> _axes = new List<string>();

		private void Start()
		{

		}

		private void Update()
		{
		}

		private void OnGUI()
		{
			AcquireInputContent();

			if (DrawAnchor != _drawAnchorInternal) {
				SetAnchor(DrawAnchor);
			}

			//Begin layout. Vertical with inner Horizontal - content in the middle
			GUILayout.BeginArea(new Rect(DrawXOffset, DrawYOffset, Screen.width - DrawXOffset, Screen.height - DrawYOffset));
			GUILayout.BeginVertical();

			if (_drawAnchorInternal == DebugStyleAnchorType.Middle
			|| _drawAnchorInternal == DebugStyleAnchorType.MiddleLeft
			|| _drawAnchorInternal == DebugStyleAnchorType.MiddleRight
			|| _drawAnchorInternal == DebugStyleAnchorType.BottomCenter
			|| _drawAnchorInternal == DebugStyleAnchorType.BottomLeft
			|| _drawAnchorInternal == DebugStyleAnchorType.BottomRight) {
				GUILayout.FlexibleSpace();
			}

			GUILayout.BeginHorizontal();

			if (_drawAnchorInternal == DebugStyleAnchorType.BottomRight
			|| _drawAnchorInternal == DebugStyleAnchorType.TopRight
			|| _drawAnchorInternal == DebugStyleAnchorType.MiddleRight
			|| _drawAnchorInternal == DebugStyleAnchorType.Middle
			|| _drawAnchorInternal == DebugStyleAnchorType.TopCenter
			|| _drawAnchorInternal == DebugStyleAnchorType.BottomCenter) {
				GUILayout.FlexibleSpace();
			}

			DrawInputDebugContent();

			if (_drawAnchorInternal == DebugStyleAnchorType.BottomLeft
			|| _drawAnchorInternal == DebugStyleAnchorType.TopLeft
			|| _drawAnchorInternal == DebugStyleAnchorType.MiddleLeft
			|| _drawAnchorInternal == DebugStyleAnchorType.Middle
			|| _drawAnchorInternal == DebugStyleAnchorType.TopCenter
			|| _drawAnchorInternal == DebugStyleAnchorType.BottomCenter) {
				GUILayout.FlexibleSpace();
			}

			GUILayout.EndHorizontal();

			if (_drawAnchorInternal == DebugStyleAnchorType.Middle
			|| _drawAnchorInternal == DebugStyleAnchorType.MiddleLeft
			|| _drawAnchorInternal == DebugStyleAnchorType.MiddleRight
			|| _drawAnchorInternal == DebugStyleAnchorType.TopCenter
			|| _drawAnchorInternal == DebugStyleAnchorType.TopLeft
			|| _drawAnchorInternal == DebugStyleAnchorType.TopRight) {
				GUILayout.FlexibleSpace();
			}

			GUILayout.EndVertical();
			GUILayout.EndArea();
		}

		private void SetAnchor(DebugStyleAnchorType newAnchor)
		{
			_drawAnchorInternal = DrawAnchor;
		}

		private void AcquireInputContent()
		{

		}

		private void DrawInputDebugContent()
		{
			for (int i = 0; i < labelCount; i++) {
				GUILayout.Label("INPUT" + i.ToString() + "    ");
			}
		}
	}
}
