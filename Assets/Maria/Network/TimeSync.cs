using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Maria.Network
{
    class TimeSync
    {
        private ulong _startTime = 0;
        private int _timeElapse = 0;
        private int _timeShift = 0;
        private int _timeSync = 0;
        private int _estimateFrom = 0;
        private int _estimateTo = 0;

        public TimeSync()
        {
            _startTime = GetTime();
            _timeElapse = -1;
        }

        private ulong GetTime()
        {
            ulong t = 0;
            DateTime dt = DateTime.Now;
            t = (ulong)dt.Second * 100;
            t += (ulong)dt.Millisecond / 10;
            return t;
        }

        public ulong LocalTime()
        {
            ulong ct = GetTime();
            return ct - _startTime;
        }

        public ulong[] Sync(int requestTime, int globalTime)
        {
            ulong[] res = new ulong[2];
            ulong now = GetTime();
            int localTime = (int)(now - _startTime);
            int lag = (int)(localTime - requestTime);
            int elapseFromLastSync = localTime - _timeSync;
            if (localTime < requestTime)
            {
                // invalid sync
                return res;
            }
            if (_timeElapse < 0 || elapseFromLastSync < 0)
            {
                // first time sync
                _timeElapse = 0;
                _timeShift = 0;
                _timeSync = localTime;
                _estimateFrom = globalTime;
                _estimateTo = globalTime + lag;
            }
            else
            {
                int estimate = globalTime + lag / 2;
                _estimateFrom += elapseFromLastSync;
                _estimateTo += elapseFromLastSync;
                int estimateLast = _estimateFrom + (_estimateTo - _estimateFrom) / 2;
                _timeElapse += elapseFromLastSync;
                _timeShift += estimate - estimateLast;
                _timeSync = localTime;
                if (estimate < _estimateFrom || estimate > _estimateTo)
                {
                    _estimateFrom = globalTime;
                    _estimateTo = globalTime + lag;
                } else
                {
                    if (globalTime > _estimateFrom)
                    {
                        _estimateFrom = globalTime;
                    }
                    if (globalTime + lag < _estimateTo)
                    {
                        _estimateTo = globalTime + lag;
                    }
                }
            }
            res[0] = (ulong)lag / 2;
            res[1] = (ulong)( _timeShift / _timeElapse);
            return res;
        }

        public ulong[] GlobalTime()
        {
            ulong[] res = new ulong[2];
            if (_timeElapse < 0)
            {
                return res;
            }
            ulong now = GetTime();
            int localTime = (int)(now - _startTime);
            int lag = (_estimateTo - _estimateFrom) / 2;
            int estimate = _estimateFrom + lag + (localTime - _timeSync);
            res[0] = (ulong)estimate;
            res[1] = (ulong)lag;
            return res;
        }

        /// <summary>
        /// debug use
        /// </summary>
        /// <param name="ti"></param>
        public void Sleep(int ti)
        {
            Thread.Sleep(ti * 10);
        }
    }
}
