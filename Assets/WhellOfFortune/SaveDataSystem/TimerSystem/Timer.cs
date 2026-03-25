using UnityEngine;
using UnityEngine.Events;

namespace TimerSystem
{
    public class Timer
    {
        #region Public Properties

        /// <summary>
        /// The total duration (in seconds) of the timer.
        /// </summary>
        public float Duration { get; private set; }

        /// <summary>
        /// A flag indicating whether the timer should be looped/restarted after it completes.
        /// </summary>
        public bool IsLooped { get; set; }

        /// <summary>
        /// A flag indicating whether the timer has completed its duration.
        /// </summary>
        public bool IsCompleted { get; private set; }

        /// <summary>
        /// A flag indicating whether the timer is currently running.
        /// </summary>
        public bool IsRunning { get; private set; }

        /// <summary>
        /// A flag indicating whether the timer uses real-time or game-time.
        /// Real-time is unaffected by changes to the timescale of the game (e.g., pausing, slow-mo),
        /// while game-time is affected by these changes.
        /// </summary>
        public bool UsesRealTime { get; private set; }

        /// <summary>
        /// A flag indicating whether the timer is currently paused.
        /// </summary>
        public bool IsPaused
        {
            get { return this._timeBeforePause.HasValue; }
        }

        /// <summary>
        /// A flag indicating whether the timer is currently stopped.
        /// </summary>
        public bool IsStopped
        {
            get { return this._timeBeforeStop.HasValue; }
        }

        /// <summary>
        /// A flag indicating whether the timer has either completed or been stopped.
        /// </summary>
        public bool IsFinished
        {
            get { return this.IsCompleted || this.IsStopped; }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Constructor for the Timer class that initializes the timer with the provided parameters.
        /// </summary>
        /// <param name="duration">The total duration (in seconds) of the timer.</param>
        /// <param name="isLooped">Flag indicating whether the timer should be looped (restarted) after completion.</param>
        /// <param name="usesRealTime">Flag indicating whether the timer uses real-time or game-time.</param>
        /// <param name="onFinish">UnityEvent invoked when the timer completes its duration.</param>
        /// <param name="onUpdate">UnityEvent invoked each frame with updated TimerData during the timer's execution.</param>
        public Timer(float duration, bool isLooped, bool usesRealTime, UnityEvent onFinish, UnityEvent<TimerData> onUpdate)
        {
            this.Duration = duration;
            this.IsLooped = isLooped;
            this.UsesRealTime = usesRealTime;

            this._onFinish = onFinish;
            this._onUpdate = onUpdate;

            this._startTime = this.GetWorldTime();
            this._lastUpdatedTime = this._startTime;
        }

        public void AddOnFinishListener(UnityAction action)
        {
            this._onFinish ??= new UnityEvent();
            this._onFinish.AddListener(action);
        }

        public void AddOnUpdateListener(UnityAction<TimerData> action)
        {
            this._onUpdate ??= new UnityEvent<TimerData>();
            this._onUpdate.AddListener(action);
        }

        /// <summary>
        /// Stops the timer's execution. If the timer is already finished, it has no effect.
        /// </summary>
        public void Stop()
        {
            if (this.IsFinished)
            {
                return;
            }

            this._timeBeforeStop = this.GetTimeElapsed();
            this._timeBeforePause = null;
        }

        /// <summary>
        /// Completes the timer's execution. Invokes the onFinish UnityEvent and stops the timer.
        /// If the timer is already finished or looped timer, it has no effect.
        /// </summary>
        public void Complete()
        {
            if (this.IsFinished)
            {
                return;
            }

            this._timeBeforeStop = this.GetTimeElapsed();
            this._timeBeforePause = null;
            this.IsCompleted = true;

            if (this._onFinish != null)
            {
                this._onFinish.Invoke();
            }
        }

        /// <summary>
        /// Pauses the timer's execution. If the timer is already paused or finished, it has no effect.
        /// </summary>
        public void Pause()
        {
            if (this.IsPaused || this.IsFinished)
            {
                return;
            }

            this._timeBeforePause = this.GetTimeElapsed();
        }

        /// <summary>
        /// Resumes the timer's execution if it was previously paused. If the timer is not paused or finished, it has no effect.
        /// </summary>
        public void Resume()
        {
            if (!this.IsPaused || this.IsFinished)
            {
                return;
            }

            this._timeBeforePause = null;
        }

        /// <summary>
        /// Returns the elapsed time (in seconds) since the timer started. If the timer has finished or the world time exceeds the finish time,
        /// it returns the total duration of the timer to prevent negative values.
        /// </summary>
        /// <returns>The elapsed time in seconds.</returns>
        public float GetTimeElapsed()
        {
            if (this.IsFinished || this.GetWorldTime() >= this.GetFinishTime())
            {
                return this.Duration;
            }

            return this._timeBeforeStop ??
                   this._timeBeforePause ??
                   this.GetWorldTime() - this._startTime;
        }

        /// <summary>
        /// Returns the remaining time (in seconds) until the timer completes its duration.
        /// </summary>
        /// <returns>The remaining time in seconds.</returns>
        public float GetTimeRemaining()
        {
            return this.Duration - this.GetTimeElapsed();
        }

        /// <summary>
        /// Returns the ratio of elapsed time to the total duration (as a fraction between 0 and 1).
        /// Optionally, the ratio can be returned as a percentage if 'inPercentage' is true.
        /// </summary>
        /// <param name="inPercentage">If true, the ratio is returned as a percentage.</param>
        /// <returns>The ratio of elapsed time to total duration.</returns>
        public float GetRatio(bool inPercentage = false)
        {
            return (this.GetTimeElapsed() / this.Duration) * (inPercentage ? 100 : 1);
        }

        /// <summary>
        /// Returns the ratio of remaining time to the total duration (as a fraction between 0 and 1).
        /// Optionally, the ratio can be returned as a percentage if 'inPercentage' is true.
        /// </summary>
        /// <param name="inPercentage">If true, the ratio is returned as a percentage.</param>
        /// <returns>The ratio of remaining time to total duration.</returns>
        public float GetRatioRemaining(bool inPercentage = false)
        {
            return (this.GetTimeRemaining() / this.Duration) * (inPercentage ? 100 : 1);
        }

        /// <summary>
        /// Returns the current world time (in seconds) based on whether the timer uses real-time or game-time.
        /// </summary>
        /// <returns>The current world time in seconds.</returns>
        public float GetWorldTime()
        {
            return this.UsesRealTime ? Time.realtimeSinceStartup : Time.time;
        }

        /// <summary>
        /// Returns the expected time (in seconds) when the timer will finish its duration.
        /// </summary>
        /// <returns>The expected finish time in seconds.</returns>
        public float GetFinishTime()
        {
            return this._startTime + this.Duration;
        }

        /// <summary>
        /// Returns the time interval (in seconds) between the last update and the current update of the TimerData.
        /// </summary>
        /// <returns>The time interval between the last two updates in seconds.</returns>
        public float GetTimeDelta()
        {
            return this.GetWorldTime() - this._lastUpdatedTime;
        }

        public void UpdateStartTime(float startTime = -1)
        {
            this._startTime = startTime >= 0 ? startTime : this.GetWorldTime();
        }

        /// <summary>
        /// Updates the timer's state and executes associated events. This method should be called each frame
        /// to keep the timer active and to trigger events at the appropriate times.
        /// </summary>
        public void Update()
        {
            if (this.IsFinished) return;

            if (this.IsPaused)
            {
                this._startTime += this.GetTimeDelta();
                this._lastUpdatedTime = this.GetWorldTime();
                return;
            }

            this._lastUpdatedTime = this.GetWorldTime();

            TimerData frameData = new TimerData
            {
                TimeElapsed = this.GetTimeElapsed(),
                TimeRemaining = this.GetTimeRemaining(),
                RatioElapsed = this.GetRatio(),
                RatioRemaining = this.GetRatioRemaining(),
                WorldTime = this.GetWorldTime(),
                FinishTime = this.GetFinishTime(),
                TimeDelta = this.GetTimeDelta()
            };

            if (this._onUpdate != null)
            {
                this._onUpdate.Invoke(frameData);
            }

            if (this.GetWorldTime() >= this.GetFinishTime())
            {
                if (this._onFinish != null)
                {
                    this._onFinish.Invoke();
                }

                if (this.IsLooped)
                {
                    this._startTime = this.GetWorldTime();
                }
                else
                {
                    this.IsCompleted = true;
                }
            }
        }

        #endregion

        #region Private Properties

        /// <summary>
        /// The time (in seconds) when the timer started or was last resumed.
        /// </summary>
        private float _startTime;

        /// <summary>
        /// The time (in seconds) when the TimerData was last updated or when the timer was last resumed.
        /// </summary>
        private float _lastUpdatedTime;

        /// <summary>
        /// The time (in seconds) when the timer was stopped. Null if the timer is not stopped.
        /// </summary>
        private float? _timeBeforeStop;

        /// <summary>
        /// The time (in seconds) when the timer was paused. Null if the timer is not paused.
        /// </summary>
        private float? _timeBeforePause;

        /// <summary>
        /// The UnityEvent invoked when the timer completes its duration.
        /// </summary>
        private UnityEvent _onFinish;

        /// <summary>
        /// The UnityEvent<TimerData> invoked each frame with updated TimerData during the timer's execution.
        /// </summary>
        private UnityEvent<TimerData> _onUpdate;

        #endregion
    }
}