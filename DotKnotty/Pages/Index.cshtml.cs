using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;

namespace DotKnotty.Pages
{
    // VULNERABILITY: Insecure Deserialization
    [Serializable]
    public class ShipConfiguration
    {
        public string ShipName { get; set; }
        public int WeaponsLevel { get; set; }
        public int ShieldLevel { get; set; }
        // Imagine this method could do something dangerous if exploited
        public void DoSomething() 
        {
            Console.WriteLine("Doing something potentially dangerous!");
        }
    }

    // VULNERABILITY: Mass Assignment
    public class RepairTicket
    {
        public int Id { get; set; }
        public string ShipName { get; set; }
        public string Description { get; set; }
        public int Priority { get; set; }  // This should not be directly settable by users
        public string AssignedTechnician { get; set; }  // This should not be directly settable by users
    }

    public class IndexModel : PageModel
    {
        private static Dictionary<string, string> _savedConfigs = new Dictionary<string, string>();
        private static List<RepairTicket> _tickets = new List<RepairTicket>();
        private static int _nextTicketId = 1;

        public void OnGet()
        {
        }

        public IActionResult OnPostSaveConfig(ShipConfiguration config)
        {
            var formatter = new BinaryFormatter();
            using (var ms = new MemoryStream())
            {
                formatter.Serialize(ms, config);
                byte[] serializedConfig = ms.ToArray();
                string configKey = $"{config.ShipName}_{DateTime.Now.Ticks}";
                _savedConfigs[configKey] = Convert.ToBase64String(serializedConfig);
            }
            return new JsonResult(new { success = true, configs = _savedConfigs });
        }

        public IActionResult OnGetLoadConfig(string configKey)
        {
            if (!_savedConfigs.TryGetValue(configKey, out var serializedConfig))
                return new JsonResult(new { success = false, message = "Configuration not found" });

            var formatter = new BinaryFormatter();
            using (var ms = new MemoryStream(Convert.FromBase64String(serializedConfig)))
            {
                // VULNERABILITY: This is where the insecure deserialization happens
                var config = (ShipConfiguration)formatter.Deserialize(ms);
                return new JsonResult(new { success = true, config = config });
            }
        }

        public IActionResult OnGetConfigs()
        {
            return new JsonResult(_savedConfigs);
        }

        // VULNERABILITY: Mass Assignment
        public IActionResult OnPostCreateTicket([FromBody] RepairTicket ticket)
        {
            ticket.Id = _nextTicketId++;
            _tickets.Add(ticket);
            return new JsonResult(ticket);
        }

        // VULNERABILITY: Broken Access Control / IDOR
        public IActionResult OnGetTicket(int id)
        {
            var ticket = _tickets.Find(t => t.Id == id);
            return new JsonResult(ticket);
        }
    }
}