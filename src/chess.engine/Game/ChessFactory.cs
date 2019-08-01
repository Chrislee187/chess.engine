using System;
using board.engine;
using board.engine.Movement;
using chess.engine.Actions;
using chess.engine.Entities;
using chess.engine.Extensions;
using chess.engine.Movement;
using chess.engine.Movement.King;
using chess.engine.SAN;

namespace chess.engine.Game
{
    public static class ChessFactory
    {
        public static ChessPieceEntityFactory ChessPieceEntityFactory()
            => new ChessPieceEntityFactory();

        public static IChessValidationSteps CastleValidationSteps()
            => new ChessValidationSteps();
        public static ChessMoveValidationProvider MoveValidationProvider()
            => new ChessMoveValidationProvider(CastleValidationSteps());

        public static ChessPathValidator PathValidator(
            IMoveValidationProvider<ChessPieceEntity> moveValidationProvider = null)
            => new ChessPathValidator(moveValidationProvider ?? MoveValidationProvider()
            );

        public static ChessPathsValidator PathsValidator(IPathValidator<ChessPieceEntity> pathValidator = null
        )
        {
            return new ChessPathsValidator(pathValidator ?? PathValidator(null)
            );
        }
        public static ChessRefreshAllPaths ChessRefreshAllPaths(ChessBoardActionProvider chessBoardActionProvider = null
            )
            => new ChessRefreshAllPaths(
                CheckDetectionService()
            );

        public static IPlayerStateService PlayerStateService() 
        => new PlayerStateService(FindAttackPaths(), PathsValidator());

        public static IFindAttackPaths FindAttackPaths()
            => new FindAttackPaths();
        public static ChessBoardActionProvider ChessBoardActionProvider(IBoardEntityFactory<ChessPieceEntity> entityFactory = null)
            => new ChessBoardActionProvider(
                entityFactory ?? ChessPieceEntityFactory()
                );

        public static ChessGame NewChessGame()
            => new ChessGame(
                ChessBoardEngineProvider(),
                ChessPieceEntityFactory(),
                CheckDetectionService()
                );

        public static ChessGame CustomChessGame(IBoardSetup<ChessPieceEntity> setup, Colours toPlay = Colours.White) 
            => new ChessGame(
                ChessBoardEngineProvider(),
                CheckDetectionService(),
                setup,
                toPlay
                );

        public static IBoardMoveService<ChessPieceEntity> BoardMoveService(
            ChessBoardActionProvider boardActionProvider = null,
            IBoardEntityFactory<ChessPieceEntity> entityFactory = null
            )
        {
            return new BoardMoveService<ChessPieceEntity>(
                boardActionProvider ?? ChessBoardActionProvider(entityFactory)
                );
        }

        public static ChessBoardEngineProvider ChessBoardEngineProvider() =>
            new ChessBoardEngineProvider(
                ChessRefreshAllPaths(),
                PathsValidator(),
                BoardMoveService(null, null));

        public static ICheckDetectionService CheckDetectionService()
        {
            return new CheckDetectionService(
                PlayerStateService(),
                BoardMoveService(null, null),
                FindAttackPaths(),
                PathsValidator()
            );
        }

        public static ISanTokenParser SanTokenFactory() => new SanTokenParser();

        public static ChessPieceEntityFactory.ChessPieceEntityFactoryTypeExtraData MoveExtraData(Colours owner,
            ChessPieceName piece)
            => new ChessPieceEntityFactory.ChessPieceEntityFactoryTypeExtraData(owner, piece);
    }
}