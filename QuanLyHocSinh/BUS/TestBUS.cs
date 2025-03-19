using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BUS
{
    public class TestBUS
    {
        public static string Method()
        {
            return "BUS " + DTO.TestDTO.Method() + DAL.TestDAL.Method();
        }
    }
}
