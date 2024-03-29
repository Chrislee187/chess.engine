﻿using chess.engine.Game;
using NUnit.Framework;

namespace chess.engine.integration.tests
{
    [TestFixture]
    public class ChessGameTests
    {
        private ChessGame _game;

        [SetUp]
        public void Setup()
        {
            _game = ChessFactory.NewChessGame();
        }
        [Test]
        public void New_game_should_have_white_as_first_played()
        {
            Assert.That(_game.CurrentPlayer, Is.EqualTo(Colours.White));
        }

        [Test]
        public void Move_should_update_current_player_when_valid()
        {
            var msg = _game.Move("d2d4");
            Assert.AreEqual(_game.CurrentPlayer, Colours.Black);
        }

        [TestCase(Colours.White, 8)]
        [TestCase(Colours.Black, 1)]
        public void EndRankFor_is_correct_for_both_colours(Colours colour, int rank)
        {
            Assert.That(ChessGame.EndRankFor(colour), Is.EqualTo(rank), $"{colour}");
        }

        [Test]
        public void GameState_some_checks_for_this()
        {
            Assert.Inconclusive();

        }
    }
}