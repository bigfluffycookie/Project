using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exercise
{
    public interface IIssue
    {
        string Text { get; }
        int Line { get; }
        int Column { get; }
    }
}
