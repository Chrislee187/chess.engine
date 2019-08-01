using System;
using System.Collections.Generic;
using board.engine;
using board.engine.Actions;
using chess.engine.Entities;
using chess.engine.Game;
using Moq;

namespace chess.engine.tests.Builders
{
    public static class ChessTestFactory
    {
        public static Mock<IBoardActionProvider<ChessPieceEntity>> BoardActionProviderMock()
            => new Mock<IBoardActionProvider<ChessPieceEntity>>();
        public static Mock<IPlayerStateService> ChessGameStateServiceMock()
            => new Mock<IPlayerStateService>();

        public static Mock<IBoardMoveService<ChessPieceEntity>> BoardMoveServiceMock()
            => new Mock<IBoardMoveService<ChessPieceEntity>>();
    }
}