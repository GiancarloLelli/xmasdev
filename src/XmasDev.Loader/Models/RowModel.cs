using System;

namespace XmasDev.Loader.Models
{
    public class RowModel
    {
        public string User { get; set; }
        public string Product { get; set; }
        public int Rating { get; set; }
        public DateTime CreatedOn { get; set; }

        public override string ToString()
        {
            return $"{User},{Product},{CreatedOn.ToString("yyyy-MM-ddTHH:mm:ss")},,{Rating}";
        }
    }
}
