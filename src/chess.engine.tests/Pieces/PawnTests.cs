using chess.engine.Game;
using chess.engine.Pieces;
using NUnit.Framework;
using Shouldly;

namespace chess.engine.tests.Pieces
{
    [TestFixture]
    public class PawnTests
    {
        [TestCase(Colours.White, 2)]
        [TestCase(Colours.Black, 7)]
        public void Should_have_correct_pawn_starting_ranks(Colours player, int rank)
        {
            Pawn.StartRankFor(player).ShouldBe(rank);
        }

        [TestCase(Colours.White, 5)]
        [TestCase(Colours.Black, 4)]
        public void Should_have_correct_enpassant_ranks(Colours player, int rank)
        {
            Pawn.EnPassantRankFor(player).ShouldBe(rank);
        }
    }
}