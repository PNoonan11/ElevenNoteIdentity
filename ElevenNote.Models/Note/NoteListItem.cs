using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ElevenNote.Models.Note
{
    public class NoteListItem
    {
        public int NoteId { get; set; }
        public string Title { get; set; }
        [Display(Name = "Created")]
        public DateTimeOffset CreatedUtc { get; set; }
        public string CategoryName { get; set; }
    }
}

