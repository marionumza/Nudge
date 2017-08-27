// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Harvest.cs" company="">
//   
// </copyright>
// <summary>
//   The harvest model.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace NudgeHarvester
{
    /// <summary>
    /// The harvest model.
    /// </summary>
    public class Harvest
    {
        /// <summary>
        /// Gets or sets the foreground app hash.
        /// </summary>
        public int ForegroundAppHash { get; set; }

        /// <summary>
        /// Gets or sets the keyboard activity.
        /// </summary>
        public int KeyboardActivity { get; set; }

        /// <summary>
        /// Gets or sets the mouse activity.
        /// </summary>
        public int MouseActivity { get; set; }

        /// <summary>
        /// Gets or sets the attention span.
        /// </summary>
        public int AttentionSpan { get; set; }

        /// <summary>
        /// Gets or sets the productive.
        /// </summary>
        public byte Productive { get; set; }
    }
}
