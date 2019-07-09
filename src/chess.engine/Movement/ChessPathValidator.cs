﻿using System;
using System.Linq;
using board.engine.Board;
using board.engine.Movement;
using chess.engine.Entities;
using chess.engine.Extensions;
using Microsoft.Extensions.Logging;

namespace chess.engine.Movement
{
    public class ChessPathValidator : IPathValidator<ChessPieceEntity>
    {
        private readonly IMoveValidationProvider<ChessPieceEntity> _validationProvider;
        private readonly ILogger<ChessPathValidator> _logger;

        public ChessPathValidator(
            ILogger<ChessPathValidator> logger,
            IMoveValidationProvider<ChessPieceEntity> validationProvider
            )
        {
            _logger = logger;
            _validationProvider = validationProvider;
        }

        public Path ValidatePath(IReadOnlyBoardState<ChessPieceEntity> boardState, Path possiblePath)
        {
            _logger?.LogDebug($"Validating path: {possiblePath}");
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

            if (validPath.Any())
            {
                _logger?.LogDebug($"Path validated as: {validPath}");
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