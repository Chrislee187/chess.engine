using board.engine;
using board.engine.Board;
using chess.engine.Entities;

namespace chess.engine.Game
{
    public class ChessBoardEngineProvider : IBoardEngineProvider<ChessPieceEntity>
    {
        private readonly IRefreshAllPaths<ChessPieceEntity> _refreshAllPaths;
        private readonly IPathsValidator<ChessPieceEntity> _chessPathsValidator;
        private readonly IBoardMoveService<ChessPieceEntity> _boardMoveService;

        public ChessBoardEngineProvider(
            IRefreshAllPaths<ChessPieceEntity> refreshAllPaths,
            IPathsValidator<ChessPieceEntity> chessPathsValidator,
            IBoardMoveService<ChessPieceEntity> boardMoveService
        )
        {
            _boardMoveService = boardMoveService;
            _chessPathsValidator = chessPathsValidator;
            _refreshAllPaths = refreshAllPaths;
        }
        public BoardEngine<ChessPieceEntity> Provide(IBoardSetup<ChessPieceEntity> boardSetup, int currentPlayer)
        {
            return new BoardEngine<ChessPieceEntity>(boardSetup,
                _chessPathsValidator,
                _boardMoveService,
                _refreshAllPaths,
                currentPlayer);
        }
    }
}