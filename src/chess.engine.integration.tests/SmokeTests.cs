using System;
using chess.engine.Game;
using chess.tests.utils.TestData;
using NUnit.Framework;
using Shouldly;

namespace chess.engine.integration.tests
{
    [TestFixture]
    public class SmokeTests
    {
        [Test]
        public void Should_play_the_wiki_game_with_san_moves()
        {
            var game = ChessFactory.NewChessGame();
            foreach (var move in WikiGame.Moves)
            {
                var msg = game.Move(move);
                if (!string.IsNullOrEmpty(msg.Error) && msg.Error.StartsWith("Error"))
                {
                    Assert.Fail($"Error: {msg.Error}");
                }

                Console.WriteLine(msg.Lan.ToString());
            }
            game.CheckState.ShouldBe(GameCheckState.None); // NOTE: Example game ends in a draw
        }
        [Test]
        public void Should_play_to_fools_mate()
        {
            var moves = new[] {"f3", "e5", "g4", "Qh4"};
            var game = ChessFactory.NewChessGame();
            foreach (var move in moves)
            {
                var msg = game.Move(move);
                if (!string.IsNullOrEmpty(msg.Error) && msg.Error.StartsWith("Error"))
                {
                    Assert.Fail($"Error: {msg}");
                }
            }

            Assert.That(game.CheckState, Is.EqualTo(GameCheckState.WhiteCheckmated));
        }
    }
}


