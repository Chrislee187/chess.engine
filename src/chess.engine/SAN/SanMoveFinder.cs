﻿using System;
using System.Collections.Generic;
using System.Linq;
using board.engine;
using board.engine.Board;
using board.engine.Movement;
using chess.engine.Entities;
using chess.engine.Exceptions;
using chess.engine.Extensions;
using chess.engine.Game;
using chess.engine.Pieces;

namespace chess.engine.SAN
{
    public class 
        SanMoveFinder
    {
        private readonly IReadOnlyBoardState<ChessPieceEntity> _boardState;

        public SanMoveFinder(IReadOnlyBoardState<ChessPieceEntity> boardState)
        {
            _boardState = boardState;
        }

        public BoardMove? Find(StandardAlgebraicNotation san, Colours forPlayer)
        {
            var t = san.ToNotation();
            if (san.CastleMove != StandardAlgebraicNotation.CastleSide.None)
            {
                return FindCastleMove(san, forPlayer);
            }

            var destination = BoardLocation.At(san.ToFileX, san.ToRankY);

            if (san.HaveFrom)
            {
                var exact = FindExactMove(san, destination);

                if (exact == null) Throw.MoveNotFound($"Move not found: {san.ToNotation()}");

                return exact;
            }

            var items = _boardState
                .GetItems((int) forPlayer, (int) san.Piece)
                .ThatCanMoveTo(destination);

            if (TryFindMove(items, destination, san, out var move)) return move;

            if (san.FromFileX.HasValue)
            {
                items = items.Where(i => i.Paths.FlattenMoves().Any(m => m.From.X == san.FromFileX.Value));
                if (TryFindMove(items, destination, san, out move)) return move;
            }

            if (san.FromRankY.HasValue)
            {
                items = items.Where(i => i.Paths.FlattenMoves().Any(m => m.From.Y == san.FromRankY.Value));
                if (TryFindMove(items, destination, san, out move)) return move;
            }


            Throw.MoveNotFound($"Couldn't disambiguate move: {san.ToNotation()}");
            return null;
        }

        private BoardMove FindCastleMove(StandardAlgebraicNotation san, Colours forPlayer)
        {
            var king = _boardState.GetItem(King.StartPositionFor(forPlayer));

            if (king == null)
            {
                Throw.MoveNotFound($"King not found: {san.ToNotation()}");
            }

            var y = forPlayer == Colours.White ? 1 : 8;

            var from = BoardLocation.At(san.FromFileX.Value, y);
            var to = BoardLocation.At(san.ToFileX, y);
            var move = king.Paths.FindMove(@from, to);

            if (move == null) Throw.MoveNotFound($"No valid castle move found: {from}{to}");

            return move;
        }

        private BoardMove FindExactMove(StandardAlgebraicNotation san, BoardLocation destination)
        {
            var from = BoardLocation.At(san.FromFileX.Value, san.FromRankY.Value);
            var item = _boardState.GetItem(@from);
            var mv = item.Paths.FindMove(@from, destination);

            var moveList = item.Paths.FlattenMoves().Select(m => m.ToChessCoords()).Aggregate((s, v) => s += $"{v},");

            if (mv == null)
            {
                Throw.MoveNotFound($"Cannot find move matching '{san.ToNotation()}', destination '{destination.ToChessCoord()}', moveList '{moveList}");
            }

            return mv;
        }

        private static bool TryFindMove(IEnumerable<LocatedItem<ChessPieceEntity>> items, BoardLocation destination,
            StandardAlgebraicNotation san,
            out BoardMove findMoveTo)
        {
            findMoveTo = null;
            var locatedItems = items as LocatedItem<ChessPieceEntity>[] ?? items.ToArray();
            if (!locatedItems.Any())
            {
                return true;
            }

            if (locatedItems.Count() == 1)
            {
                var item = locatedItems.Single();
                if (san.PromotionPiece.HasValue)
                {
                    findMoveTo = item.Paths.FlattenMoves().FindMove(item.Location, destination,
                        ChessFactory.MoveExtraData(item.Item.Player, san.PromotionPiece.Value));
                }
                else
                {
                    findMoveTo = item.FindMoveTo(destination);
                }
                return findMoveTo != null;
            }

            return false;
        }
    }
}