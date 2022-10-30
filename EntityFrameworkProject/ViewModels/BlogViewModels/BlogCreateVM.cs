﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EntityFrameworkProject.ViewModels.BlogViewModels
{
    public class BlogCreateVM
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Can't be empty")]
        public List<IFormFile> Photos { get; set; }
        public string Title { get; set; }
        public string Desc { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
