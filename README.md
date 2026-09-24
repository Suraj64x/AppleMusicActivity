<div align="center">

<img src="assets/icon.png" width="120" alt="iMusicActivity">

# iMusicActivity

**Apple Music on Windows, live on Discord.**

<sub>Play a song. Your friends see it.</sub>

<br>

<img src="assets/presence.png" width="480" alt="Discord showing Listening to Apple Music">

<br>

<a href="https://github.com/Suraj64x/AppleMusicActivity/releases/download/v2.4.0/iMusicActivity.exe">
  <img src="https://img.shields.io/badge/Download-v2.4.0-FA2D48?style=for-the-badge&logo=windows&logoColor=white" alt="Download v2.4.0">
</a>
&nbsp;
<a href="https://github.com/Suraj64x/AppleMusicActivity/releases/tag/v2.4.0">
  <img src="https://img.shields.io/badge/Release-2.4.0-2B2D31?style=for-the-badge" alt="Release">
</a>

<br><br>

<img src="https://img.shields.io/badge/Windows%2011-0078D4?style=flat&logo=windows&logoColor=white" alt="Windows 11">
<img src="https://img.shields.io/badge/Apple%20Music-FA2D48?style=flat&logo=applemusic&logoColor=white" alt="Apple Music">
<img src="https://img.shields.io/badge/Discord-5865F2?style=flat&logo=discord&logoColor=white" alt="Discord">

<br><br>

<img src="assets/dots.svg" width="70" alt="">

<br><br>

<table>
  <tr>
    <td align="center" width="200">
      <img src="assets/apple-sing.svg" width="56" alt=""><br>
      <b>Presence</b><br>
      <sub>Song, artist, album, art, time</sub>
    </td>
    <td align="center" width="200">
      <img src="assets/apple-wink.svg" width="56" alt=""><br>
      <b>Scrobble</b><br>
      <sub>Last.fm and ListenBrainz</sub>
    </td>
    <td align="center" width="200">
      <img src="assets/apple-sleep.svg" width="56" alt=""><br>
      <b>Quiet</b><br>
      <sub>Lives in the tray</sub>
    </td>
  </tr>
</table>

<br>

<img src="assets/faces/sunny.svg" width="20" alt="">
&nbsp;
<sub>Optional lyrics &nbsp;·&nbsp; optional scrobbles &nbsp;·&nbsp; one tray icon</sub>
&nbsp;
<img src="assets/faces/blush.svg" width="20" alt="">

<br><br>

## <img src="assets/faces/mint.svg" width="26" align="top" alt=""> &nbsp;Install

[Download v2.4.0](https://github.com/Suraj64x/AppleMusicActivity/releases/download/v2.4.0/iMusicActivity.exe)<br>
Use [Apple Music from the Store](https://apps.microsoft.com/detail/9PFHDD62MXS1)<br>
Install [.NET 10 Desktop Runtime](https://dotnet.microsoft.com/en-us/download/dotnet/10.0) if Windows asks<br>
Run the exe, then turn on Discord **Activity Status**

<br>

<sub>Keep Apple Music and iMusicActivity on the same virtual desktop.</sub>

<br><br>

## <img src="assets/faces/fresh.svg" width="26" align="top" alt=""> &nbsp;Setup

Double-click the tray icon.

<table>
  <tr>
    <td align="center" width="280">
      <b>Last.fm</b><br>
      <sub>API key, secret, username, password</sub>
    </td>
    <td align="center" width="280">
      <b>ListenBrainz</b><br>
      <sub>User token</sub>
    </td>
  </tr>
</table>

<sub>Passwords live in Windows Credential Manager. Offline listens are not saved.</sub>

<br><br>

## <img src="assets/faces/dusk.svg" width="26" align="top" alt=""> &nbsp;Build

```bat
dotnet publish iMusicActivity/iMusicActivity.csproj -c Release -r win-x64 --self-contained false -p:PublishSingleFile=true
```

<sub>Logs live in <code>%localappdata%\iMusicActivity</code> · <a href="https://github.com/Suraj64x/AppleMusicActivity/issues">Open an issue</a></sub>

<br><br>

<img src="assets/faces/sunny.svg" width="22" alt="">
<img src="assets/faces/peach.svg" width="22" alt="">
<img src="assets/faces/mint.svg" width="22" alt="">
<img src="assets/icon.png" width="28" alt="">
<img src="assets/faces/dusk.svg" width="22" alt="">
<img src="assets/faces/blush.svg" width="22" alt="">
<img src="assets/faces/sleep.svg" width="22" alt="">

<br><br>

**Made with love by SMOKiE**

</div>
