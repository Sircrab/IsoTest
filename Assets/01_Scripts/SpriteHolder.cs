using UnityEngine;
using System;

public class SpriteHolder : MonoSingleton<SpriteHolder>
{
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
            throw new InvalidTileIDException("Invalid Tile ID.");
        }
        if (m_sprites[ID] == null)
        {
            throw new NullReferenceException(String.Format("Tile with id %d is null.", ID));
        }
        return m_sprites[ID];
    }

    public class InvalidTileIDException : Exception
    {
        public InvalidTileIDException(string message) : base(message) { }
    }
}
