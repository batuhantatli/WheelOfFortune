namespace TimerSystem
{
    /// <summary>
    /// Structure representing timer data containing various properties related to the timer's state and progress.
    /// </summary>
    public struct TimerData
    {
        /// <summary>
        /// The elapsed time (in seconds) since the timer started.
        /// </summary>
        public float TimeElapsed;

        /// <summary>
        /// The remaining time (in seconds) until the timer completes its duration.
        /// </summary>
        public float TimeRemaining;

        /// <summary>
        /// The ratio of elapsed time to the total duration (expressed as a fraction between 0 and 1).
        /// </summary>
        public float RatioElapsed;

        /// <summary>
        /// The ratio of remaining time to the total duration (expressed as a fraction between 0 and 1).
        /// </summary>
        public float RatioRemaining;

        /// <summary>
        /// The current world time (in seconds) when the TimerData is updated.
        /// </summary>
        public float WorldTime;

        /// <summary>
        /// The expected time (in seconds) when the timer will finish its duration.
        /// </summary>
        public float FinishTime;

        /// <summary>
        /// The time interval (in seconds) between the last update and the current update of the TimerData.
        /// </summary>
        public float TimeDelta;
    }
}