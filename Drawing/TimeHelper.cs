﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Drawing
{
    public class TimeHelper
    {
        private Stopwatch _stopwatch;

        private int _framesCouner = 0;

        private int _fps = 0;
        public int FPS { get => _fps; }

        private long _previousFPSMeasurementTime;

        private long _previousTicks;

        private float _time;
        public float Time { get => _time; }

        public float prevTime = Environment.TickCount;
        public float newTime = Environment.TickCount;
        public float DeltaT { get { return newTime - prevTime; } }

        public TimeHelper()
        {
            _stopwatch = new Stopwatch();
            Reset();
        }

        public void Reset()
        {
            _stopwatch.Reset();
            _framesCouner = 0;
            _fps = 0;
            _stopwatch.Start();
            _previousFPSMeasurementTime = _stopwatch.ElapsedMilliseconds;
            _previousTicks = _stopwatch.ElapsedTicks;
        }

        public void Update()
        {
            prevTime = newTime;
            newTime = Environment.TickCount;
            long ticks = _stopwatch.ElapsedTicks;
            _time = (float)ticks / TimeSpan.TicksPerSecond;
            _previousTicks = ticks;

            _framesCouner++;
            if (_stopwatch.ElapsedMilliseconds - _previousFPSMeasurementTime >= 1000)
            {
                _fps = _framesCouner;
                _framesCouner = 0;
                _previousFPSMeasurementTime = _stopwatch.ElapsedMilliseconds;
            }
        }
    }
}
