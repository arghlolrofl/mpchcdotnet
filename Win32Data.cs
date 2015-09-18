using System;

namespace MpcHcDotNet.Win32Data {
    /// <summary>
    /// DataContainer for incoming Windows Message
    /// </summary>
    public class Incoming {
        /// <summary>
        /// Command received
        /// </summary>
        public ApiCmd.Received ReceivedCommand { get; set; } 
        /// <summary>
        /// Payload/data/arguments for command received
        /// </summary>
        public string Payload { get; set; }

        public Incoming(Win32EventArgs args) {
            Payload = args.Payload;

            ReceivedCommand = (ApiCmd.Received)args.CopyData.dwData;
        }
    }


    /// <summary>
    /// Contains data to be passed to another 
    /// application by the WM_COPYDATA message. 
    /// </summary>
    public struct CopyDataStruct {
        /// <summary>
        /// The data to be passed to the receiving application. 
        /// </summary>
        public IntPtr dwData;
        /// <summary>
        /// The size, in bytes, of the data pointed to by the lpData member.
        /// </summary>
        public int cbData;
        /// <summary>
        /// The data to be passed to the receiving application. 
        /// This member can be NULL. 
        /// </summary>
        public IntPtr lpData;
    }
}
