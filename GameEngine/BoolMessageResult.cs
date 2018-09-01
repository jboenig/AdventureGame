using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventureGameEngine
{
    /// <summary>
    /// This class is used to return a boolean result and
    /// a message from method calls.
    /// </summary>
    public sealed class BoolMessageResult
    {
        public static BoolMessageResult Success = new BoolMessageResult(true, string.Empty);

        public BoolMessageResult(bool isSuccess, string message)
        {
            this.IsSuccess = isSuccess;
            this.Message = message;
        }

        public bool IsSuccess
        {
            get;
            private set;
        }

        public string Message
        {
            get;
            private set;
        }
    }
}
