using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Threading;
using MpcHcDotNet.Model;
using MpcHcDotNet.Win32Data;

namespace MpcHcDotNet {
    public delegate void ApiHandler(Win32EventArgs args);
    public delegate void LogHandler(string msg);

    public class Api : INotifyPropertyChanged {
        #region INotifyPropertyChanged Member
        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string pName) {
            PropertyChangedEventHandler handler = PropertyChanged;

            if (handler != null)
                handler(this, new PropertyChangedEventArgs(pName));
        }
        #endregion

        #region Imports
        [DllImport("user32.dll")]
        private static extern int SendMessage(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);
        #endregion

        #region Events
        public event LogHandler NewLogMessage;

        public event ApiHandler WMReceived;
        public event ApiHandler ConnectedToHost;
        public event ApiHandler ChangingCurrentPlayback;
        public event ApiHandler NotifyingSeek;
        public event ApiHandler ListingSubtitles;
        public event ApiHandler ListingAudioPids;
        public event ApiHandler EndingPlayback;

        public event ApiHandler Playing;
        public event ApiHandler Pausing;
        public event ApiHandler Stopping;
        #endregion

        #region Properties
        private readonly System.Timers.Timer timer;

        /// <summary>
        /// Source handle for WM_COPYDATA
        /// </summary>
        public IntPtr HWndFrom { get; set; }
        /// <summary>
        /// Destination handle for WM_COPYDATA
        /// </summary>
        public IntPtr HWndTo { get; set; }
        /// <summary>
        /// Handle to message data
        /// </summary>
        public IntPtr HCopyData { get; set; }
        /// <summary>
        /// MPC HC executable
        /// </summary>
        public FileInfo MpcHcExeFile { get; set; }

        private PlayInfo _nowPlaying;
        public PlayInfo NowPlaying {
            get { return _nowPlaying; }
            set {
                _nowPlaying = value;
                RaisePropertyChanged("NowPlaying");

                if(NowPlaying == null)
                    NowPlaying.PropertyChanged -= OnNowPlayingChanged;
                else
                    NowPlaying.PropertyChanged += OnNowPlayingChanged;
            }
        }
        private bool isConnected;
        public bool IsConnected {
            get { return isConnected; }
            set {
                if (isConnected == value) return;

                isConnected = value;
                RaisePropertyChanged("IsConnected");

                sendWinMessage(ApiCmd.Send.GETNOWPLAYING, String.Empty);
            }
        }
        #endregion

        #region Constructor(s)
        /// <summary>
        /// 
        /// </summary>
        public Api(IntPtr hWndFrom) {
            HWndFrom = hWndFrom;
            WMReceived += OnWMReceived;

            timer = new System.Timers.Timer(9876);
            timer.AutoReset = true;
            timer.Elapsed += (s, a) => GetCurrentPosition();
        }
        #endregion


        public IntPtr WndProc(IntPtr hwnd, int payload, IntPtr wParam, IntPtr lParam, ref bool handled) {
            if (payload == (int)WmType.WM_COPYDATA) {
                var cd = new CopyDataStruct();
                cd = (CopyDataStruct)Marshal.PtrToStructure(lParam, typeof(CopyDataStruct));

                handled = true;
                WMReceived(new Win32EventArgs(cd));
            }

            return IntPtr.Zero;
        }


        /// <summary>
        /// Initializes the connection to the current mpc-hc.exe process.
        /// </summary>
        /// <param name="path"></param>
        public void Connect(FileInfo executable) {
            if (!executable.Exists)
                throw new FileNotFoundException(executable.FullName);

            MpcHcExeFile = executable;

            runProcess(MpcHcExeFile.FullName, String.Format("/slave {0}", HWndFrom));
        }

        /// <summary>
        /// Opens a new file.
        /// </summary>
        /// <param name="path"></param>
        public void OpenFile(string path) {
            sendWinMessage(ApiCmd.Send.OPENFILE, path);
        }
        /// <summary>
        /// Plays or pauses the currently playing media file.
        /// </summary>
        public void PlayPause() {
            sendWinMessage(ApiCmd.Send.PLAYPAUSE, "");
        }
        /// <summary>
        /// Receive file info of currently playing media.
        /// </summary>
        public void GetNowPlaying() {
            sendWinMessage(ApiCmd.Send.GETNOWPLAYING, "");
        }
        /// <summary>
        /// 
        /// </summary>
        public void GetCurrentPosition() {
            if (NowPlaying != null)
                sendWinMessage(ApiCmd.Send.GETCURRENTPOSITION, String.Empty);
        }

        public void SetSubtitle(int subId) {
            sendWinMessage(ApiCmd.Send.SETSUBTITLETRACK, subId.ToString());
        }

        public void SetAudioTrack(int trackId) {
            sendWinMessage(ApiCmd.Send.SETAUDIOTRACK, trackId.ToString());
        }


        /// <summary>
        /// Starts a process with given arguments
        /// </summary>
        /// <param name="path">Executable file</param>
        /// <param name="args">Commandline arguments</param>
        private void runProcess(string path, string args) {
            var psi = new ProcessStartInfo(path);
            psi.Arguments = args;

            var p = new Process();
            p.StartInfo = psi;

            p.Start();
        }
        /// <summary>
        /// Sends a command to with given arguments to the
        /// MPC HC executable process.
        /// </summary>
        /// <param name="sendCmd"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        private int sendWinMessage(ApiCmd.Send sendCmd, string args) {
            int result = -1;
            var dataStruct = packParams((uint)sendCmd, args);

            // Initialize unmanged memory to hold the struct.
            IntPtr argsPtr = Marshal.AllocHGlobal(Marshal.SizeOf(dataStruct));

            try {
                // Copy the struct to unmanaged memory.
                Marshal.StructureToPtr(dataStruct, argsPtr, false);
                // Send message to mpc-hc.exe
                result = SendMessage(HWndTo, (uint)WmType.WM_COPYDATA, HWndFrom, argsPtr);
            } catch (Exception) {
                throw;
            } finally {
                // Free the unmanaged memory.
                Marshal.FreeHGlobal(argsPtr);
            }

            return result;
        }
        /// <summary>
        /// Packs the command into a WM_COPYDATA structure.
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        private CopyDataStruct packParams(uint cmd, string param) {
            var data = new CopyDataStruct {
                dwData = new IntPtr((Int64) cmd),
                cbData = (param.Length + 1) * Marshal.SystemDefaultCharSize,
                lpData = Marshal.StringToHGlobalUni(param)
            };

            return data;
        }



        private void OnNowPlayingChanged(object sender, PropertyChangedEventArgs args) {
            if (args.PropertyName != "Path") return;

            if (String.IsNullOrEmpty(NowPlaying.Path))
                return;
            
            GetCurrentPosition();

            if (!timer.Enabled)
                timer.Start();
        }

        private void OnWMReceived(Win32EventArgs args) {
            var w32Data = new Incoming(args);

            NewLogMessage(String.Format(
                "### {0} => {1}", w32Data.ReceivedCommand, w32Data.Payload
            ));

            switch (w32Data.ReceivedCommand) {
                case ApiCmd.Received.CONNECT:
                    int to = Convert.ToInt32(w32Data.Payload);
                    HWndTo = new IntPtr(to);
                    IsConnected = to > 0;
                    ConnectedToHost(null);
                    break;
                case ApiCmd.Received.STATE:
                    switch ((ApiCmd.LoadState) Convert.ToInt32(w32Data.Payload)) {
                        case ApiCmd.LoadState.MLS_CLOSED:
                            break;
                        case ApiCmd.LoadState.MLS_CLOSING:
                            break;
                        case ApiCmd.LoadState.MLS_LOADED:
                            break;
                        case ApiCmd.LoadState.MLS_LOADING:
                            break;
                    }
                    break;
                case ApiCmd.Received.PLAYMODE:
                    switch ((ApiCmd.PlayState) Convert.ToInt32(w32Data.Payload)) {
                        case ApiCmd.PlayState.PS_PLAY:
                            Playing(args);
                            if (!timer.Enabled) timer.Start();
                            break;
                        case ApiCmd.PlayState.PS_PAUSE:
                            Pausing(args);
                            if(timer.Enabled) timer.Stop();
                            break;
                        case ApiCmd.PlayState.PS_STOP:
                            Stopping(args);
                            if (timer.Enabled) timer.Stop();
                            break;
                        case ApiCmd.PlayState.PS_UNUSED:
                            break;
                    }
                    break;
                case ApiCmd.Received.NOWPLAYING:
                    NowPlaying = new PlayInfo();

                    string[] info = w32Data.Payload.Split('|');
                    if (!String.IsNullOrEmpty(info[0]))
                        NowPlaying.Title = info[0];
                    if (!String.IsNullOrEmpty(info[1]))
                        NowPlaying.Author = info[1];
                    if (!String.IsNullOrEmpty(info[2]))
                        NowPlaying.Description = info[2];
                    if (!String.IsNullOrEmpty(info[3]))
                        NowPlaying.Path = info[3];
                    if (!String.IsNullOrEmpty(info[4]))
                        NowPlaying.Duration = info[4];

                    ChangingCurrentPlayback(args);
                    break;
                case ApiCmd.Received.LISTSUBTITLETRACKS:
                    ListingSubtitles(args);
                    break;
                case ApiCmd.Received.LISTAUDIOTRACKS:
                    ListingAudioPids(args);
                    break;
                case ApiCmd.Received.CURRENTPOSITION:
                case ApiCmd.Received.NOTIFYSEEK:
                    if (w32Data.Payload.Contains(".")) {
                        NowPlaying.CurrentSecond = Convert.ToInt32(
                            w32Data.Payload.Substring(0, w32Data.Payload.IndexOf('.'))
                        );
                    } else {
                        NowPlaying.CurrentSecond = Convert.ToInt32(w32Data.Payload);
                    }
                    if (NotifyingSeek != null)
                        NotifyingSeek(args);
                    break;
                case ApiCmd.Received.NOTIFYENDOFSTREAM:
                    if (timer.Enabled) timer.Stop();
                    EndingPlayback(args);
                    break;
                case ApiCmd.Received.PLAYLIST:
                    break;
                default:
                    break;
            }
        }
    }
}
