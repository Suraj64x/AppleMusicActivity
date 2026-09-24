<div align="center">

<img src="assets/icon.png" width="120" alt="iMusicActivity">

# iMusicActivity

Apple Music on Windows, live on Discord.

<br>

<a href="https://github.com/Suraj64x/AppleMusicActivity/releases/download/v1.4.0/iMusicActivity.exe">
  <img src="https://img.shields.io/badge/Download-v1.4.0-FA2D48?style=for-the-badge&logo=windows&logoColor=white" alt="Download v1.4.0">
</a>
&nbsp;
<a href="https://github.com/Suraj64x/AppleMusicActivity/releases/tag/v1.4.0">
  <img src="https://img.shields.io/badge/Release-1.4.0-2B2D31?style=for-the-badge" alt="Release">
</a>

<br><br>

<img src="https://img.shields.io/badge/Windows%2011-0078D4?style=flat&logo=windows&logoColor=white" alt="Windows 11">
<img src="https://img.shields.io/badge/Apple%20Music-FA2D48?style=flat&logo=applemusic&logoColor=white" alt="Apple Music">
<img src="https://img.shields.io/badge/Discord-5865F2?style=flat&logo=discord&logoColor=white" alt="Discord">
<img src="https://img.shields.io/badge/GPL--3.0-2B2D31?style=flat" alt="GPL-3.0">

</div>

---

<div align="center">

| Presence | Scrobble | Quiet |
| :---: | :---: | :---: |
| Song, artist, album, art, time | Last.fm and ListenBrainz | Lives in the tray |

</div>

Play Apple Music. Discord shows it. Optional lyrics. Optional scrobbles. One tray icon.

---

## Install

1. [Download v1.4.0](https://github.com/Suraj64x/AppleMusicActivity/releases/download/v1.4.0/iMusicActivity.exe)
2. Use [Apple Music from the Store](https://apps.microsoft.com/detail/9PFHDD62MXS1)
3. Install [.NET 10 Desktop Runtime](https://dotnet.microsoft.com/en-us/download/dotnet/10.0) if Windows asks
4. Run the exe · enable Discord **Activity Status**

> Keep Apple Music and iMusicActivity on the same virtual desktop.

---

## Setup

Double-click the tray icon.

| Last.fm | ListenBrainz |
| :---: | :---: |
| API key, secret, username, password | User token |

Passwords go in Windows Credential Manager. Offline listens are not saved.

---

## Build

```bat
dotnet publish iMusicActivity/iMusicActivity.csproj -c Release -r win-x64 --self-contained false -p:PublishSingleFile=true
```

Logs: `%localappdata%\iMusicActivity` · [Issues](https://github.com/Suraj64x/AppleMusicActivity/issues)

---

<div align="center">

**Made with love by SMOKiE**

[GPL-3.0](LICENSE) · includes [AMWin-RP](https://github.com/PKBeam/AMWin-RP) by [PKBeam](https://github.com/PKBeam)

</div>
