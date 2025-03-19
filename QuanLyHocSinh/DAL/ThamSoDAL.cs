using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public static class ThamSoDAL
    {
        public static int LayTuoiToiDa()
        {
            string strCmd = "Select TuoiToiDa From ThamSo";
            return (byte)DAL.DataProvider.ExecuteScalar(strCmd);
        }

        public static int LayTuoiToiThieu()
        {
            string strCmd = "Select TuoiToiThieu From ThamSo";
            return (byte)DAL.DataProvider.ExecuteScalar(strCmd);
        }
    }
}
