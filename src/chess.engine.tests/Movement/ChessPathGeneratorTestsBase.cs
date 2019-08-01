using System.Collections.Generic;
using System.Linq;
using board.engine.Board;
using board.engine.Movement;
using chess.engine.Entities;
using chess.engine.Game;
using Moq;
using Shouldly;

namespace chess.engine.tests.Movement
{
    public abstract class ChessPathGeneratorTestsBase
    {
        protected Mock<IBoardState<ChessPieceEntity>> BoardStateMock;

        protected void PathsShouldContain(IEnumerable<Path> paths, Path path, Colours colour)
        {
            paths.Contains(path).ShouldBeTrue();

        }

        protected void SetUp()
        {
            BoardStateMock = new Mock<IBoardState<ChessPieceEntity>>();
        }
    }
}