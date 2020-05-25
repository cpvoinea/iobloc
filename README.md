# iobloc
## 3.2
* fork System.Console and add 'WriteArea' method to draw full multi-colored screen at once
* split into projects
* integrate demo games into same project and use new native console - see Walker and Matrix
* convert everything to core 3.1
## 3.1
* .NET Framework port - iobloc4win.csproj builds in Visual Studio while iobloc.csproj still works for Core 3.0 in VS Code
* new WinForms renderer displays panel graphics instead of table of controls
* game panels hold generic cells (int type was used until now)
* refactored Launcher to choose render type and game
## v3.0
* WinForms port
* launcher
## v2.6
* 9 games: tetris, runner, helicopter, breakout, invaders, snake, sokoban, paint, table
* simple table AI
* bonus pack of sudoku levels
* level progression, ending animations
* multi-platform
* SDK as nuget package (+demo game)
* dynamic integration of external games
* extensive documentation
