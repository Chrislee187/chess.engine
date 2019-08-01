using System.Diagnostics;
using System.Linq;
using board.engine;
using board.engine.Board;
using board.engine.Movement;
using chess.engine.Entities;
using chess.engine.Extensions;
using chess.engine.SAN;

namespace chess.engine.Game
{
    public class ChessGame
    {
        private readonly BoardEngine<ChessPieceEntity> _engine;
        private readonly ICheckDetectionService _checkDetectionService;

        private SanMoveFinder _sanMoveFinder;

        public static bool OutOfBounds(int value) => value < 1 || value > 8;
        public GameCheckState CheckState { get; private set; }
        public bool InProgress => CheckState == GameCheckState.None;
        public LocatedItem<ChessPieceEntity>[,] Board => _engine.Board;

        public IBoardState<ChessPieceEntity> BoardState => _engine.BoardState;

        public Colours CurrentPlayer => (Colours) _engine.CurrentPlayer;

        public ChessGame(
            IBoardEngineProvider<ChessPieceEntity> boardEngineProvider,
            IBoardEntityFactory<ChessPieceEntity> entityFactory,
            ICheckDetectionService checkDetectionService
        )
            : this(boardEngineProvider, checkDetectionService, new ChessBoardSetup(entityFactory))
        {
        }

        public ChessGame(
            IBoardEngineProvider<ChessPieceEntity> boardEngineProvider,
            ICheckDetectionService checkDetectionService,
            IBoardSetup<ChessPieceEntity> setup,
            Colours whoseTurn = Colours.White)
        {
            _engine = boardEngineProvider.Provide(setup, (int) whoseTurn);

            _checkDetectionService = checkDetectionService;
            _engine.CurrentPlayer = (int) whoseTurn;
            CheckState = _checkDetectionService.Check(BoardState);
        }

        public string Move(string input)
        {
            if (!StandardAlgebraicNotation.TryParse(input, out var san))
            {
                // TODO: More detailed error
                return $"Error: invalid move {input}, are you using upper-case for Files?";
            }

            _sanMoveFinder = new SanMoveFinder(_engine.BoardState);

            var move = _sanMoveFinder.Find(san, (Colours) _engine.CurrentPlayer);

            if (move == null)
            {
                return $"Error: No matching move found: {input}";
            }

            var validMove = PlayValidMove(move);
            return validMove;

        }

        private string PlayValidMove(BoardMove move)
        {
            ClearPawnTwoStepState();

            var preMove = _engine.BoardState.ToTextBoard();
            _engine.Move(move);

            if (preMove == _engine.BoardState.ToTextBoard())
            {
                Debugger.Break();
            }

            _engine.CurrentPlayer = (int) NextPlayer();
            CheckState = _checkDetectionService.Check(BoardState);

            return CheckState != GameCheckState.None
                ? CheckState.ToString()
                : "";

        }

        private void ClearPawnTwoStepState()
        {
            _engine.BoardState.GetItems()
                .Where(i => i.Item is PawnEntity)
                .Select(i => i.Item as PawnEntity)
                .ForEach(p => p.TwoStep = false);
        }

        private Colours NextPlayer() => (Colours) _engine.CurrentPlayer == Colours.White ? Colours.Black : Colours.White;

        #region Meta Info

        public static int DirectionModifierFor(Colours player) => player == Colours.White ? +1 : -1;
        public static int EndRankFor(Colours colour) => colour == Colours.White ? 8 : 1;

        #endregion
    }

    public enum PlayerState
    {
        None,
        Check,
        Checkmate
    }
}