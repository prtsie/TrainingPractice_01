namespace BVV_Task_2.Pieces;

public class Pawn : ChessPiece
{
    public override string Name => "Пешка";

    public override char Icon => '\u265f';

    public override IEnumerable<Vector> GetMoves(Vector position) 
        => position.Y != 8 ? [position with { Y = position.Y + 1 }] : [];
}