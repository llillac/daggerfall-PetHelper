using System;
using DaggerfallWorkshop.Game;
using UnityEngine;

namespace Game.Pet
{
    public class LootTimer : MonoBehaviour
    {
        private float _currentTime;
        private int _lastTime;
        private int _totalTime;
        private bool _isRunning;

        private Action _onTimerUp;

        public void StartTimer(int totalTime, Action onTimerUp)
        {
            if (_isRunning) return;

            _onTimerUp = onTimerUp;
            _totalTime = totalTime;
            _currentTime = 0;
            _lastTime = 0;
            _isRunning = true;
        }

        private void Update()
        {
            if (GameManager.IsGamePaused) return;

            if (!_isRunning) return;

            _currentTime += Time.deltaTime;
            _lastTime = (int) _currentTime;

            if (_lastTime != _totalTime) return;

            _onTimerUp?.Invoke();
            StopTimer();
        }

        public void StopTimer()
        {
            _isRunning = false;
            _onTimerUp = null;
        }
    }
}