using System.Linq;
using board.engine.Board;
using board.engine.Movement;

namespace board.engine
{
    public class BoardEngine<TEntity> where TEntity : class, IBoardEntity
    {
        private readonly IBoardMoveService<TEntity> _boardMoveService;
        public readonly IBoardState<TEntity> BoardState;

        private readonly IBoardSetup<TEntity> _boardSetup;
        private readonly IRefreshAllPaths<TEntity> _refreshAllPaths;

        public int Width { get; private set; } = 8;
        public int Height { get; private set; } = 8;

        public int CurrentPlayer { get; set; } = 0;

        public BoardEngine(IBoardSetup<TEntity> boardSetup, 
            IPathsValidator<TEntity> pathsValidator,
            IBoardMoveService<TEntity> boardMoveService
            )

            : this(boardSetup, pathsValidator, boardMoveService, new DefaultRefreshAllPaths(), 0)
        { }

        public BoardEngine(IBoardSetup<TEntity> boardSetup,
            IPathsValidator<TEntity> pathsValidator,
            IBoardMoveService<TEntity> boardMoveService,
            IRefreshAllPaths<TEntity> refreshAllPaths, 
            int currentPlayer)
        {
            CurrentPlayer = currentPlayer;
            _boardMoveService = boardMoveService;

            BoardState = new BoardState<TEntity>(pathsValidator);

            _boardSetup = boardSetup;
            _boardSetup.SetupPieces(this);

            _refreshAllPaths = refreshAllPaths;
            _refreshAllPaths.RefreshAllPaths(BoardState, CurrentPlayer);
        }

        public void ResetBoard()
        {
            ClearBoard();
            _boardSetup.SetupPieces(this);
            _refreshAllPaths.RefreshAllPaths(BoardState, CurrentPlayer);
        }

        public void ClearBoard() => BoardState.Clear();

        public BoardEngine<TEntity> AddPiece(TEntity create, BoardLocation startingLocation)
        {
            BoardState.PlaceEntity(startingLocation, create);
            create.AddMoveTo(startingLocation);
            return this;
        }

        public LocatedItem<TEntity>[,] Board
        {
            get
            {
                var pieces = new LocatedItem<TEntity>[8, 8];
                for (int x = 1; x <= Width; x++)
                {
                    for (var y = Height; y > 0; y--)
                    {
                        var location = BoardLocation.At(x, y);

                        if (BoardState.IsEmpty(location))
                        {
                            pieces[x - 1, y - 1] = null;
                        }
                        else
                        {
                            var entity = BoardState.GetItem(location);
                            pieces[x - 1, y - 1] = entity;
                        }
                    }
                }

                return pieces;
            }
        }

        public void Move(BoardMove move)
        {
            var t = move.ToString();
            _boardMoveService.Move(BoardState, move);

            _refreshAllPaths.RefreshAllPaths(BoardState, CurrentPlayer);
        }

        private class DefaultRefreshAllPaths : IRefreshAllPaths<TEntity>
        {
            public void RefreshAllPaths(IBoardState<TEntity> boardState, int currentPlayer) 
                => boardState.GetItems().ToList()
                    .ForEach(boardState.RegenerateValidatedPaths);
        }
    }
}