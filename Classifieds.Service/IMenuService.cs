using Classifieds.Domain.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Classifieds.Service
{
    public interface IMenuService : IGenericService<Menu>
    {
        IEnumerable<Menu> FindByType(String[] types);
    }
}
