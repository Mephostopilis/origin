using System;
using System.Threading;

namespace Maria.Network
{
    public class TimeSync
    {
        public class Lag
        {
            public int L { get; set; }
            public double E { get; set; }
        }

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
            return (ulong)(dt.Ticks / 10 / 1000 / 10);
        }

        public int LocalTime()
        {
            ulong ct = GetTime();
            return (int)(ct - _startTime);
        }

        /// <summary>
        /// 更正globaltime
        /// </summary>
        /// <param name="requestTime">c2s时候的localtime</param>
        /// <param name="globalTime">s的</param>
        /// <returns></returns>
        public Lag Sync(int requestTime, int globalTime)
        {
            Lag res = new Lag();
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
            res.L = lag / 2;
            if (_timeShift == 0 || _timeElapse <= 0)
            {
                res.E = 0;
            }
            else
            {
                res.E = (double)(_timeShift / _timeElapse);
            }
            return res;
        }

        public int[] GlobalTime()
        {
            int[] res = new int[2];
            if (_timeElapse < 0)
            {
                return res;
            }
            ulong now = GetTime();
            int localTime = (int)(now - _startTime);
            int lag = (_estimateTo - _estimateFrom) / 2;
            int estimate = _estimateFrom + lag + (localTime - _timeSync);
            res[0] = estimate;
            res[1] = lag;
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
