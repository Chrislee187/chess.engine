using System.Collections.Generic;
using System.Linq;
using board.engine;
using board.engine.Board;
using board.engine.Movement;
using chess.engine.Entities;
using chess.engine.Game;

namespace chess.engine.Movement
{
    public class ChessRefreshAllPaths : IRefreshAllPaths<ChessPieceEntity>
    {
        private readonly ICheckDetectionService _checkDetectionService;

        public ChessRefreshAllPaths(
            ICheckDetectionService checkDetectionService
            )
        {
            _checkDetectionService = checkDetectionService;
        }


        private readonly IDictionary<string, LocatedItem<ChessPieceEntity>[]> _stateCache
        = new Dictionary<string, LocatedItem<ChessPieceEntity>[]>();

        public void RefreshAllPaths(IBoardState<ChessPieceEntity> boardState, int currentPlayer)
        {
            RefreshPathsFeature(boardState, (Colours) currentPlayer);

            // NOTE: IMPORTANT: Kings must be evaluated last to ensure that moves
            // from other pieces that would cause check are generated first!
            // kings have an EntityType of int.MaxValue
            var boardStateGetAllItemLocations = boardState.GetItems()
                .OrderBy(i => i.Item.EntityType)
                .Select(i => i.Location).ToList();
            
            foreach (var loc in boardStateGetAllItemLocations)
            {
                RemovePathsThatContainMovesThatLeaveUsInCheck(boardState, loc);
            }

        }

        private void RefreshPathsFeature(IBoardState<ChessPieceEntity> boardState, Colours currentPlayer)
        {
            if (FeatureFlags.CachingPaths)
            {
                // Need proper boardstate key I think, currently a few tests fail, I guess around some state related
                // so something not encoded in the textboard (enpassant  and castle viability namely)
                var stateKey = ChessGameConvert.SerialiseBoard(boardState);
                if (_stateCache.TryGetValue(stateKey, out var items))
                {
                    boardState.UpdatePaths(items);
                }
                else
                {

                    RefreshChessPaths(boardState, currentPlayer);

                    if (FeatureFlags.CachingPaths)
                    {
                        _stateCache.Add(stateKey, boardState.GetItems().ToArray());
                    }
                }
            }
            else
            {
                RefreshChessPaths(boardState, currentPlayer);
            }
        }

        private static void RefreshChessPaths(IBoardState<ChessPieceEntity> boardState, Colours whoseTurn)
        {
            // NOTE: Kings cannot move in to check, so we regenerate their state last so the know
            // all of the enemy piece attack paths
            var nonKings = boardState.GetItems().Where(i => i.Item.EntityType != (int) ChessPieceName.King);
            var kings = boardState.GetItems().Where(i => i.Item.EntityType == (int) ChessPieceName.King).ToList();

            boardState.RefreshPathsFor(nonKings);

            // NOTE: Kings can't move next to each other, so we update the enemy kings state first so that we can
            // so when regen the friendly kings state it knows where the enemy kings can move to, and therefore
            // where it cannot

            if(whoseTurn == Colours.White)
            {
                boardState.RefreshPathsFor(kings.Where(k => k.Item.Owner == (int)Colours.Black));
                boardState.RefreshPathsFor(kings.Where(k => k.Item.Owner == (int)Colours.White));
            }
            else
            {
                boardState.RefreshPathsFor(kings.Where(k => k.Item.Owner == (int)Colours.White));
                boardState.RefreshPathsFor(kings.Where(k => k.Item.Owner == (int)Colours.Black));
            }

        }

        private void RemovePathsThatContainMovesThatLeaveUsInCheck(IBoardState<ChessPieceEntity> boardState, BoardLocation loc)
        {
            var piece = boardState.GetItem(loc);
            var validPaths = new Paths();
            foreach (var path in piece.Paths)
            {
                var validPath = ValidatePathForDiscoveredCheck(boardState, path);
                if(validPath != null) validPaths.Add(validPath);
            }
            piece.UpdatePaths(validPaths);
        }

        private Path ValidatePathForDiscoveredCheck(IBoardState<ChessPieceEntity> boardState, Path path)
        {
            var validPath = new Path(
                    path.Where(move => !_checkDetectionService.DoesMoveLeaveUsInCheck(boardState, move))
                );
            
            return !validPath.Any() ? null : validPath;
        }
    }
}