﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Upload.DataAccess.Model;


namespace Upload.DataAccess
{
    public interface IDataAccess
    {
        bool SaveShirt(ShirtData s);
        ShirtData ReadShirt(string path);
        bool UpdateShirt(ShirtData s);
        bool DeleteShirt();
    }
}
