using UnityEngine;
using System.Collections;

namespace ANR
{
    public class GestureCounter : MonoBehaviour
    {
        #region Properties

        public enum CounterState
        {
            Run,
            Stop,
            Pause
        }

        public CounterState CurrentState
        {
            get { return _currentState; }
        }

        private CounterState _currentState = CounterState.Stop;
        private float _step;
        private float _maxTimer;
        private float _timer;
        private float _preTimer;

        private EndTimer _endTimerFunction;
        private EndEverySeconds _endEverySeconds;
        private UpdatingPercentage _updating;

        #endregion

        public delegate void EndTimer();

        public delegate void EndEverySeconds(int secs);

        public delegate void UpdatingPercentage(float percent);

        // Use this for initialization
        void Start()
        {
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            switch (CurrentState)
            {
                case CounterState.Run:

                    _timer -= Time.fixedDeltaTime * _step;

                    if (_updating != null)
                    {
                        _updating(1 - _timer * 1.0f / _maxTimer);
                    }

                    if (Mathf.Abs(_timer - _preTimer) >= 1)
                    {
                        _preTimer = _timer;
                        if (_endEverySeconds != null)
                        {
                            _endEverySeconds(Mathf.RoundToInt(_preTimer));
                        }
                    }

                    if (_timer < 0)
                    {
                        _timer = _maxTimer;
                        _currentState = CounterState.Stop;

                        if (_endTimerFunction != null)
                        {
                            _endTimerFunction();
                        }
                    }

                    break;
                case CounterState.Pause:
                    break;
                case CounterState.Stop:
                    _preTimer = _timer = _maxTimer;
                    break;
            }
        }

        public void StartTimer(float _maxTimer, EndTimer endFunc)
        {
            _step = 1;
            _timer = this._maxTimer = _maxTimer;
            _endTimerFunction = endFunc;
            _endEverySeconds = null;
            _updating = null;
            _currentState = CounterState.Run;
        }

        public void StartTimerUpdateSeconds(float _maxTimer, EndTimer endFunc, EndEverySeconds endSecs = null)
        {
            _step = 1;
            _timer = this._maxTimer = _maxTimer;
            _endTimerFunction = endFunc;
            _endEverySeconds = endSecs;
            _updating = null;
            _currentState = CounterState.Run;
        }

        public void StartTimerUpdatePercentage(float _maxTimer, EndTimer endFunc,
            UpdatingPercentage updatingFunc = null)
        {
            _step = 1;
            _timer = this._maxTimer = _maxTimer;
            _endTimerFunction = endFunc;
            _endEverySeconds = null;
            _updating = updatingFunc;
            _currentState = CounterState.Run;
        }

        public void StopTimer()
        {
            _currentState = CounterState.Stop;
            _endTimerFunction = null;
            _endEverySeconds = null;
            //Debug.Log ("Stop");
        }

        public void PauseTimer()
        {
            if (_currentState == CounterState.Run)
            {
                _currentState = CounterState.Pause;
                //Debug.Log("Pause");
            }
        }

        public void ContinueTimer()
        {
            if (_currentState == CounterState.Pause)
            {
                _currentState = CounterState.Run;
                //Debug.Log("Continue");
            }
        }
    }
}