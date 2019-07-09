# CHESS.ENGINE

## Introduction

Firstly please note this is an "engine" that implements the rules of chess, it maintains the state of the board and understands how pieces move and can generate a list of valid moves for a given board state, it also understands game end conditions (namely check & checkmate, stalement detection not yet implemented). It cannot actually play chess beyond random move selection, and is designed to be the base for future efforts to (amongst other ideas) create actual players that use board state and move list for analysis purposes to generate a new move.

## Tech Stack

.NET Core Library

## Overview

The "engine" currently consists of the following components;

* The [board.engine][link.board.engine] which contains core mechanics required to manage pieces on a chess board.
* The [board.engine.tests.utils][link.board.engine.tests.utils] as a generalised place to store stuff useful for testing the board engine.
* The [board.engine.tests][link.board.engine.tests] units tests for the above.
* The [chess.engine][link.chess.engine] which contains the mechanics specific to the game of chess.
* The [chess.engine.tests][link.chess.engine.tests] units tests for the above.
* The [chess.engine.integration.tests][link.chess.engine.integration.tests] move heavyweight tests for the engine that tend use full board state or full games for testing.
* The [chess.tests.utils][link.chess.tests.utils] as a generalised place to store stuff useful for testing the chess engine across different layers (unit, integration, feature etc.).
* [Spiker][link.Spiker] a simple console application which I use to quickly test out ideas that don't really work via spike tests, a proper console chess player application will be coming shortly.

## Approach to chess

The basic design is around a two stage process, firstly each piece supplies the list of moves it could make along with predicate conditions for validating the move.

For example.

* A pawn can move one square forward, if the square in front is empty.
* A king can move one square forward, if the square in front is empty AND not under attack.

The board then executes the predicates to filter out invalid moves, leaving a list of only moves that are valid for the current board state.

## Building/Testing/Running

As a `dotnet` solution, the default `build, test` targets are available.

As this is an "engine" repository there is no specific host for it (such as a console app, or a web service), so there is nothing to `run` beyond the [Spiker app](https://github.com/Chrislee187/chess.engine/blob/master/src/spiker/Program.cs)) I use for ad-hoc testing.
 
### Getting Started

As big fan of testable code, I use interfaces to abstract various parts of the implementation to facilitate testing, this can lead to some clumsy looking constructors so I also use Dependency Injection using the  [.NET Core `Microsoft.Extensions.DependencyInjection` NUGET package](https://www.nuget.org/packages/Microsoft.Extensions.DependencyInjection/) to handle object instantiation internally.

A simple static factory class exists to resolve all default dependencies allowing a new chess game to be instansiated with;

``` c#
    var game = ChessFactory.NewChessGame();
```

moves can then be played using;

``` c#
    var result = game.Move(move)
```

Where move is a standard chess move in [SAN](https://en.wikipedia.org/wiki/Algebraic_notation_(chess)) format.

NOTE: Full co-ordinate moves such as `d2d4` or `e8g8` are valid SAN moves.

See [`chess.engine.integration.tests.SmokeTests`][link.SmokeTests] for a couple of examples.

[link.swaggerui]: https://chess-web-api.azurewebsites.net/swagger/index.html
[link.chess.webapi.index.live]: https://chess-web-api.azurewebsites.net
[link.Spiker]: /src/spiker
[link.SmokeTests]: https://github.com/Chrislee187/chess.engine/blob/master/src/chess.engine.integration.tests/SmokeTests.cs
[link.board.engine]: /src/board.engine
[link.chess.engine]: /src/chess.engine
[link.chess.webapi]: /src/chess.webapi
[link.chess.engine.tests]: /src/chess.engine.tests
[link.board.engine.tests]: /src/board.engine.tests
[link.chess.tests.utils]: /src/chess.tests.utils
[link.board.engine.tests.utils]: /src/board.engine.tests.utils
[link.chess.engine.integration.tests]: https://github.com/Chrislee187/chess.engine/src/chess.engine.integration.tests
