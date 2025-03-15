namespace BVV_Task_2.Pieces;

public class King : ChessPiece
{
    public override string Name => "Король";

    public override char Icon => '\u265a';
    
    public override IEnumerable<Vector> GetMoves(Vector position)
    {
        var result = new List<Vector>();
        for (var i = -1; i <= 1; i++)
        {
            if (position.Y + i > 8 || position.Y + i < 1)
            {
                continue;
            }
            for (var j = -1; j <= 1; j++)
            {
                if (i == 0 && j == 0)
                {
                    continue;
                }

                if (position.X + j > 8 || position.X + j < 1)
                {
                    continue;
                }
                
                result.Add(new(position.X + j, position.Y + i));
            }
        }

        return result;
    }
}