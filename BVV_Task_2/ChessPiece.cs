namespace BVV_Task_2;

public abstract class ChessPiece
{
    public abstract string Name { get; }
    
    public abstract char Icon { get; }

    public abstract IEnumerable<Vector> GetMoves(Vector position);
}