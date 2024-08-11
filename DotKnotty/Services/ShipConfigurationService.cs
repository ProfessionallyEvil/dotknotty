using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using DotKnotty.Models;

namespace DotKnotty.Services
{
    public class ShipConfigurationService
    {
        public ShipConfigurationService()
        {
            Console.WriteLine("WARNING: This service contains an intentional vulnerability for demonstration purposes.");
            Console.WriteLine("This should NEVER be used in a production environment.");
        }

        public ShipConfiguration LoadConfiguration(string base64Data)
        {
        Console.WriteLine("WARNING: Processing potentially untrusted data. This is a security risk!");
        byte[] data = Convert.FromBase64String(base64Data);
        using (MemoryStream ms = new MemoryStream(data))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                object deserializedObject = formatter.Deserialize(ms);

                // Check the type of the deserialized object
                if (deserializedObject is ShipConfiguration config)
                {
                    return config;
                }
                else if (deserializedObject is VulnerableClass vulnerableObj)
                {
                    Console.WriteLine("Deserialized vulnerable class");
                    // If the object is of type VulnerableClass, execute the command
                    vulnerableObj.ExecuteCommand();
                    // You might want to return null or handle the scenario where a VulnerableClass was deserialized
                    return null; // Or throw an exception if this should not happen
                }
                else
                {
                    throw new InvalidOperationException("Deserialized object is not of a recognized type.");
                }
            }
        }

        public string SaveConfiguration(ShipConfiguration config)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(ms, config);
                return Convert.ToBase64String(ms.ToArray());
            }
        }
    }
}