using System;
using System.Runtime.InteropServices;
using MpcHcDotNet.Win32Data;

namespace MpcHcDotNet {
    /// <summary>
    /// Wraps a message from the host to be used as event arguments.
    /// </summary>
    public class Win32EventArgs : EventArgs {
        /// <summary>
        /// 
        /// </summary>
        public CopyDataStruct CopyData { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Payload { get; set; }


        public Win32EventArgs(CopyDataStruct cd) {
            CopyData = cd;
            Payload = Marshal.PtrToStringAuto(cd.lpData, cd.cbData);
            Payload = Payload.Substring(0, Payload.IndexOf("\0")).Replace("\0", String.Empty);
        }
    }
}
