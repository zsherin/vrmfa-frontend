# vrmfa-frontend
front end Unity App for the virtual reality museum for all

## Setup
Install the [**Unity3d**](http://unity3d.com/) game engine, and then open the Assets/MainScene.Unity file. It will open to a scene with two objects in the game. Find a Google Cardboard headset, and you will be able to run the game on your phone by building it from Unity for your specific phone (tested on Android, nothing else).

## Usage
It's as simple as pressing play! The game is currently set up for use with a Google Cardboard headset, and the local museum file. To move with the Google Cardboard headset, simply pull and release quickly on the side-mounted magnet to start and stop moving in your look direction.
If you want to build your own museums, or see how they're created, set up the [**vrmfa-backend**](https://github.com/hariharsubramanyam/vrmfa-backend) repository, and use it to generate museums. Then, point the url variable on the Museum Generator object to the url and port that you have the server running on. It will download whatever museum has been generated, and use that to run.
Good luck! Feel free to contact zarrro45@gmail.com with questions.

## Limitations
Currently, because of how much memory the images require, you can only have about five to seven rooms in a museum, or the phone app crashes. I'm sure this is fixable; it just wasn't on our priority list for completing the MVP.