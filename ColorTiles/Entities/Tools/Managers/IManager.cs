using System;

namespace ColorTiles.Entities.Tools.Managers;

public abstract class IManager<T> : IDisposable where T : class
{
    public abstract void Add(T tileSet);
    public abstract void Remove(T tileSet);
    public abstract void Remove(int index);
    public abstract T Get(int id);

    public abstract T LoadDefault();

    public abstract void Dispose();
}