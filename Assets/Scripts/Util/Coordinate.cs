public struct Coordinate
{
    public int x;
    public int y;

    public override string ToString()
    {
        return "[" + x + "," + y + "]";
    }

    public bool Equals( Coordinate coordinate )
    {
        return coordinate.x == x && coordinate.y == y;
    }
}