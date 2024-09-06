# Saket.Serialization

## Binary Serialization for .Net.
An easy to use Binary Serialization and deserialization library for .Net 8. Can be used to save data to disk or networking.

## How to use


### Simple Example
```csharp
// You can write data to any byte array
byte[] myData = new byte[512]; 

// Create a new writer. Writer is a struct and doesn't allocate any Heap memory. You can create these as you please.
// It's recommended to just create a new one every time you want to write to a byte[].
ISerializer writer = new ByteWriter(myData);

// You can easily serialize any unmanaged type
Vector3 myVector = new (1.4f, 0.1f, -5f);
writer.Serialize(ref myVector); // Pass you value with ref

// You can read data from any byte[] 
ISerializer reader = new ByteReader(myData);

Vector3 newVector = Vector3.Zero;
reader.Serialize(ref newVector);

// newVector now has the value from  myVector
```

### Custom Serialization

Implement using ISerializable:

```csharp
// Implement this interface on any object you want to serialize
public interface ISerializable
{
    void Serialize(ISerializer serializer);
}
```


```csharp
// A class with custom serialization
public class MySaveFile : ISerializable
{
	public Vector3[] Positions;
	public float Health;
	public string Name;

    void Serialize(ISerializer s)
	{
		s.Serialize(ref Positions);
		s.Serialize(ref Health);
		s.Serialize(ref Name);
	}
}
```
Then you can easily convert your struct to binary:
When serializing with the ISerializable function you can combine both Serializaiton and Deserialization in the same function.

```csharp
MySaveFile save;

// Save MySaveFile to a file
byte[] myData = new byte[512]; 

var writer = new ByteWriter(myData);

writer.Serialize(ref save);


using (FileStream outputFile = File.Open("c:/myfile.txt", FileMode.OpenOrCreate))
{
    outputFile.Write(writer.DataAsSpan);
}
```

### Extras

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

## References
https://referencesource.microsoft.com/#mscorlib/system/io/binarywriter.cs


https://referencesource.microsoft.com/#mscorlib/system/io/binaryreader.cs

https://www.jacksondunstan.com/articles/3568
https://blog.tedd.no/2020/06/01/faster-c-array-access/