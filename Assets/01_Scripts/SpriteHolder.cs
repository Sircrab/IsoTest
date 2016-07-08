using UnityEngine;
using System.Collections;
using System;

public class SpriteHolder : MonoSingleton<SpriteHolder> {

    [SerializeField]
    private Sprite[] m_sprites;
    public Sprite[] Sprites
    {
        set { m_sprites = value; }
    }

    public Sprite GetSpriteByID(int ID)
    {
        if (ID < 0 || ID >= m_sprites.Length)
        {
            throw new InvalidTileIDException("Invalid Tile ID");
        }
        if(m_sprites[ID] == null)
        {
            throw new NullTileException("Tile is not correctly set in array");
        }
        return m_sprites[ID];
    }

    public class InvalidTileIDException : Exception
    {
        public InvalidTileIDException(string message) : base(message) { }
    }

    public class NullTileException : Exception
    {
        public NullTileException(string message) : base(message) { }
    }

}
