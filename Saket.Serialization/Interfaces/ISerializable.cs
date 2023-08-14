namespace Saket.Serialization
{
    /// <summary>
    /// Interface for implementing custom serializable types.
    /// Both reading and writing is combined in a single function.
    /// You can know by reading ISerializer.IsReader
    /// </summary>
    public interface ISerializable
    {
        void Serialize(ISerializer serializer);
    }
}