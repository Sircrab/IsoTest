using System;

[Serializable]
public class Tile : IComparable, ITile
{
    public const int c_emptyTileID = 0;
    //y is height, x is column, z is row
    public int x;
    public int y;
    public int z;

    private int id = 0;
    
    public virtual int ID
    {
        get { return id; }
        set { id = value; }
    }
    
    public Tile() { }

    public Tile(int x, int y, int z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }

    public int CompareTo(object obj)
    {
        if(obj == null || !(obj is Tile))
        {
            return 1;
        }
        Tile other = obj as Tile;

        if(y.CompareTo(other.y) == 0 )
        {
            if ( z.CompareTo(other.z) == 0 )
            {
                return x.CompareTo(other.x);
            }
            return z.CompareTo(other.z);
        }
        return y.CompareTo(other.y);    
    }

    public virtual bool IsEmpty()
    {
        return false;
    }
}
