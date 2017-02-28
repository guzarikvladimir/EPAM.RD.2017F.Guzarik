using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyServiceLibrary.Abstract
{
    public interface IPoolObjectCreator<out T, in F>
    {
        T Create();
        T Create(F obj);
    }
}
