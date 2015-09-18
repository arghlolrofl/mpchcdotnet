namespace MpcHcDotNet {
    public enum WmType {
        WM_COPYDATA = 0x4A
    }

    public class ApiCmd {
        ///<Summary>
        ///List of the constants related to commands sent from MPC
        ///</ Summary>
        ///<Remarks> </ remarks>
        public enum Received {
            ///<Summary>
            ///Befehl RELATED
            ///</ Summary>
            ///<Remarks> Verbindung zwischen MPC und dem Objektmanagement etabliert
            ///Und umgekehrt </ remarks>
            CONNECT = 0x50000000,

            ///<Summary>
            ///Befehlszustand
            ///</ Summary>
            ///<Remarks> Zeigt an, dass die Nachricht gibt den aktuellen Status der MPC.
            ///Finden Sie in der Liste für den angegebenen Loadstate. </ Remarks>
            STATE = 0x50000001,

            ///<Summary>
            ///Befehl PLAY MODE
            ///</ Summary>
            ///<Remarks> Gibt den Stand der Dinge in MPC.
            ///Finden Sie in der Liste für die angegebenen PLAYSTATE </ remarks>
            PLAYMODE = 0x50000002,

            ///<Summary>
            ///Befehl nowplaying
            ///</ Summary>
            ///<Remarks> Gibt an, dass Informationen in der Nachricht in der zurück
            ///Format: "Titel | Autor | Beschreibung | Dateiname | Dauer". Der Separator ist
            ///| (Pipe). "|", Wo es in der Zeichenfolge vorhanden ist Charakter, Wille
            ///Ersetzt' \ | ", um aus dem Abscheider zu unterscheiden. </ Remarks>
            NOWPLAYING = 0x50000003,

            ///<Summary>
            ///Befehl Liste der Tracks SOTTOTITOLO
            ///</ Summary>
            ///<Remarks> Gibt an, dass die Nachricht enthält eine Liste der Titel
            ///Durch das Zeichen Rohr (das eine vom Trenn unterscheiden
            ///Zeichen in der Zeichenfolge, die Zeichenfolge "|" "\ |") ersetzt wird.
            ///Message Format "track1 | track2 | ... | TRACKn." Der letzte Track ist
            ///Aktiv. Wenn er zurückkehrt "-1" nicht vorhanden ist, keine Spur. wenn
            ///Zurückgegeben "-2" ist keine Untertiteldatei geladen. </ Remarks>
            LISTSUBTITLETRACKS = 0x50000004,

            ///<Summary>
            ///Befehl Liste der Tracks AUDIO
            ///</ Summary>
            ///<Remarks> Gibt an, dass die Nachricht enthält eine Liste der Titel
            ///Durch das Zeichen Rohr (das eine vom Trenn unterscheiden
            ///Zeichen in der Zeichenfolge, die Zeichenfolge "|" "\ |") ersetzt wird.
            ///Message Format "track1 | track2 | ... | TRACKn." Der letzte Track ist
            ///Aktiv. Wenn er zurückkehrt "-1" nicht vorhanden ist, keine Spur. wenn
            ///Zurückgegeben "-2" ist keine Audio-Dateien hochladen. </ Remarks>
            LISTAUDIOTRACKS = 0x50000005,

            ///<Summary>
            ///Befehlsstrom POSITION
            ///</ Summary>
            ///<Remarks> Gibt an, dass die Nachricht die Position in Sekunden enthält
            ///Auf dem aktuellen Stream. </ Remarks>
            CURRENTPOSITION = 0x50000007,

            ///<Summary>
            ///Befehlsbenachrichtigung JUMP
            ///</ Summary>
            ///<Remarks> vorsieht, dass ein Zeitsprung gemacht und die wurde
            ///Nachricht enthält die aktuelle Position </ remarks>
            NOTIFYSEEK = 0x50000008,

            ///<Summary>
            ///Befehl MITTEILUNG AN DAS ENDE DER STREAM
            ///</ Summary>
            ///<Remarks> Gibt an, dass Sie das Ende des Streams erreicht haben. die Botschaft
            ///Enthält keine Daten </ remarks>
            NOTIFYENDOFSTREAM = 0x50000009,

            ///<Summary>
            ///Befehl PLAY LIST
            ///</ Summary>
            ///<Remarks> Gibt an, dass die Nachricht enthält eine Liste der Titel
            ///Durch das Zeichen Rohr (das eine vom Trenn unterscheiden
            ///Zeichen in der Zeichenfolge, die Zeichenfolge "|" "\ |") ersetzt wird.
            ///Message Format "track1 | track2 | ... | TRACKn." Der letzte Track ist
            ///Aktiv. Wenn er zurückkehrt "-1" nicht vorhanden ist, keine Spur. </ Remarks>
            PLAYLIST = 0x50000006,
        }
        ///<Summary>
        ///List of the constants related to the commands recognized by MPC
        ///</ Summary>
        ///<Remarks> </ remarks>
        public enum Send : uint {
            ///<Summary>
            ///OPEN FILE Command
            ///</ Summary>
            ///<Remarks> The message shall contain the name and file path </ remarks>
            OPENFILE = 0xA0000000,
            ///<Summary>
            ///STOP command
            ///</ Summary>
            ///<Remarks> </ remarks>
            STOP = 0xA0000001,
            ///<Summary>
            ///CLOSE FILE Command
            ///</ Summary>
            ///<Remarks> </ remarks>
            CLOSEFILE = 0xA0000002,
            ///<Summary>
            ///Command PLAY / PAUSE
            ///</ Summary>
            ///<Remarks> </ remarks>
            PLAYPAUSE = 0xA0000003,
            ///<Summary>
            ///Command ADD FILE TO PLAYLIST
            ///</ Summary>
            ///<Remarks> The message must contain the file name and path to be added
            ///In playlist </ remarks>
            ADDTOPLAYLIST = 0xA0001000,
            ///<Summary>
            ///Command EMPTY PLAYLIST
            ///</ Summary>
            ///<Remarks> </ remarks>
            CLEARPLAYLIST = 0xA0001001,
            ///<Summary>
            ///Command PLAY PLAYLIST
            ///</ Summary>
            ///<Remarks> </ remarks>
            STARTPLAYLIST = 0xA0001002,
            ///<Summary>
            ///Command REMOVE FILE FROM THE PLAYLIST
            ///</ Summary>
            ///<Remarks> The message must contain the index of the file to be removed.
            ///COMMAND REQUIRED BUT NOT IMPLEMENTED IN API MPC </ remarks>
            REMOVEFROMPLAYLIST = 0xA0001003, //TODO >>
            ///<Summary>
            ///Command SET POSITION
            ///</ Summary>
            ///<Remarks> The message must contain the new location in seconds
            ///On the current stream </ remarks>
            SETPOSITION = 0xA0002000,
            ///<Summary>
            ///Command SET DELAY AUDIO
            ///</ Summary>
            ///<Remarks> The message must contain the delay in milliseconds
            ///Audio stream </ remarks>
            SETAUDIODELAY = 0xA0002001,
            ///<Summary>
            ///Command SET DELAY Subtitles
            ///</ Summary>
            ///<Remarks> The message must specify the delay, in milliseconds, of the
            ///Subtitles </ remarks>
            SETSUBTITLEDELAY = 0xA0002002,
            ///<Summary>
            ///Command SET FILE CURRENT PLAYLIST
            ///</ Summary>
            ///<Remarks> The message must contain the index of the file to make it active.
            ///CURRENTLY NOT WORKING </ remarks>
            SETINDEXPLAYLIST = 0xA0002003, //>> DOES NOT WORK
            ///<Summary>
            ///SOUNDTRACK SET Command
            ///</ Summary>
            ///<Remarks> The message must specify the index of the audio track
            ///Select </ remarks>
            SETAUDIOTRACK = 0xA0002004,
            ///<Summary>
            ///SET TRACE command SOTTOTITOLO
            ///</ Summary>
            ///<Remarks> The message must specify the index of the subtitle track
            ///To select </ remarks>
            SETSUBTITLETRACK = 0xA0002005,
            ///<Summary>
            ///Operation Restore SUBTITLE TRACKS
            ///</ Summary>
            ///<Remarks> </ remarks>
            GETSUBTITLETRACKS = 0xA0003000,
            ///<Summary>
            ///Operation Restore CURRENT POSITION
            ///</ Summary>
            ///<Remarks> </ remarks>
            GETCURRENTPOSITION = 0xA0003004,
            ///<Summary>
            ///N SECONDS TO SKIP Command
            ///</ Summary>
            ///<Remarks> The message must specify a value in seconds to be added
            ///To the current position to make the jump. Take positive values
            ///To go ahead and negatives to go back. </ Remarks>
            JUMPOFNSECONDS = 0xA0003005,
            ///<Summary>
            ///Operation Restore AUDIO TRACKS
            ///</ Summary>
            ///<Remarks> </ remarks>
            GETAUDIOTRACKS = 0xA0003001,
            ///<Summary>
            ///Operation Restore CURRENT INFO ON STREAM
            ///</ Summary>
            ///<Remarks> CURRENTLY NOT WORKING </ remarks>
            GETNOWPLAYING = 0xA0003002,
            ///<Summary>
            ///Operation Restore LIST OF ELEMENTS IN PLAYLIST
            ///</ Summary>
            ///<Remarks> </ remarks>
            GETPLAYLIST = 0xA0003003,
            ///<Summary>
            ///Command SWITCH FULL SCREEN
            ///</ Summary>
            ///<Remarks> </ remarks>
            TOGGLEFULLSCREEN = 0xA0004000,
            ///<Summary>
            ///Command SHORT JUMP AHEAD
            ///</ Summary>
            ///<Remarks> </ remarks>
            JUMPFORWARDMED = 0xA0004001,
            ///<Summary>
            ///Command SHORT JUMP BACK
            ///</ Summary>
            ///<Remarks> </ remarks>
            JUMPBACKWARDMED = 0xA0004002,
            ///<Summary>
            ///Command INCREASE VOLUME
            ///</ Summary>
            ///<Remarks> </ remarks>
            INCREASEVOLUME = 0xA0004003,
            ///<Summary>
            ///Command DECREASE VOLUME
            ///</ Summary>
            ///<Remarks> </ remarks>
            DECREASEVOLUME = 0xA0004004,
            ///<Summary>
            ///Command SWITCH SHADER
            ///</ Summary>
            ///<Remarks> </ remarks>
            SHADER_TOGGLE = 0xA0004005,
            ///<Summary>
            ///Command CLOSE MPC
            ///</ Summary>
            ///<Remarks> </ remarks>
            CLOSEAPP = 0xA0004006,
            ///<Summary>
            ///Command SHOW MESSAGE OSD
            ///</ Summary>
            ///<Remarks> The message must specify a group of three parameters:
            ///Position of the message (refer to the list OSD_POSITION);
            ///Message time (in milliseconds);
            ///Message content in an array of CHAR (max 128 char) </ remarks>
            OSDSHOWMESSAGE = 0xA0005000
        }
        ///<Summary>
        ///List of the constants related to the command CMD_RECEIVED.CMD_STATE
        ///</ Summary>
        ///<Remarks> Specify in the message. Indicate the status of MPC </ remarks>
        public enum LoadState {
            ///<Summary>
            ///State CLOSED
            ///</ Summary>
            ///<Remarks> </ remarks>
            MLS_CLOSED = 0,
            ///<Summary>
            ///State IN LOADING
            ///</ Summary>
            ///<Remarks> </ remarks>
            MLS_LOADING = 1,
            ///<Summary>
            ///State LOADED
            ///</ Summary>
            ///<Remarks> </ remarks>
            MLS_LOADED = 2,
            ///<Summary>
            ///State CLOSING
            ///</ Summary>
            ///<Remarks> </ remarks>
            MLS_CLOSING = 3
        }
        ///<Summary>
        ///List of the constants related to the command CMD_RECEIVED.CMD_PLAYMODE
        ///</ Summary>
        ///<Remarks> Specify in the message. Indicate the status of playback </ remarks>
        public enum PlayState {
            ///<Summary>
            ///State PLAY
            ///</ Summary>
            ///<Remarks> </ remarks>
            PS_PLAY = 0,
            ///<Summary>
            ///State PAUSE
            ///</ Summary>
            ///<Remarks> </ remarks>
            PS_PAUSE = 1,
            ///<Summary>
            ///STOP state
            ///</ Summary>
            ///<Remarks> </ remarks>
            PS_STOP = 2,
            ///<Summary>
            ///NOT USED State
            ///</ Summary>
            ///<Remarks> </ remarks>
            PS_UNUSED = 3
        }
        ///<Summary>
        ///List of the constants related to the position of the OSD message for
        ///Command CMD_SEND.Cmd_OSDSHOWMESSAGE
        ///</ Summary>
        ///<Remarks> </ remarks>	 
        public enum OsdPosition {
            ///<Summary>
            ///Position NO MESSAGE
            ///</ Summary>
            ///<Remarks> </ remarks>
            OSD_NOMESSAGE = 0,
            ///<Summary>
            ///Position TOP LEFT
            ///</ Summary>
            ///<Remarks> </ remarks>
            OSD_TOPLEFT = 1,
            ///<Summary>
            ///Position TOP RIGHT
            ///</ Summary>
            ///<Remarks> </ remarks>
            OSD_TOPRIGHT = 2
        }
    }
}
