using System.Linq;
using board.engine;
using board.engine.Board;
using board.engine.Movement;
using chess.engine.Entities;

namespace chess.engine.Movement
{
    public class ChessPathsValidator : IPathsValidator<ChessPieceEntity>
    {
        private readonly IPathValidator<ChessPieceEntity> _pathValidator;

        public ChessPathsValidator(IPathValidator<ChessPieceEntity> pathValidator
            )
        {
            _pathValidator = pathValidator;
        }

        public Paths GetValidatedPaths(IBoardState<ChessPieceEntity> boardState, ChessPieceEntity entity, BoardLocation boardLocation)
        {
            var paths = new Paths(
                entity.PathGenerators
                    .SelectMany(pg => pg.PathsFrom(boardLocation, entity.Owner))
            );

            var validPaths = RemoveInvalidMoves(boardState, paths);

            return validPaths;
        }

        private Paths RemoveInvalidMoves(IBoardState<ChessPieceEntity> boardState, Paths possiblePaths) =>
            new Paths(
                possiblePaths
                    .Where(possiblePath => _pathValidator.ValidatePath(boardState, possiblePath).Any())
                    .Select(pp => _pathValidator.ValidatePath(boardState, pp))
            );
    }
}