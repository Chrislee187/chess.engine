using System;
using chess.engine.Exceptions;

namespace chess.engine.Extensions
{
    public static class Throw
    {
        public static void InvalidBoardLocation(string location) 
            => throw new ArgumentException($"Invalid BoardLocation: {location}");

        public static void NullArgument(string argName)
            => throw new ArgumentNullException(argName);

        public static void InvalidArgument(string message, string argName)
            => throw new ArgumentException(message, argName);

        public static void NotImplemented(string message)
            => throw new NotImplementedException(message);

        public static void InvalidGameState(string message)
            => throw new Exception(message);

        public static void BoardBuilder(string message)
            => throw new Exception(message);

        public static void InvalidPiece(char piece)
            => throw new Exception($"Invalid piece '{piece}'");

        public static void UnsupportedMoveType(int type)
            => throw new ArgumentOutOfRangeException(nameof(type),$"Unsupported move type: {type}");

        public static void InvalidBoardFormat(string message) 
            => Throw.InvalidArgument(message, "serialised-board");

        public static void InvalidSan(string san)
            => throw new ArgumentException($"Invalid SAN notation: {san}");

        public static void MoveNotFound(string move)
            => throw new MoveFinderException($"Move not found: {move}");
    }
}