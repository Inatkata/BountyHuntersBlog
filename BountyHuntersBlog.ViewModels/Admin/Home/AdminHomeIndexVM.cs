using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BountyHuntersBlog.ViewModels.Admin.Home
{
    public class AdminHomeIndexVM
    {
            public int Id { get; set; }
            public string UserName { get; set; } = null!;
            public string MissionTitle { get; set; } = null!;
            public string Content { get; set; } = null!;
            public bool IsDeleted { get; set; }
            public DateTime CreatedOn { get; set; }

        
    }
}

