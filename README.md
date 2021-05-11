# MicMuter
A Beat Saber mod that automatically mutes a selected microphone while playing a song.

Compatible with Beat Saber 1.15.0

## Installation
* Required dependency (can be installed using Mod Assistant)
    * BeatSaberMarkupLanguage v1.5.2+
* Mod installation
    * Download the latest ZIP from the [Releases](https://github.com/TechDiem/BeatSaberMicMuter/releases) page and extract it to your Beat Saber folder. After that, you should find a `MicMuter.dll` in your `Beat Saber\Plugins` folder and a `CSCore.dll` in your `Beat Saber\Libs` folder.
    * You should see a MicMuter entry in the Mod Settings menu when installed.

## Configuration
You can configure the plugin using the  `Settings (⚙) -> Mod Settings -> MicMuter` menu.
* Enabled in singleplayer/party/campaign: Enable automatic muting in every gamemode except multiplayer (singleplayer, party, campaign)
* Enabled in multiplayer: Enable automatic muting in multiplayer (online)
* Unmute on pause: Unmute the microphone while in the pause menu (only singleplayer)
* Microphone: Select your microphone here. Make sure that every name only exists once.


## Credits 
* [CSCore](https://github.com/filoe/cscore): Library used to access the CoreAudioApi to control the microphone