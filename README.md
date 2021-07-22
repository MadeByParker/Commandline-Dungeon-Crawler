# 2020-COMP1000-CW2 ☠

This is the readme for my commandline dungeom crawler game. 

# For University COMP1000 Module

Code explanation video --> [here](https://youtu.be/s-HPMGwV-Es)

# Description how to run: 🏃‍♂️

I have compiled the project so that it can be executed without any issues 😊.

### On Windows Command Prompt:

* Type cd .\bin\Release\netcore3.1 then type Crawler.exe.

### On Windows Powershell:

* Type the same then type .\Crawler.exe.

* Or go to that folder and execute the .exe file.

### Starting the game: ▶

* Load the map with either `"load Simple.map"` or `"load Advanced.map"` command.

* Start the game by typing `"play"`.

### Info on the game: ℹ

First information: You are a character and have to escape dungeons where there is gold and monsters! Play the levels to escape!

Aim: This is a Dungeon Crawler made for the Command Line/ Powershell. Move the character, represented by an '@', starting on a 'S', by using the WASD keys and make your move around the map without crashing into walls '#' and battling with Monsters! 'M' by using the SPACEBAR key and picking up pieces of gold 'G' by pressing E key.

## Controls: 🎮

| Key | Action |
| --- | ------ |
|  W  | Move Up |
|  S  | Move Down |
|  A  | Move Left |
|  D  | Move Right |
|  E  |Pickup Gold |
| "Spacebar" | Attack Monsters |

This project used a various amount of techniques:

* Reading files.
* Jagged arrays.
* Switches.
* Various C# techniques.
* C# Console Application on Visual Studio 2019.

## Resources used: 📚

* Maze Game Coding Video for ideas: [Video on Youtube](https://www.youtube.com/watch?v=T0MpWTbwseg)
* Linkedin's C# Essentials Playlist: [Linkedin Playlist on C#](https://www.linkedin.com/learning/c-sharp-essential-training-1-syntax-and-object-oriented-programming/working-with-constants-and-enumerators?u=26140778)
* W3Schools C# Section: [W3Schools C#](https://www.w3schools.com/cs/default.asp)

## Screenshots: 🖼

![play-screen](https://user-images.githubusercontent.com/72020025/109822983-67b2a480-7c2f-11eb-9ba1-d287bbe3219e.PNG)

Play screen after map loaded

![Map-select](https://user-images.githubusercontent.com/72020025/109822988-684b3b00-7c2f-11eb-9e0d-4fb1be926862.PNG)

Map Selection Screen.

![gameplay](https://user-images.githubusercontent.com/72020025/109822990-684b3b00-7c2f-11eb-8780-d249a344b947.PNG)

Gameplay

## Edits of code: 📓

* 08/01/2021 - Fixed code so it passes all tests in Powershell and Visual Studio and changed the UI to look more appealing.
* 11/01/2021 - Fixed loading bug with the advanced map under new execution phase and loading it in after typing 'play' and polished the reset of variable on the replay feature.
* 12/01/2021 - Fixed bugs that messed with tests, although gold pickup doesn't work if you're on it, only works if you're next to it and updated user interface.
* 13/01/2021 - Fixed attack action where .Trim() would remove the ' ' and then replace it with no empty space so it wouldn't register attacking the monster.
* 14/01/2021 - For now if you move over the 'G' it will automatically increment gold counter but pickup stills works if next to the gold and pressing E, also ammended comments.
