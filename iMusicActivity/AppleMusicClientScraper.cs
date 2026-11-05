using FlaUI.Core.AutomationElements;
using FlaUI.UIA3;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Timers;

namespace iMusicActivity {

    internal class AppleMusicInfo: IEquatable<AppleMusicInfo> {
        // the following fields are only valid if HasSong is true.
        // DateTimes are in UTC.
        public string SongName;
        public string SongSubTitle;
        public string SongAlbum;
        public string SongArtist;
        public bool IsPaused = true;
        public DateTime? PlaybackStart;
        public DateTime? PlaybackEnd;
        public int? SongDuration = null;
        public List<string>? ArtistList = null;
        public string? CoverArtUrl = null;
        public string? SongUrl = null;
        public string? ArtistUrl = null;
        public int? CurrentTime = null;
        public List<LyricLine>? SyncedLyrics = null;
        public bool LyricsSearched = false;

        public AppleMusicInfo(string songName, string songSubTitle, string songAlbum, string songArtist) {
            this.SongName = Sanitize(songName);
            this.SongSubTitle = Sanitize(songSubTitle);
            this.SongAlbum = Sanitize(songAlbum);
            this.SongArtist = Sanitize(songArtist);
        }

        private static string Sanitize(string s) {
            if (string.IsNullOrEmpty(s)) return s;
            // Remove common invisible/formatting characters used by Apple Music
            return Regex.Replace(s, @"[\u200B-\u200F\u202A-\u202E]", "").Trim();
        }

        public override string ToString() {
            var str = $"[AppleMusicInfo] {SongName} by {SongArtist} on {SongAlbum}";
            if (SongDuration != null) {
                str += $"\n| Duration:  {SongDuration} sec";
            }
            if (CoverArtUrl != null) {
                str += $"\n| Album art: {CoverArtUrl}";
            }
            return str;
        }
        public void Print() {
            Trace.WriteLine(ToString());
        }

        public bool Equals(AppleMusicInfo? other) {
            return other is not null && other!.SongName == SongName && other!.SongArtist == SongArtist && other!.SongSubTitle == SongSubTitle;
        }
        public override bool Equals(object? obj) => Equals(obj as AppleMusicInfo);
        public static bool operator == (AppleMusicInfo? a1, AppleMusicInfo? a2) {
            if (a1 is null && a2 is null) {
                return true;
            } else if (a1 is null || a2 is null) {
                return false;
            } else {
                return a1.Equals(a2);
            }
        }
        public static bool operator != (AppleMusicInfo? a1, AppleMusicInfo? a2) {
            return !(a1 == a2);
        }

        public override int GetHashCode() {
            return SongName.GetHashCode() ^ SongArtist.GetHashCode() ^ SongSubTitle.GetHashCode();
        }
    }

    internal class AppleMusicClientScraper {
        struct WebReqFailCounters {
            public int MaxFails = Constants.NumFailedSearchesBeforeAbandon;

            public int SongDuration = 0;
            public int AlbumArt = 0;
            public int ArtistList = 0;
            public int SongUrl = 0;
            public int ArtistUrl = 0;

            public WebReqFailCounters() { }
        };

        private static readonly Regex ComposerPerformerRegex = new Regex(@"By\s.*?\s\u2014", RegexOptions.Compiled);

        public delegate void RefreshHandler(AppleMusicInfo? newInfo);
        string? lastFmApiKey;
        Timer timer;
        RefreshHandler refreshHandler;
        AppleMusicInfo? currentSong;
        public bool composerAsArtist; // for classical music, treat composer (not performer) as artist
        Logger? logger;
        double? previousSongProgress;
        string appleMusicRegion;
        WebReqFailCounters webReqFails = new();
        LRCLibClient lrclibClient;
        UIA3Automation? automation;
        int refreshing = 0;
        Task? webScrapeTask;
        AppleMusicInfo? webScrapeSong;

        public AppleMusicClientScraper(string? lastFmApiKey, int refreshPeriodInSec, bool composerAsArtist, string appleMusicRegion, RefreshHandler refreshHandler, Logger? logger = null) {
            this.refreshHandler = refreshHandler;
            this.logger = logger;
            this.lastFmApiKey = lastFmApiKey;
            this.composerAsArtist = composerAsArtist;
            this.appleMusicRegion = appleMusicRegion;
            this.lrclibClient = new LRCLibClient(logger);

            timer = new Timer(refreshPeriodInSec * 1000);
            timer.Elapsed += Refresh;
            Refresh(this, null);
            timer.Start();
        }

        ~AppleMusicClientScraper() {
            timer.Elapsed -= Refresh;
        }

        public void ChangeRegion(string region) {
            this.appleMusicRegion = region;
            Refresh(this, null);
        }

        public void Refresh(object? source, ElapsedEventArgs? e) {
            // skip this tick if the last refresh is still running, so slow lookups can't pile up
            if (System.Threading.Interlocked.Exchange(ref refreshing, 1) == 1) {
                return;
            }

            // always run off the UI thread, so UI Automation never blocks the app
            Task.Run(async () => {
                try {
                    await GetAppleMusicInfo();
                } catch (Exception ex) {
                    logger?.Log($"Something went wrong while scraping: {ex}");
                }
                try {
                    refreshHandler(currentSong);
                } catch (Exception ex) {
                    logger?.Log($"Something went wrong while updating: {ex}");
                } finally {
                    System.Threading.Interlocked.Exchange(ref refreshing, 0);
                }
            });
        }

        public async Task GetAppleMusicInfo() {
            var isMiniPlayer = true;
            var amProcesses = Process.GetProcessesByName("AppleMusic");
            var processId = amProcesses.FirstOrDefault()?.Id;
            foreach (var p in amProcesses) {
                p.Dispose();
            }
            if (processId == null) {
                logger?.Log("Could not find an AppleMusic.exe process");
                currentSong = null;
                return;
            }

            // reuse one automation instance; making a new one every refresh is slow.
            // the timeout stops a frozen Apple Music window from stalling us for long.
            automation ??= new UIA3Automation { TransactionTimeout = TimeSpan.FromSeconds(3) };

            // find apple music windows
            var windows = automation.GetDesktop().FindAllChildren(c => c.ByProcessId(processId.Value)).ToList();

            // if no windows on the normal desktop, search for virtual desktops and add them
            if (windows.Count == 0) {
                logger?.Log("No windows found on desktop, trying alternative search");
                using var app = FlaUI.Core.Application.Attach(processId.Value);
                var vdesktopWin = app.GetMainWindow(automation, TimeSpan.FromSeconds(3));
                if (vdesktopWin != null) {
                    windows.Add(vdesktopWin);
                }
            }

            // find an apple music window that we can extract information from
            AutomationElement? amSongPanel = null;
            foreach (var window in windows) {
                // TODO: can localisation change the window name of the Mini Player?
                isMiniPlayer = window.Name == "Mini Player";

                if (isMiniPlayer) {
                    amSongPanel = window.FindFirstDescendant(cf => cf.ByClassName("InputSiteWindowClass"));

                    // preference the mini player because it always has timestamps visible
                    if (amSongPanel != null) {
                        break;
                    }
                } else {
                    amSongPanel = window.FindFirstDescendant(cf => cf.ByAutomationId("TransportBar")) ?? amSongPanel;
                }
            }

            if (isMiniPlayer) {
                logger?.Log("Using Mini Player");
            }

            if (amSongPanel == null) {
                logger?.Log("Apple Music song panel is not initialised or missing");
                currentSong = null;
                return;
            }

            // ================================================
            //  Get song fields
            // ------------------------------------------------

            var songFieldsPanel = isMiniPlayer ? amSongPanel : amSongPanel.FindFirstChild("LCD");
            var songFields = songFieldsPanel?.FindAllChildren(cf => cf.ByAutomationId("myScrollViewer")) ?? [];

            // ================================================
            //  Check if there is a song playing
            // ------------------------------------------------

            // an active mini player must have a song 
            if (!isMiniPlayer && songFields.Length != 2) {
                currentSong = null;
                return;
            }

            // ================================================
            //  Get song, artist and album names
            // ------------------------------------------------

            var songNameElement = songFields[0];
            var songAlbumArtistElement = songFields[1];


            // the upper rectangle is the song name; the bottom rectangle is the author/album
            // lower .Bottom = higher up on the screen (?)
            if (songNameElement.BoundingRectangle.Bottom > songAlbumArtistElement.BoundingRectangle.Bottom) {
                songNameElement = songFields[1];
                songAlbumArtistElement = songFields[0];
            }

            var songName = songNameElement.Name;
            var songAlbumArtist = songAlbumArtistElement.Name;

            // the mini player duplicates the album/artist string by a power of two (when hovered over) to mimic an infinite scroll effect
            if (isMiniPlayer) {
                songName = DeduplicatedString(songName) ?? songName;
                songAlbumArtist = DeduplicatedString(songAlbumArtist) ?? songAlbumArtist;
            }

            string songArtist = "";
            string songAlbum = "";
            string? songPerformer = null;

            // parse song string into album and artist
            try {
                var songInfo = ParseSongAlbumArtist(songAlbumArtist, composerAsArtist);
                songArtist = songInfo.Item1;
                songAlbum = songInfo.Item2;
                songPerformer = songInfo.Item3;
            } catch (Exception ex) {
                logger?.Log($"Could not parse '{songAlbumArtist}' into artist and album: {ex}");
            }

            // ================================================
            //  Initialise basic song data and web scraper
            // ------------------------------------------------

            var newSong = new AppleMusicInfo(songName, songAlbumArtist, songAlbum, songArtist);

            // only clear out the current song if song is new
            if (currentSong != newSong) {
                // keep the same album art if it's another song in the same album
                if (newSong.SongAlbum == currentSong?.SongAlbum && newSong.SongArtist == currentSong?.SongArtist) {
                    newSong.CoverArtUrl = currentSong.CoverArtUrl;
                }
                currentSong = newSong;
                webReqFails = new();
                previousSongProgress = null;
            }

            // when searching for song info, use the performer as the artist instead of composer
            var webScraper = new AppleMusicWebScraper(songName, songAlbum, songPerformer ?? songArtist, appleMusicRegion, logger, lastFmApiKey);

            // ================================================
            //  Get song playback status
            // ------------------------------------------------

            // check if the song is paused or not
            var playPauseButton = amSongPanel.FindFirstChild("TransportControl_PlayPauseStop");
            var songProgressSlider = (isMiniPlayer ? amSongPanel.FindFirstChild("Scrubber") : amSongPanel.FindFirstChild("LCD")?.FindFirstChild("LCDScrubber"))?.Patterns.RangeValue.Pattern;
            var songProgressPercent = songProgressSlider == null ? 0 : songProgressSlider.Value / songProgressSlider.Maximum;
            var readAt = DateTime.UtcNow;

            // grab playback status directly from Apple Music for English languages
            if (playPauseButton?.Name == "Play" || playPauseButton?.Name == "Pause") {
                currentSong.IsPaused = playPauseButton.Name == "Play";

            } else { // ... otherwise fallback to tracking song progress
                currentSong.IsPaused = previousSongProgress != null && songProgressPercent == previousSongProgress;
                previousSongProgress = songProgressPercent;
            }

            // ================================================
            //  Get song timestamps
            // ------------------------------------------------

            int? currentTime = null;
            int? remainingDuration = null;
                
            var currentTimeElement = songFieldsPanel?.FindFirstChild(cf => cf.ByAutomationId("CurrentTime"));
            var remainingDurationElement = songFieldsPanel?.FindFirstChild(cf => cf.ByAutomationId("Duration"));

            // the Apple Music timestamps, if visible
            if (currentTimeElement != null && remainingDurationElement != null) {
                currentTime = ParseTimeString(currentTimeElement!.Name);
                remainingDuration = ParseTimeString(remainingDurationElement!.Name);
            }

            // potentially slow HTTP requests: don't start more while the last ones for this song are still running
            if (webScrapeTask == null || webScrapeTask.IsCompleted || !ReferenceEquals(webScrapeSong, currentSong)) {
                webScrapeSong = currentSong;
                webScrapeTask = DoWebScrapes(currentSong, webScraper);
            }
            // wait up to 2s for the req to finish
            await Task.WhenAny(webScrapeTask, Task.Delay(2000));

            // once we know the song length, work the timestamps out from the scrubber
            if (currentSong.SongDuration != null) {
                double duration = (int)currentSong.SongDuration;
                currentSong.CurrentTime = (int)(songProgressPercent * duration);
                SetPlaybackTimes(currentSong, readAt, songProgressPercent * duration, (1 - songProgressPercent) * duration);
            } else if (currentTime != null && remainingDuration != null) {
                SetPlaybackTimes(currentSong, readAt, (int)currentTime, (int)remainingDuration);
            }
        }

        // Keep the previous timestamps unless they're more than 2s off (a seek, a pause or a new song).
        // Recomputing them every refresh makes them jitter by a second, so Discord gets a "new" presence each time.
        private static void SetPlaybackTimes(AppleMusicInfo song, DateTime readAt, double currentTime, double remaining) {
            var start = readAt.AddSeconds(-currentTime);
            if (song.PlaybackStart == null || Math.Abs((start - song.PlaybackStart.Value).TotalSeconds) > 2) {
                song.PlaybackStart = start;
            }
            song.PlaybackEnd = song.PlaybackStart.Value.AddSeconds(currentTime + remaining);
        }

        // results are written to the song they were requested for, even if a new song has started since
        private async Task DoWebScrapes(AppleMusicInfo song, AppleMusicWebScraper webScraper) {
            // web query for song duration if we don't have it
            if (song.SongDuration == null && webReqFails.SongDuration < webReqFails.MaxFails) {
                var result = await webScraper.GetSongDuration();
                if (result == null) {
                    webReqFails.SongDuration++;
                    if (webReqFails.SongDuration == webReqFails.MaxFails) {
                        logger?.Log("Reached max fails for GetSongDuration.");
                    }
                } else {
                    webReqFails.SongDuration = 0;
                }
                song.SongDuration = ParseTimeString(result);
            }

            // ================================================
            //  Get song cover art  
            // ------------------------------------------------

            if (song.CoverArtUrl == null && webReqFails.AlbumArt<webReqFails.MaxFails) {
                var result = await webScraper.GetAlbumArtUrl();
                if (result == null) {
                    webReqFails.AlbumArt++;
                    if (webReqFails.AlbumArt == webReqFails.MaxFails) {
                        logger?.Log("Reached max fails for GetAlbumArt.");
                    }
                } else {
                    webReqFails.AlbumArt = 0;
                }
                song.CoverArtUrl = result;
            }

            // ================================================
            //  Get song artists, as a list
            // ------------------------------------------------

            if (song.ArtistList == null && webReqFails.ArtistList < webReqFails.MaxFails) {
                var result = await webScraper.GetArtistList();
                if (result.Count == 0) {
                    webReqFails.ArtistList++;
                    if (webReqFails.ArtistList == webReqFails.MaxFails) {
                        logger?.Log("Reached max fails for GetArtistList.");
                    }
                } else {
                    webReqFails.ArtistList = 0;
                }
                song.ArtistList = result;
                if (song.ArtistList.Count == 0) {
                    song.ArtistList = null;
                }

            }

            // ================================================
            // Get music url
            // ------------------------------------------------

            if (song.SongUrl == null && webReqFails.SongUrl < webReqFails.MaxFails) {
                var result = await webScraper.GetSongUrl();
                if (result == null) {
                    webReqFails.SongUrl++;
                    if (webReqFails.SongUrl == webReqFails.MaxFails) {
                        logger?.Log("Reached max fails for GetSongUrl.");
                    }
                } else {
                    webReqFails.SongUrl = 0;
                }
                song.SongUrl = result;
            }

            // ================================================
            // Get artist url
            // ------------------------------------------------

            if (song.ArtistUrl == null && webReqFails.ArtistUrl < webReqFails.MaxFails) {
                var result = await webScraper.GetArtistUrl();
                if (result == null) {
                    webReqFails.ArtistUrl++;
                    if (webReqFails.ArtistUrl == webReqFails.MaxFails) {
                        logger?.Log("Reached max fails for GetArtistUrl.");
                    }
                } else {
                    webReqFails.ArtistUrl = 0;
                }
                song.ArtistUrl = result;
            }

            // ================================================
            // Get lyrics
            // ------------------------------------------------

            if (!song.LyricsSearched && iMusicActivity.Properties.Settings.Default.EnableSyncLyrics) {
                song.LyricsSearched = true;
                var result = await lrclibClient.GetSyncedLyrics(song.SongName, song.SongArtist, song.SongDuration);
                if (result != null) {
                    song.SyncedLyrics = result.Lyrics;
                }
            }
        }

        // e.g. parse "-1:30" to 90 seconds
        private static int? ParseTimeString(string? time) {

            if (string.IsNullOrWhiteSpace(time)) {
                return null;
            }

            // A valid time string should not be the song name (case-insensitive check for common time-like titles)
            // Most durations are less than an hour, and Apple Music format is usually M:SS or -M:SS
            if (!Regex.IsMatch(time, @"^-?\d{1,3}:\d{2}$")) {
                return null;
            }

            // remove leading "-"
            string cleanTime = time;
            if (cleanTime.Contains('-')) {
                cleanTime = cleanTime.Split('-')[1];
            }

            var parts = cleanTime.Split(':');
            if (parts.Length < 2) return null;

            if (int.TryParse(parts[0], out int min) && int.TryParse(parts[1], out int sec)) {
                return min * 60 + sec;
            }

            return null;
        }

        private static Tuple<string, string, string?> ParseSongAlbumArtist(string songAlbumArtist, bool composerAsArtist) {
            string songArtist;
            string songAlbum;
            string? songPerformer = null;

            // some classical songs add "By " before the composer's name
            var songComposerPerformer = ComposerPerformerRegex.Matches(songAlbumArtist);
            if (songComposerPerformer.Count > 0) {
                var songComposer = songAlbumArtist.Split(" \u2014 ")[0].Remove(0, 3);
                songPerformer = songAlbumArtist.Split(" \u2014 ")[1];
                songArtist = composerAsArtist ? songComposer : songPerformer;
                songAlbum = songAlbumArtist.Split(" \u2014 ")[2];
            } else {
                // U+2014 is the emdash used by the Apple Music app, not the standard "-" character on the keyboard!
                var songSplit = songAlbumArtist.Split(" \u2014 ");
                if (songSplit.Length > 1) {
                    songArtist = songSplit[0];
                    songAlbum = songSplit[1];
                } else { // no emdash, probably custom music
                    // TODO find a better way to handle this?
                    songArtist = songSplit[0];
                    songAlbum = songSplit[0];
                }
            }
            return new(songArtist, songAlbum, songPerformer);
        }

        // if the string is duplicated, halve it recursively
        private static string? DeduplicatedString(string s) {
            string firstHalf = s.Substring(0, (s.Length + 1) / 2 - 1);
            string secondHalf = s.Substring((s.Length + 1) / 2);
            if (firstHalf == secondHalf) {
                return DeduplicatedString(firstHalf) ?? firstHalf;
            }
            return null;
        }
    }
}
