using System.Collections.Generic;
using board.engine;
using board.engine.Actions;
using board.engine.Movement;
using chess.engine.Extensions;
using chess.engine.Game;

namespace chess.engine.tests.Builders
{
    public class ChessPathDestinationsBuilder
    {
        private readonly BoardLocation _start;

        private readonly List<(BoardLocation, int)> _destinations = new List<(BoardLocation, int)>();
        public ChessPathDestinationsBuilder(BoardLocation start)
        {
            _start = start;
        }

        public ChessPathDestinationsBuilder To(BoardLocation at, int chessMoveTypes)
        {
            _destinations.Add((at, chessMoveTypes));
            return this;
        }
        public ChessPathDestinationsBuilder To(string at, int chessMoveTypes = (int) DefaultActions.MoveOnly)
        {
            return To(at.ToBoardLocation(), chessMoveTypes);
        }

        public ChessPathDestinationsBuilder ToUpdatePiece(string at, ChessPieceName promotionPiece, DefaultActions defaultActions = DefaultActions.UpdatePiece)
        {
            return To(at.ToBoardLocation(), (int) defaultActions);
        }

        public Path Build()
        {
            var path = new Path();

            foreach (var destination in _destinations)
            {
                path.Add(BoardMove.Create(_start, destination.Item1, destination.Item2));
            }

            return path;
        }
    }
}