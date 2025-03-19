using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class TestDAL
    {
        public static string Method()
        {
            return "DAL " + DTO.TestDTO.Method();
        }
    }
}
