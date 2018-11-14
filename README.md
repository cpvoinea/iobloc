# iobloc
## Running the code
* [dotnet core SDK 2.1](https://www.microsoft.com/net/download/dotnet-core/2.1) is a prerequisite to run the code
* run **build.bat** and it will build for all supported platforms
* the resulting binaries are found in the **Release** folder
## Playing the games
* the opening screen is a menu with the list of games
* there is a **Level** option for selecting a master level before opening a game
* the level changes the game speed, higher level means faster speed
* to enter a game, punch the shortcut key (displayed before ':' in the menu)
* there are **9 built-in games** + dynamically loaded games configured in settings
## Changing the settings
* run the game with a command argument
```sh
iobloc settings.txt
```
* this will create the settings file which can be modified
* each line is a setting consisting of two literals: name and value; the rest of the words are ignored
* common settings to change:
  * *Height* and *Width* can be changed, sometimes in relation to *BlockWidth* (number of horizontal characters per block)
  * *PlayerColor*, *EnemyColor* and the other colors with string values of [ConsoleColor enum](https://docs.microsoft.com/en-us/dotnet/api/system.consolecolor?view=netcore-2.1)
  * *Help* text can be changed, each comma ',' indicating a new line of text
  * *FrameMultiplier* is used to calculate the game speed, higher values means slower speed
  * it is **not** recommended to change *AllowedKeys* as they are sometimes hardcoded when handled
* setting values cannot contain spaces because everything after the first space is ignored
* run the game again with the same argument to use the settings
## Using the SDK
* install [nuget package](https://www.nuget.org/packages/iobloc/)
```sh
dotnet add package iobloc
```
* implement **iobloc.BasicBoard** abstract class to create a new game
* alternatively, implement **iobloc.IBoard** interface to create everything from scratch
* check demo code from [release 2.5](https://github.com/cpvoinea/iobloc/releases/tag/v2.5) to get an idea about how to implement boards
```cs
class Program
{
    class HelloWorld : BasicBoard
    {
        public HelloWorld() : base(12, 1, "Hello World!") { }

        public override void HandleInput(string key) { }
    }

    static void Main(string[] args)
    {
        new HelloWorld().Run();
    }
}
```
## Integrating new games
* use a modified settings file as command argument
* add a new section (sections are separated by empty lines) with the following settings:
  * **AssemblyPath** is path to dll containing new game, path cannot contain spaces
  * **ClassName** is the fully formed name of the class which is implementing **IBoard** interface
  * **Name** is the display name used in the menu
  * **MenuKeys** is the shortcut key in the menu
* the game will show up in the menu and will run if dll loading is successful
## v2.6
* 9 games: tetris, runner, helicopter, breakout, invaders, snake, sokoban, paint, table
* table co-op
* bonus pack of sudoku levels
* level progression, ending animations
* multi-platform
* SDK as nuget package (+demo game)
* dynamic integration of external games
* extensive documentation
## v3.0 (planned)
* AI opponent
