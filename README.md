<div align="center">

<img src="assets/icon.png" width="120" alt="iMusicActivity">

# iMusicActivity

Apple Music on Windows, live on Discord.

<br>

<img src="assets/hero.svg" width="640" alt="">

<br><br>

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

Play Apple Music. Discord shows it.<br>
Optional lyrics. Optional scrobbles. One tray icon.

</div>

<br>

## <img src="assets/faces/sunny.svg" width="28" align="top" alt=""> Install

<img src="assets/faces/mint.svg" width="18" align="top" alt=""> &nbsp;[Download v1.4.0](https://github.com/Suraj64x/AppleMusicActivity/releases/download/v1.4.0/iMusicActivity.exe)<br>
<img src="assets/faces/fresh.svg" width="18" align="top" alt=""> &nbsp;Use [Apple Music from the Store](https://apps.microsoft.com/detail/9PFHDD62MXS1)<br>
<img src="assets/faces/dusk.svg" width="18" align="top" alt=""> &nbsp;Install [.NET 10 Desktop Runtime](https://dotnet.microsoft.com/en-us/download/dotnet/10.0) if Windows asks<br>
<img src="assets/faces/peach.svg" width="18" align="top" alt=""> &nbsp;Run the exe, then turn on Discord **Activity Status**

> [!TIP]
> Keep Apple Music and iMusicActivity on the same virtual desktop.

<br>

## <img src="assets/faces/fresh.svg" width="28" align="top" alt=""> Setup

Double-click the tray icon.

<table>
  <tr>
    <td align="center" width="300">
      <b>Last.fm</b><br>
      <sub>API key, secret, username, password</sub>
    </td>
    <td align="center" width="300">
      <b>ListenBrainz</b><br>
      <sub>User token</sub>
    </td>
  </tr>
</table>

<img src="assets/faces/sleep.svg" width="18" align="top" alt=""> &nbsp;Passwords live in Windows Credential Manager. Offline listens are not saved.

<br>

## <img src="assets/faces/dusk.svg" width="28" align="top" alt=""> Build

```bat
dotnet publish iMusicActivity/iMusicActivity.csproj -c Release -r win-x64 --self-contained false -p:PublishSingleFile=true
```

Logs live in `%localappdata%\iMusicActivity`. Something off? [Open an issue](https://github.com/Suraj64x/AppleMusicActivity/issues).

<br>

<div align="center">

<img src="assets/faces/sunny.svg" width="22" alt="">
<img src="assets/faces/fresh.svg" width="22" alt="">
<img src="assets/faces/blush.svg" width="22" alt="">
<img src="assets/apple.svg" width="30" alt="">
<img src="assets/faces/dusk.svg" width="22" alt="">
<img src="assets/faces/peach.svg" width="22" alt="">
<img src="assets/faces/mint.svg" width="22" alt="">

<br><br>

**Made with love by SMOKiE**

<sub>[GPL-3.0](LICENSE) &nbsp;·&nbsp; includes [AMWin-RP](https://github.com/PKBeam/AMWin-RP) by [PKBeam](https://github.com/PKBeam)</sub>

</div>
