namespace Saket.Serialization
{
    /// <summary>
    /// Interface for implementing custom serializable types.
    /// </summary>
    public interface ISerializable
    {
        void Serialize(SerializerWriter writer);
        void Deserialize(ref SerializerReader reader);
    }
}