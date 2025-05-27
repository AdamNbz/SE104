using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO;

public class ThamSo
{
    public int Id { get; set; }
    public int TuoiToiDa { get; set; }
    public int TuoiToiThieu { get; set; }
    public int SiSoToiDa { get; set; }
}
