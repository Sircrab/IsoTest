using System;

public class EmptyTile : Tile
{
    public override int ID
    {
        get { return c_emptyTileID; }
        set { throw new InvalidOperationException("Cannot change ID of empty tile"); }
    }

    public override bool IsEmpty()
    {
        return true;
    }
}
