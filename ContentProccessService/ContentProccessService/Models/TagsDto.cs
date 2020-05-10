using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContentProccessService.Application.Dtos
{
    public class TagsDto
    {
        public string Name { get; set; }
        public bool? IsActive { get; set; }

        public TagsDto(string name, bool? isActive)
        {
            Name = name;
            IsActive = isActive;
        }
    }
}
