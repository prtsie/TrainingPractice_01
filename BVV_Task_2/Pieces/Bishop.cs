namespace BVV_Task_2.Pieces;

public class Bishop : ChessPiece
{
    public override string Name => "Слон";
    
    public override char Icon => '\u265d';

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
        }

        return result;
    }
}