using System;
using System.Diagnostics;
using System.IO;
using System.Net.Http;

namespace iMusicActivity {
    public static class Constants {
        public static string ProgramVersionBase {
            get {
                try {
                    var exePath = Process.GetCurrentProcess().MainModule?.FileName;
                    if (exePath == null) {
                        return "";
                    }
                    FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(exePath);
                    return $"v{fvi.FileVersion}";
                } catch (Exception ex) {
                    new Logger().Log($"Error getting version string: {ex}");
                    return "";
                }
            }
        } 
#if RELEASE
        public static string  ProgramVersion = ProgramVersionBase;
#else
        public static string  ProgramVersion = $"{ProgramVersionBase}-dev";
#endif                        
        public static int    MaxLogFiles                    = 10; // files
        public static int    RefreshPeriod                  = 5; // seconds
        public static int    NumFailedSearchesBeforeAbandon = 5; // attempts
        public static string ApplicationStylisedName        = "iMusicActivity";
        public static string DiscordClientID                = "1066220978406953012";
        public static string DiscordAppleMusicImageKey      = "applemusic1024x";
        public static string DiscordAppleMusicPlayImageKey  = "applemusicplay1024x";
        public static string DiscordAppleMusicPauseImageKey = "applemusicpause1024x";
        public static string LastFMCredentialTargetName     = "Last FM Password";
        public static int    LastFMTimeBeforeScrobbling     = 20; // seconds
        public static string GithubReleasesApiUrl           = "https://api.github.com/repos/Suraj64x/AppleMusicActivity/releases/latest";
        public static string GithubReleasesUrl              = "https://github.com/Suraj64x/AppleMusicActivity/releases";
        public static string GithubRepoUrl                  = "https://github.com/Suraj64x/AppleMusicActivity/";
        public static string DefaultAppleMusicRegion        = "US";
        public static string WindowsStartupFolder => Environment.GetFolderPath(Environment.SpecialFolder.Startup);
        public static string WindowsAppDataFolder => Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        public static string AppDataFolder => Path.Combine(WindowsAppDataFolder, ApplicationStylisedName);
        public static string AppShortcutPath => Path.Join(WindowsStartupFolder, "iMusicActivity.lnk");
        public static string? ExePath => Process.GetCurrentProcess().MainModule?.FileName;

        public static readonly HttpClient HttpClient = new();

        public static string[] ValidAppleMusicRegions = [
            "ae","ag","ai","am","ao","ar","at","au",
            "az","ba","bb","be","bg","bh","bj","bm",
            "bo","br","bs","bt","bw","by","bz","ca",
            "cd","cg","ch","ci","cl","cm","cn","co",
            "cr","cv","cy","cz","de","dk","dm","do",
            "dz","ec","ee","eg","es","fi","fj","fm",
            "fr","ga","gb","gd","ge","gh","gm","gr",
            "gt","gw","gy","hk","hn","hr","hu","id",
            "ie","il","in","iq","is","it","jm","jo",
            "jp","ke","kg","kh","kn","kr","kw","ky",
            "kz","la","lb","lc","lk","lr","lt","lu",
            "lv","ly","ma","md","me","mg","mk","ml",
            "mm","mn","mo","mr","ms","mt","mu","mv",
            "mw","mx","my","mz","na","ne","ng","ni",
            "nl","no","np","nz","om","pa","pe","pg",
            "ph","pl","pt","py","qa","ro","rs","ru",
            "rw","sa","sb","sc","se","sg","si","sk",
            "sl","sn","sr","sv","sz","tc","td","th",
            "tj","tm","tn","to","tr","tt","tw","tz",
            "ua","ug","us","uy","uz","vc","ve","vg",
            "vn","vu","xk","ye","za","zm","zw"
        ];
    }
}