namespace TeaTime.Services
{
    using System;
    using System.Text.RegularExpressions;

    public class InventoryService
    {
        public string GenerateItemCode(ulong id, string name)
        {
            var num = ((int)Math.Floor(Math.Log10(id) + 1) >= 8) 
                    ? Convert.ToString(id) 
                    : id.ToString().PadLeft(8, '0');
            
            return $"{Regex.Replace(name, @"[^A-Za-z0-9]+", "").ToUpper()}{num}";
        }
    }
}
