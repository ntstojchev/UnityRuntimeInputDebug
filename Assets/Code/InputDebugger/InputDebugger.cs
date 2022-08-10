using Code.InputActions;
using Code.Styles;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Code.InputDebugger
{
	public class InputDebugger : MonoBehaviour
	{
		[Header("Capture settings")]
		public InputDebugVisualStyle InputActions;

		[Header("Position")]
		public DebugStyleAnchorType DrawAnchor = DebugStyleAnchorType.TopLeft;
		public int DrawXOffset = 0;
		public int DrawYOffset = 0;

		private DebugStyleAnchorType _drawAnchorInternal;

		public int labelCount = 5;

		private List<KeyInputAction> _captureKeys = new List<KeyInputAction>();
		private List<AxisInputAction> _captureAxes = new List<AxisInputAction>();

		private List<KeyInputAction> _displayKeyActions = new List<KeyInputAction>();
		private Dictionary<AxisInputAction, float> _displayAxisActions = new Dictionary<AxisInputAction, float>();

		private void Start()
		{
			UpdateCapturedInputActions();
		}

		public void UpdateCapturedInputActions()
		{
			_captureKeys.Clear();
			_captureKeys.AddRange(InputActions.Keys);

			_captureAxes.Clear();
			_captureAxes.AddRange(InputActions.Axes);
		}

		private void Update()
		{
			_displayKeyActions.Clear();
			_displayAxisActions.Clear();

			if (Input.anyKey) {
				foreach (KeyInputAction keyAction in _captureKeys) {
					if (Input.GetKey(keyAction.Key)) {
						_displayKeyActions.Add(keyAction);
					}
				}
			}

			foreach (AxisInputAction axisAction in _captureAxes) {
				float axisValue = Input.GetAxis(axisAction.AxisName);
				if (axisValue != 0) {
					_displayAxisActions.Add(axisAction, axisValue);
				}
			}
		}

		private void OnGUI()
		{
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

			#region Draw InputKeyActions
			GUILayout.BeginHorizontal();

			if (_drawAnchorInternal == DebugStyleAnchorType.BottomRight
			|| _drawAnchorInternal == DebugStyleAnchorType.TopRight
			|| _drawAnchorInternal == DebugStyleAnchorType.MiddleRight
			|| _drawAnchorInternal == DebugStyleAnchorType.Middle
			|| _drawAnchorInternal == DebugStyleAnchorType.TopCenter
			|| _drawAnchorInternal == DebugStyleAnchorType.BottomCenter) {
				GUILayout.FlexibleSpace();
			}

			DrawInputKeyActions();

			if (_drawAnchorInternal == DebugStyleAnchorType.BottomLeft
			|| _drawAnchorInternal == DebugStyleAnchorType.TopLeft
			|| _drawAnchorInternal == DebugStyleAnchorType.MiddleLeft
			|| _drawAnchorInternal == DebugStyleAnchorType.Middle
			|| _drawAnchorInternal == DebugStyleAnchorType.TopCenter
			|| _drawAnchorInternal == DebugStyleAnchorType.BottomCenter) {
				GUILayout.FlexibleSpace();
			}

			GUILayout.EndHorizontal();
			#endregion

			#region Draw InputAxisActions
			GUILayout.BeginHorizontal();

			if (_drawAnchorInternal == DebugStyleAnchorType.BottomRight
			|| _drawAnchorInternal == DebugStyleAnchorType.TopRight
			|| _drawAnchorInternal == DebugStyleAnchorType.MiddleRight
			|| _drawAnchorInternal == DebugStyleAnchorType.Middle
			|| _drawAnchorInternal == DebugStyleAnchorType.TopCenter
			|| _drawAnchorInternal == DebugStyleAnchorType.BottomCenter) {
				GUILayout.FlexibleSpace();
			}

			DrawINputAxisActions();

			if (_drawAnchorInternal == DebugStyleAnchorType.BottomLeft
			|| _drawAnchorInternal == DebugStyleAnchorType.TopLeft
			|| _drawAnchorInternal == DebugStyleAnchorType.MiddleLeft
			|| _drawAnchorInternal == DebugStyleAnchorType.Middle
			|| _drawAnchorInternal == DebugStyleAnchorType.TopCenter
			|| _drawAnchorInternal == DebugStyleAnchorType.BottomCenter) {
				GUILayout.FlexibleSpace();
			}

			GUILayout.EndHorizontal();
			#endregion

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

		private void DrawInputKeyActions()
		{
			if (_displayKeyActions.Any()) {
				foreach (KeyInputAction keyAction in _displayKeyActions) {
					GUILayout.Label(keyAction.Name.ToString());
				}
			}
		}

		private void DrawINputAxisActions()
		{
			if (_displayAxisActions.Any()) {
				foreach (KeyValuePair<AxisInputAction, float> axisInputActionPair in _displayAxisActions) {
					GUILayout.Label(axisInputActionPair.Key.AxisName.ToString());
				}
			}
		}

		[ContextMenu("Update Input Actions")]
		private void ContextUpdateCapturedInputActions()
		{
			UpdateCapturedInputActions();
		}
	}
}
