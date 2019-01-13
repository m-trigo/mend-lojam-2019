public struct Coordinate
{
    public int x;
    public int y;

    public Coordinate( int x, int y )
    {
        this.x = x;
        this.y = y;
    }

    public override string ToString()
    {
        return "[" + x + "," + y + "]";
    }

    public bool Equals( Coordinate coordinate )
    {
        return coordinate.x == x && coordinate.y == y;
    }
}