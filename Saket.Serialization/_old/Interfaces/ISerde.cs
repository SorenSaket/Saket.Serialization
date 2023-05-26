using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saket.Serialization
{
    /// <summary>
    /// Interface for implementing custom serializable types.
    /// </summary>
    public interface ISerde
    {
        void Serialize(IWriter writer);
        void Deserialize(IReader reader);
    }
}