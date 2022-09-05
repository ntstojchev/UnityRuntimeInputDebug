using Code.InputActions;
using System.Collections.Generic;

namespace Code.InputDebugger
{
	/// <summary>
	/// Handles information about the input events during a frame
	/// </summary>
	public class FrameInputSummary
	{
		/// <summary>
		/// Frame count when the event occured
		/// </summary>
		public int FrameCount;

		/// <summary>
		/// Time since the start of the game when the input event occured
		/// </summary>
		public float FrameTime;

		/// <summary>
		/// All input actions that occured during this frame
		/// </summary>
		public List<KeyInputAction> KeyActions;
	}
}
