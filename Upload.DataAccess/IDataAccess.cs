using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Upload.Model;


namespace Upload.DataAccess
{
    public interface IDataAccess
    {
        bool SaveShirt(Shirt s);
        Shirt GetShirt();
        bool UpdateShirt(Shirt s);
        bool DeleteShirt();
    }
}
