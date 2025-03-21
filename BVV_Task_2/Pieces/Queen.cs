namespace BVV_Task_2.Pieces;

public class Queen : ChessPiece
{
    public override string Name => "Ферзь";

    public override char Icon => '\u265b';
    
    public override IEnumerable<Vector> GetMoves(Vector position)
    {
        var result = new List<Vector>();
        for (var i = -8; i <= 8; i++)
        {
            if (i == 0)
            {
                continue;
            }
            if (position.X + i <= 8 && position.X + i > 0
                && position.Y + i <= 8 && position.Y + i > 0)
            {
                result.Add(new(position.X + i, position.Y + i));
            } // /

            if (position.X + i <= 8 && position.X + i > 0
             && position.Y - i <= 8 && position.Y - i > 0)
            {
                result.Add(new(position.X + i, position.Y - i));
            } // \

            if (position.X + i <= 8 && position.X + i > 0
             && position.Y > 0 && position.Y <= 8)
            {
                result.Add(new(position.X + i, position.Y));
            } // -

            if (position.Y + i <= 8 && position.Y + i > 0
             && position.X > 0 && position.X <= 8)
            {
                result.Add(new(position.X, position.Y + i));
            } // |
        }

        return result;
    }
}