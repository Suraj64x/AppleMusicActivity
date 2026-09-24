<div align="center">

<img src="assets/icon.png" width="140" alt="iMusicActivity">

# iMusicActivity

### Apple Music → Discord, beautifully.

Bring your Apple Music listening activity to Discord with artwork, timestamps, playback state and optional lyrics — while staying quietly in the system tray.

<br>

<a href="https://github.com/Suraj64x/AppleMusicActivity/raw/master/iMusicActivity.exe">
  <img src="https://img.shields.io/badge/Download%20for%20Windows-FA2D48?style=for-the-badge&logo=windows&logoColor=white" alt="Download">
</a>
&nbsp;
<a href="LICENSE">
  <img src="https://img.shields.io/badge/License-GPL--3.0-111111?style=for-the-badge" alt="GPL-3.0">
</a>
&nbsp;
<a href="https://dotnet.microsoft.com/download/dotnet/10.0">
  <img src="https://img.shields.io/badge/.NET%2010-512BD4?style=for-the-badge&logo=dotnet&logoColor=white" alt=".NET 10">
</a>

<br><br>

<img src="https://img.shields.io/badge/Windows%2011%2024H2%2B-0078D4?style=flat-square&logo=windows&logoColor=white" alt="Windows 11">
<img src="https://img.shields.io/badge/Apple%20Music-FA2D48?style=flat-square&logo=applemusic&logoColor=white" alt="Apple Music">
<img src="https://img.shields.io/badge/Discord-5865F2?style=flat-square&logo=discord&logoColor=white" alt="Discord">

<br><br>

<a href="https://github.com/Suraj64x/AppleMusicActivity/issues">Issues</a>
&nbsp;·&nbsp;
<a href="https://apps.microsoft.com/detail/9PFHDD62MXS1">Apple Music</a>
&nbsp;·&nbsp;
<a href="https://github.com/Suraj64x/AppleMusicActivity/raw/master/iMusicActivity.exe">iMusicActivity.exe</a>

</div>

---

<div align="center">

## ♪ Your music, finally visible.

**Play a song in Apple Music. Discord keeps up.**

</div>

---

## ✦ What is iMusicActivity?

The Apple Music app on Windows doesn't natively publish your listening activity to Discord.

**iMusicActivity is the missing link.**

It runs quietly in the background, reads Apple Music's current playback state through Windows UI Automation, and turns it into a full Discord Rich Presence.

The same process can also send your listening history to **Last.fm** and **ListenBrainz**.

```text
                 ┌─────────────────┐
                 │   Apple Music   │
                 └────────┬────────┘
                          │
                    UI Automation
                          │
                          ▼
                 ┌─────────────────┐
                 │ iMusicActivity  │
                 └───────┬─────────┘
                         │
              ┌──────────┼──────────┐
              ▼          ▼          ▼
           Discord     Last.fm   ListenBrainz
```

One app.  
One tray icon.  
No unnecessary windows.

---

## ◉ Features

<table>
<tr>
<td width="50%" valign="top">

### 🎧 Discord Rich Presence

Show:

- Song
- Artist
- Album
- Artwork
- Elapsed time
- Playback state

Keep your activity visible while playing, or optionally while paused.

</td>

<td width="50%" valign="top">

### ♫ Experimental Lyrics

Display synced lyrics through your Discord activity.

Lyrics can be cached, cleared, or disabled from settings.

</td>
</tr>

<tr>
<td width="50%" valign="top">

### ◌ Scrobbling

Send your listening history to:

**Last.fm**  
**ListenBrainz**

Last.fm passwords are stored using **Windows Credential Manager**.

</td>

<td width="50%" valign="top">

### ◈ Quiet by Design

iMusicActivity lives in the Windows system tray.

Start it with Windows if you want.

Right-click the tray icon when you need it.  
Otherwise, it stays out of your way.

</td>
</tr>

<tr>
<td width="50%" valign="top">

### 文 Multiple Languages

Available in:

`English` · `Deutsch` · `Türkçe`  
`한국어` · `日本語` · `Русский` · `Español`

</td>

<td width="50%" valign="top">

### ♮ Classical Mode

For classical music, use the **composer as the displayed artist** when the performer isn't the main focus.

</td>
</tr>
</table>

---

## ⚡ Install

### Requirements

```text
Windows 11 24H2+
Apple Music — Microsoft Store version
.NET 10 Desktop Runtime
```

### 1 · Download

<a href="https://github.com/Suraj64x/AppleMusicActivity/raw/master/iMusicActivity.exe">
<b>→ Download iMusicActivity.exe</b>
</a>

### 2 · Install Apple Music

<a href="https://apps.microsoft.com/detail/9PFHDD62MXS1">
<b>→ Get Apple Music from the Microsoft Store</b>
</a>

### 3 · Install .NET

If Windows asks for it, install the:

<a href="https://dotnet.microsoft.com/download/dotnet/10.0">
<b>.NET 10 Desktop Runtime</b>
</a>

### 4 · Launch

Run:

```text
iMusicActivity.exe
```

Look for iMusicActivity in the **Windows system tray**.

### 5 · Enable Discord Activity

In Discord:

```text
Settings
→ Activity Settings
→ Activity Privacy
→ Activity Status
→ ON
```

That's it.

> **Virtual desktops:** Apple Music and iMusicActivity need to be on the **same virtual desktop**. This is a Windows platform limitation.

---

## ♫ Scrobbling

Connect iMusicActivity to your preferred music-tracking service.

| Service | Required |
|---|---|
| **Last.fm** | API key · API secret · Username · Password |
| **ListenBrainz** | User token |

Last.fm passwords are stored in **Windows Credential Manager**, not a plaintext file.

<sub>Offline listens are discarded and are not submitted later.</sub>

---

## ⚙ Settings

Everything important stays behind the tray icon.

```text
iMusicActivity
│
├── Discord Presence
├── Lyrics
├── Last.fm
├── ListenBrainz
├── Classical Mode
├── Language
├── Start with Windows
└── Exit
```

Configure it once.

Then let it disappear.

---

## 🛠 Build from Source

Clone the repository:

```bash
git clone https://github.com/Suraj64x/AppleMusicActivity.git
cd AppleMusicActivity
```

Publish the application:

```bat
dotnet publish iMusicActivity/iMusicActivity.csproj ^
  -c Release ^
  -r win-x64 ^
  --self-contained false ^
  -p:PublishSingleFile=true
```

The finished executable will be placed in the repository root.

### Development

Open:

```text
iMusicActivity.sln
```

with Visual Studio.

Need the SDK?

<a href="https://dotnet.microsoft.com/download/dotnet/10.0">
<b>.NET 10</b>
</a>

---

## 🧩 Troubleshooting

### Discord isn't showing my music

Check:

```text
✓ Discord Activity Status is enabled
✓ Apple Music is the Microsoft Store version
✓ iMusicActivity is running
✓ Both apps are on the same virtual desktop
```

### Need logs?

Logs are stored here:

```text
%localappdata%\iMusicActivity
```

Attach the relevant logs when opening an issue.

<a href="https://github.com/Suraj64x/AppleMusicActivity/issues">
<b>→ Open an issue</b>
</a>

---

## ♥ Credits

<div align="center">

Made with love by **SMOKiE**

GPL-3.0

Includes work from  
<a href="https://github.com/PKBeam/AMWin-RP">AMWin-RP</a> by
<a href="https://github.com/PKBeam">PKBeam</a>.

<br><br>

### Press play. Let Discord keep up.

<sub>iMusicActivity · Apple Music for Windows · Discord Rich Presence</sub>

</div>
