
/// <summary> Объект, который можно помыть </summary>
public interface ICleanable {
    /// <summary> Метод помыть/почистить </summary>
    void Clean();
}

/// <summary> Объект, который можно кликнуть мышкой </summary>
public interface ITouchable
{
    void Touch();
}