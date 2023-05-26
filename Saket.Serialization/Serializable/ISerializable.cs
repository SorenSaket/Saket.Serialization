namespace Saket.Serialization
{
    /// <summary>
    /// Interface for implementing custom serializable types.
    /// </summary>
    public interface ISerializable
    {
        void Serialize(ISerializer serializer);
    }
}