// MIT License Copyright(c) 2022 Nedislav Stoychev, https://github.com/ntstojchev/UnityRuntimeInputDebug

using Code.InputActions;
using Code.Styles;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Code.InputDebugger
{
	public class InputDebugger : MonoBehaviour
	{
		/// <summary>
		/// Starts/stops the input display debugger
		/// </summary>
		[Tooltip("Starts/stops the input display debugger")]
		public bool InputDisplayEnabled = false;

		/// <summary>
		/// Should the input debugger capture the mouse position
		/// </summary>
		[Tooltip("Should the input debugger capture the mouse position")]
		public bool CaptureMousePosition = false;

		[Header("Capture settings")]
		/// <summary>
		/// Input actions collection to capture during the frame
		/// </summary>
		public InputDebugCaptureActions InputActions;

		/// <summary>
		/// Display size for the icons that are assigned on the input actions
		/// </summary>
		public Vector2 InputIconsSize = new Vector2(64, 64);

		[Header("Capture default display settings")]

		/// <summary>
		/// Axis display pattern when appropriate asset (icon) is not provided;
		/// </summary>
		[Tooltip("Axis display pattern when appropriate asset (icon) is not provided")]
		public string AxisValueLabelPattern = "{0} {1}";

		/// <summary>
		/// Display label for mouse coordinates when CaptureMousePosition is enabled
		/// </summary>
		[Tooltip("Display label for mouse coordinates when CaptureMousePosition is enabled")]
		public string MousePositionLabelPattern = "MousePos: {0}x {1}y";

		/// <summary>
		/// Text style that will be applied on the debug labels
		/// </summary>
		[Tooltip("Text style that will be applied on the debug labels")]
		public GUIStyle DebugTextStyle;

		[Header("Runtime input display position")]
		public DebugStyleAnchorType DrawAnchor = DebugStyleAnchorType.TopLeft;

		/// <summary>
		/// Draw offset in pixels for the X axis that will be applied on the anchor
		/// </summary>
		[Tooltip("Draw offset in pixels for the X axis that will be applied on the anchor")]
		public int DrawXOffset = 0;

		/// <summary>
		/// Draw offset in pixels for the Y axis that will be applied on the anchor
		/// </summary>
		[Tooltip("Draw offset in pixels for the Y axis that will be applied on the anchor")]
		public int DrawYOffset = 0;

		[Header("Input History Settings")]
		/// <summary>
		/// Starts/stops the input debugger
		/// </summary>
		[Tooltip("Starts/stops the input history debug")]
		public bool InputHistoryEnabled = false;

		/// <summary>
		/// Number of stored input actions in the input history
		/// </summary>
		[Tooltip("Number of stored input actions in the input history")]
		public int QueueSize = 40;

		/// <summary>
		/// Time in seconds when the input action will be removed
		/// </summary>
		[Tooltip("Time in seconds when the input action will be removed")]
		public float DequeueTime = 1.5f;

		/// <summary>
		/// When both debuggers are active what pixel to be applied on the X axis to avoid overlap
		/// </summary>
		[Tooltip("When both debuggers are active what pixel to be applied on the X axis to avoid overlap")]
		public float InputHistoryDrawYOffset = 200f;

		private DebugStyleAnchorType _drawAnchorInternal;

		private List<KeyInputAction> _captureKeys = new List<KeyInputAction>();
		private List<AxisInputAction> _captureAxes = new List<AxisInputAction>();

		private List<KeyInputAction> _displayKeyActions = new List<KeyInputAction>();
		private Dictionary<AxisInputAction, float> _displayAxisActions = new Dictionary<AxisInputAction, float>();

		private LinkedList<FrameInputSummary> _inputHistory = new LinkedList<FrameInputSummary>();
		private List<KeyInputAction> _justPressedKeys = new List<KeyInputAction>();

		private void Start()
		{
			//Prepares the debugger to capture input from the assigned input set
			UpdateCapturedInputActionsFromInputSet();
		}

		private void Update()
		{
			if (InputDisplayEnabled) {
				
				//Try to capture the input actions and fill the collections
				FillInputCaptureInformation();

				//Capture axes that have negative/positive values
				foreach (AxisInputAction axisAction in _captureAxes) {
					float axisValue = Input.GetAxis(axisAction.AxisName);
					if (axisValue != 0) {
						_displayAxisActions.Add(axisAction, axisValue);
					}
				}
			}

			if (InputHistoryEnabled) {

				if (!InputDisplayEnabled) {
					FillInputCaptureInformation();
				}

				//If something was JUST pressed, add it to the history
				if (_justPressedKeys.Any()) {

					if (_inputHistory.Count > QueueSize) {
						_inputHistory.RemoveLast();
					}

					_inputHistory.AddFirst(new FrameInputSummary
					{
						FrameCount = Time.frameCount,
						FrameTime = Time.time,
						KeyActions = new List<KeyInputAction>(_displayKeyActions),
					});
				}

				//Remove input actions that have expired
				if (_inputHistory.Any()) {
					if (Time.time - _inputHistory.Last().FrameTime > DequeueTime) {
						_inputHistory.RemoveLast();
					}
				}
			}
		}

		/// <summary>
		/// Update all capture keys from the InputActions set
		/// </summary>
		private void FillInputCaptureInformation()
		{
			//Prepare capture collections
			_displayKeyActions.Clear();
			_displayAxisActions.Clear();
			_justPressedKeys.Clear();

			if (Input.anyKey) {
				foreach (KeyInputAction keyAction in _captureKeys) {
					if (Input.GetKey(keyAction.Key)) {
						_displayKeyActions.Add(keyAction);
					}

					if (Input.GetKeyDown(keyAction.Key)) {
						_justPressedKeys.Add(keyAction);
					}
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

			if (InputDisplayEnabled) {

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

				DrawInputKeyActions(_displayKeyActions);

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

				DrawInputAxisActions(_displayAxisActions);

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

			}

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

			//Begin drawing the input history in separate area
			if (InputHistoryEnabled) {
				if (_drawAnchorInternal == DebugStyleAnchorType.TopLeft) {
					GUILayout.BeginArea(new Rect(0, InputHistoryDrawYOffset, Screen.width, Screen.height));
				}
				else {
					GUILayout.BeginArea(new Rect(0, 0, Screen.width, Screen.height));
				}

				foreach (FrameInputSummary inputSummary in _inputHistory) {
					GUILayout.BeginHorizontal();
					DrawInputKeyActions(inputSummary.KeyActions);
					GUILayout.EndHorizontal();
				}
				GUILayout.EndArea();
			}
		}

		/// <summary>
		/// Updates captured input actions and axes from the assigned input set
		/// </summary>
		public void UpdateCapturedInputActionsFromInputSet()
		{
			if (InputActions == null) {
				Debug.LogWarning("InputDebugger cannot work with empty capture set. Either assign input capture set or create a new one and then assign it. " +
					"You can create new one from the context menu: InputDebugger/InputDebugCaptureActions");
				return;
			}

			FillCaptureInputKeyActions(InputActions.Keys);
			FillCaptureInputAxisActions(InputActions.Axes);
		}

		/// <summary>
		/// Add input key action to the input capture list
		/// </summary>
		/// <param name="keyAction">Input key action to be added</param>
		public void AddCaptureInputKeyAction(KeyInputAction keyAction)
		{
			if (!_captureKeys.Contains(keyAction)) {
				_captureKeys.Add(keyAction);
			}
		}

		/// <summary>
		/// Add input axis action to the input capture list
		/// </summary>
		/// <param name="axisAction">Axis action to be added</param>
		public void AddCaptureInputAxisAction(AxisInputAction axisAction)
		{
			if (!_captureAxes.Contains(axisAction)) {
				_captureAxes.Add(axisAction);
			}
		}

		/// <summary>
		/// Removes input key action from the capture list
		/// </summary>
		/// <param name="keyAction">Input key action to be removed</param>
		public void RemoveCaptureInputKeyAction(KeyInputAction keyAction)
		{
			_captureKeys.Remove(keyAction);
		}

		/// <summary>
		/// Removes input axis action from the capture list
		/// </summary>
		/// <param name="axisAction">Axis action to be removed</param>
		public void RemoveCaptureInputAxisAction(AxisInputAction axisAction)
		{
			_captureAxes.Remove(axisAction);
		}

		/// <summary>
		/// Clears and fills the capture input keys with the given list of input actions
		/// </summary>
		/// <param name="keyActions"></param>
		public void FillCaptureInputKeyActions(List<KeyInputAction> keyActions)
		{
			_captureKeys.Clear();
			_captureKeys.AddRange(keyActions);
		}

		/// <summary>
		/// Clears and fills the capture input axes with the given list of input axes
		/// </summary>
		/// <param name="axisActions"></param>
		public void FillCaptureInputAxisActions(List<AxisInputAction> axisActions)
		{
			_captureAxes.Clear();
			_captureAxes.AddRange(axisActions);
		}

		/// <summary>
		/// Enables InputDebugger component based on Behaviour.enabled field
		/// </summary>
		public void EnableInputDebugger()
		{
			enabled = true;
		}

		/// <summary>
		/// Disables InputDebugger component based on Behaviour.enabled field
		/// </summary>
		public void DisableInputDebugger()
		{
			enabled = false;
		}

		private void SetAnchor(DebugStyleAnchorType newAnchor)
		{
			if (newAnchor == DebugStyleAnchorType.None) {
				newAnchor = DebugStyleAnchorType.TopLeft;
			}

			_drawAnchorInternal = DrawAnchor;
		}

		private void DrawInputKeyActions(List<KeyInputAction> inputActions)
		{
			if (inputActions.Any()) {
				foreach (KeyInputAction keyAction in inputActions) {
					if (keyAction.Icon == null) {
						if (string.IsNullOrEmpty(keyAction.Name)) {
							GUILayout.Label(keyAction.Key.ToString(), DebugTextStyle);
						}
						else {
							GUILayout.Label(keyAction.Name.ToString(), DebugTextStyle);
						}
					}
					else {
						GUIStyle protoStyle = new GUIStyle();
						protoStyle.normal.background = keyAction.Icon.texture;
						GUILayout.Box(Texture2D.blackTexture, protoStyle, GUILayout.Width(InputIconsSize.x), GUILayout.Height(InputIconsSize.y));
					}
				}
			}
		}

		private void DrawInputAxisActions(Dictionary<AxisInputAction, float> axisActions)
		{
			if (axisActions.Any()) {
				foreach (KeyValuePair<AxisInputAction, float> axisInputActionPair in axisActions) {
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
			UpdateCapturedInputActionsFromInputSet();
		}
	}
}
