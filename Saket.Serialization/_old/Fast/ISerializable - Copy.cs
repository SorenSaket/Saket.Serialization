namespace Saket.Serialization
{
    


    /// <summary>
    /// Interface for implementing custom serializable types.
    /// </summary>
    public interface IFastSerializable
    {
        void Serialize(FastWriter writer);
        void Deserialize(ref FastReader reader);
    }
}