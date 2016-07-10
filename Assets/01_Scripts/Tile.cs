using System;

[System.Serializable]
public class Tile : IComparable
{ 
    //y is height, x is column, z is row
    public int x, y, z;
    public int id = 0;

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
}
