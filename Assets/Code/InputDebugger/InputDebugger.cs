using Code.InputActions;
using Code.Styles;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Code.InputDebugger
{
	public class InputDebugger : MonoBehaviour
	{
		public bool Enabled = false;

		[Header("Capture settings")]
		public InputDebugVisualStyle InputActions;
		public Vector2 InputIconsSize = new Vector2(64, 64);
		public GUIStyle DebugTextStyle;
		public string AxisValueLabelPattern = "{0} {1}";

		public bool CaptureMousePosition = false;
		public string MousePositionLabelPattern = "MousePos: {0}x {1}y";

		[Header("Position")]
		public DebugStyleAnchorType DrawAnchor = DebugStyleAnchorType.TopLeft;
		public int DrawXOffset = 0;
		public int DrawYOffset = 0;

		private DebugStyleAnchorType _drawAnchorInternal;

		private List<KeyInputAction> _captureKeys = new List<KeyInputAction>();
		private List<AxisInputAction> _captureAxes = new List<AxisInputAction>();

		private List<KeyInputAction> _displayKeyActions = new List<KeyInputAction>();
		private Dictionary<AxisInputAction, float> _displayAxisActions = new Dictionary<AxisInputAction, float>();

		private void Start()
		{
			UpdateCapturedInputActions();
		}

		private void Update()
		{
			if (!Enabled) {
				return;
			}

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

			//Begin layout. Vertical with inner Horizontals - content in the middle
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

			DrawInputAxisActions();

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

			#region Draw Capture Mouse Position
			if (CaptureMousePosition) {
				GUILayout.BeginHorizontal();

				if (_drawAnchorInternal == DebugStyleAnchorType.BottomRight
				|| _drawAnchorInternal == DebugStyleAnchorType.TopRight
				|| _drawAnchorInternal == DebugStyleAnchorType.MiddleRight
				|| _drawAnchorInternal == DebugStyleAnchorType.Middle
				|| _drawAnchorInternal == DebugStyleAnchorType.TopCenter
				|| _drawAnchorInternal == DebugStyleAnchorType.BottomCenter) {
					GUILayout.FlexibleSpace();
				}

				DrawMousePosition();

				if (_drawAnchorInternal == DebugStyleAnchorType.BottomLeft
				|| _drawAnchorInternal == DebugStyleAnchorType.TopLeft
				|| _drawAnchorInternal == DebugStyleAnchorType.MiddleLeft
				|| _drawAnchorInternal == DebugStyleAnchorType.Middle
				|| _drawAnchorInternal == DebugStyleAnchorType.TopCenter
				|| _drawAnchorInternal == DebugStyleAnchorType.BottomCenter) {
					GUILayout.FlexibleSpace();
				}

				GUILayout.EndHorizontal();
			}
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

		public void UpdateCapturedInputActions()
		{
			_captureKeys.Clear();
			_captureKeys.AddRange(InputActions.Keys);

			_captureAxes.Clear();
			_captureAxes.AddRange(InputActions.Axes);
		}

		private void SetAnchor(DebugStyleAnchorType newAnchor)
		{
			if (newAnchor == DebugStyleAnchorType.None) {
				newAnchor = DebugStyleAnchorType.TopLeft;
			}

			_drawAnchorInternal = DrawAnchor;
		}

		private void DrawInputKeyActions()
		{
			if (_displayKeyActions.Any()) {
				foreach (KeyInputAction keyAction in _displayKeyActions) {
					if (keyAction.Icon == null) {
						GUILayout.Label(keyAction.Name.ToString(), DebugTextStyle);
					}
					else {
						GUIStyle protoStyle = new GUIStyle();
						protoStyle.normal.background = keyAction.Icon.texture;
						GUILayout.Box(Texture2D.blackTexture, protoStyle, GUILayout.Width(InputIconsSize.x), GUILayout.Height(InputIconsSize.y));
					}
				}
			}
		}

		private void DrawInputAxisActions()
		{
			if (_displayAxisActions.Any()) {
				foreach (KeyValuePair<AxisInputAction, float> axisInputActionPair in _displayAxisActions) {
					if (axisInputActionPair.Value > 0) {
						if (axisInputActionPair.Key.PositiveValueIcon == null) {
							DrawAxisLabel(axisInputActionPair);
						}
						else {
							GUIStyle protoStyle = new GUIStyle();
							protoStyle.normal.background = axisInputActionPair.Key.PositiveValueIcon.texture;
							GUILayout.Box(Texture2D.blackTexture, protoStyle, GUILayout.Width(InputIconsSize.x), GUILayout.Height(InputIconsSize.y));
						}
					}
					else if (axisInputActionPair.Value < 0) {
						if (axisInputActionPair.Key.NegativeValueIcon == null) {
							DrawAxisLabel(axisInputActionPair);
						}
						else {
							GUIStyle protoStyle = new GUIStyle();
							protoStyle.normal.background = axisInputActionPair.Key.NegativeValueIcon.texture;
							GUILayout.Box(Texture2D.blackTexture, protoStyle, GUILayout.Width(InputIconsSize.x), GUILayout.Height(InputIconsSize.y));
						}
					}
				}
			}
		}

		private void DrawAxisLabel(KeyValuePair<AxisInputAction, float> axisPair)
		{
			GUILayout.Label(
				string.Format(AxisValueLabelPattern,
					axisPair.Key.AxisName.ToString(),
					axisPair.Value.ToString()
				), 
				DebugTextStyle
			);
		}

		private void DrawMousePosition()
		{
			GUILayout.Label(string.Format(MousePositionLabelPattern, Input.mousePosition.x, Input.mousePosition.y), DebugTextStyle);
		}

		[ContextMenu("Update Input Actions")]
		private void ContextUpdateCapturedInputActions()
		{
			UpdateCapturedInputActions();
		}
	}
}
