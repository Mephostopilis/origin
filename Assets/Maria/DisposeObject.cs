using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Maria {
    public class DisposeObject : IDisposable {

        protected bool _disposed = false;

        ~DisposeObject() {
            Dispose(false);
        }

        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing) {
            if (_disposed) {
                return;
            }
            if (disposing) {
                // 清理托管资源，调用自己管理的对象的Dispose方法
            }
            // 清理非托管资源

            _disposed = true;
        }

    }
}
