namespace CSharpChess.MoveGeneration
{
    public class QueenMoveGenerator : StraightLineMoveGenerator
    {
        public QueenMoveGenerator() : base(Chess.Rules.Queen.DirectionTransformations)
        { }
    }
}