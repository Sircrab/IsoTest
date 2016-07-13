
using NUnit.Framework;
using UnityEngine;
using System;

[TestFixture()]
public class SpriteHolderTest
{
    private SpriteHolder instance;
    [SetUp()]
    public void Init()
    {
        instance = SpriteHolder.instance;
    }

    [Test()]
    public void GetSpriteByID_throwsException_onInvalidIDNegative()
    {
        Assert.Throws<SpriteHolder.InvalidTileIDException>(() => instance.GetSpriteByID(-1));
    }

    [Test()]
    public void GetSpriteByID_throwsException_onInvalidIDPositive()
    {
        Assert.Throws<SpriteHolder.InvalidTileIDException>(() => instance.GetSpriteByID(int.MaxValue));
    }

    [Test()]
    public void GetSpriteByID_throwsException_onNullSprite()
    {
        Sprite[] sprites = new Sprite[] { new Sprite(), null, new Sprite() };
        instance.Sprites = sprites;

        Assert.Throws<NullReferenceException>(() => instance.GetSpriteByID(1));
    }
}