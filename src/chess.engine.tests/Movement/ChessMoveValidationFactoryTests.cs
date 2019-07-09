using System;
using board.engine.Movement;
using chess.engine.Movement;
using chess.engine.Movement.King;
using NUnit.Framework;
using Shouldly;

namespace chess.engine.tests.Movement
{
    [TestFixture]
    public class ChessMoveValidationFactoryTests
    {
        private ChessMoveValidationProvider _provider;

        [SetUp]
        public void SetUp()
        {
            _provider = new ChessMoveValidationProvider(new ChessValidationSteps());
        }
        [Test]
        public void FactorySupportsAllMoveTypes()
        {
            foreach (ChessMoveTypes type in Enum.GetValues(typeof(ChessMoveTypes)))
            {
                Should.NotThrow(() => _provider.Create((int)type, null), $"{type} is not support");
            }
        }
    }
}