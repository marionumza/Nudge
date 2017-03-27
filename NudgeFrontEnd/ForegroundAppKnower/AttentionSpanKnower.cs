// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AttentionSpanKnower.cs" company="Sammy Guergachi">
//   Sammy Guergachi 2017
// </copyright>
// <summary>
//   The attention span knower.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace NudgeHarvester
{
    /// <summary>
    /// The attention span knower.
    /// </summary>
    public class AttentionSpanKnower
    {
        /// <summary>
        /// The _current span.
        /// </summary>
        private int currentSpan;

        /// <summary>
        /// The increment.
        /// </summary>
        /// <param name="amount">
        /// The amount.
        /// </param>
        public void Increment(int amount)
        {
            this.currentSpan += amount;
        }

        /// <summary>
        /// The is span greater than.
        /// </summary>
        /// <param name="amount">
        /// The amount.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool IsSpanGreaterThan(int amount)
        {
            return this.currentSpan > amount;
        }

        /// <summary>
        /// The grab attention span.
        /// </summary>
        /// <returns>
        /// The <see cref="int"/>.
        /// </returns>
        public int GrabAttentionSpan()
        {
            // Send current span then clear it
            int tempSpan = this.currentSpan;
            this.currentSpan = 0;

            return tempSpan;
        }

        /// <summary>
        /// The get attention span.
        /// </summary>
        /// <returns>
        /// The <see cref="int"/>.
        /// </returns>
        public int GetAttentionSpan()
        {
            return this.currentSpan;
        }
    }
}
