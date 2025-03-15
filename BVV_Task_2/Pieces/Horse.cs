
namespace BVV_Task_2.Pieces;

public class Horse : ChessPiece
{
    public override string Name => "Конь";

    public override char Icon => '\u265e';
    
    public override IEnumerable<Vector> GetMoves(Vector position)
    {
        var result = new List<Vector>();
        if (position.X + 3 <= 8 && position.Y + 1 <= 8)
        {
            result.Add(new(position.X + 3, position.Y + 1));
        }
        if (position.X + 3 <= 8 && position.Y - 1 > 0)
        {
            result.Add(new(position.X + 3, position.Y - 1));
        }
        if (position.X - 3 > 0 && position.Y + 1 <= 8)
        {
            result.Add(new(position.X - 3, position.Y + 1));
        }
        if (position.X - 3 > 0 && position.Y - 1 > 0)
        {
            result.Add(new(position.X - 3, position.Y - 1));
        }
        if (position.X + 1 <= 8 && position.Y + 3 <= 8)
        {
            result.Add(new(position.X + 1, position.Y + 3));
        }
        if (position.X + 1 <= 8 && position.Y - 3 > 0)
        {
            result.Add(new(position.X + 1, position.Y - 3));
        }
        if (position.X - 1 > 0 && position.Y + 3 <= 8)
        {
            result.Add(new(position.X - 1, position.Y + 3));
        }
        if (position.X - 1 > 0 && position.Y - 3 > 0)
        {
            result.Add(new(position.X - 1, position.Y - 3));
        }

        return result;
    }
}