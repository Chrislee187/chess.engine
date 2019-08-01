using System;
using System.Linq;
using board.engine.Board;
using board.engine.Movement;
using chess.engine.Entities;
using chess.engine.Extensions;

namespace chess.engine.Movement
{
    public class ChessPathValidator : IPathValidator<ChessPieceEntity>
    {
        private readonly IMoveValidationProvider<ChessPieceEntity> _validationProvider;

        public ChessPathValidator(IMoveValidationProvider<ChessPieceEntity> validationProvider
            )
        {
            _validationProvider = validationProvider;
        }

        public Path ValidatePath(IReadOnlyBoardState<ChessPieceEntity> boardState, Path possiblePath)
        {
            var validPath = new Path();
            // TODO: Not happy with algorithm, those breaks :(
            foreach(var move in possiblePath)
            {
                if (!_validationProvider.TryGetValue(move.MoveType, out var moveTests))
                {
                    Throw.UnsupportedMoveType(move.MoveType);
                }

                if (!moveTests.All(t => t(move, boardState)))
                {
                    break;
                }

                validPath.Add(move);

                if (PathIsBlocked(move, boardState)) break;
            }

            return validPath;
        }

        private static bool PathIsBlocked(BoardMove move, IReadOnlyBoardState<ChessPieceEntity> boardState)
        {
            if (boardState.IsEmpty(move.To)) return false;

            var movePlayerColour = boardState.GetItem(move.From)?.Item.Player;
            var takeEntity = boardState.GetItem(move.To)?.Item;
            var moveIsATake = takeEntity != null && takeEntity.Player != movePlayerColour;
            return moveIsATake;
        }
    }
}