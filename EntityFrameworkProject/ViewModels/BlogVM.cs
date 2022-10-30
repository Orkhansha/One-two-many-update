using EntityFrameworkProject.Models;
using System.Collections.Generic;

namespace EntityFrameworkProject.ViewModels
{
    public class BlogVM
    {
        public string Header { get; set; }
        public string Description { get; set; }
        public List<Blog> Blog { get; set; }
    }
}
