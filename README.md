Binary Serialization for .Net.

Same implementation as the built-in BinaryReader is for some reason 1.6 times slower.

Some Aspects where inspired by serialization in COM.Unity.Netcode.Gameobjects.

Implement using ISerde or ISerializable:

```csharp
 	public interface ISerde
    {
        void Serialize(IWriter writer);
        void Deserialize(IReader reader);
    }
	
    public interface ISerializable
    {
        void Serialize(ISerializer serializer);
    }
```


The recommend use is to implement the interface ISerde:


```csharp

	public struct ComplexSerdeStruct : ISerde
	{
		public System.Numerics.Vector3 position;
		public bool isActive;
		

		public void Serialize(IWriter writer){
			writer.Write(position);
			writer.Write(isActive);
		}

		public void Deserialize(IReader reader){
			position = reader.ReadVector3();
			isActive = reader.ReadBoolean();
		}
	}
```
Then you can easily conver your struct to binary:

```csharp

	ByteWriter writer = new();
	
	ComplexSerdeStruct value = new(new Vector3(1.2f,54f, -3f), false);

	value.Serialize(writer);

	// The binary is now avaliable in writer.Data

```


The generic Serialize<T> read methods are 12 times slower compared to a specfic read functions like reader.ReadUInt64(), but add the advantage of being easier to use. ._.

When serializing with the ISerializable function you can combine both Serializaiton and Deserialization in the same function.

```csharp

void Serialize(ISerializer serializer){
	
	// Do common things
	// These are the same when ser and de
	serializer.Serialize(ref Value1);
	serializer.Serialize(ref Value2);
	
	// Reading specific 
	if(serializer.IsReader)
	{
		serializer.Serialize(ref Value3);
		Uncompress(ref Value3);
	}
	else
	{
		// Writing specific
	}
}
```

Shortcuts:
https://referencesource.microsoft.com/#mscorlib/system/io/binarywriter.cs


https://referencesource.microsoft.com/#mscorlib/system/io/binaryreader.cs

https://www.jacksondunstan.com/articles/3568
https://blog.tedd.no/2020/06/01/faster-c-array-access/