using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pract_7.models
{
    public class FMovie
    {
        private string title = "";
        public string? Title { get; set; } = null!;

        public string? Genre { get; set; } = null!;

        public int? DurationMinutes { get; set; } = null;

        public int? ReleaseYear { get; set; } = null;

        public bool? IsAvailable { get; set; } = null;
    }
}
