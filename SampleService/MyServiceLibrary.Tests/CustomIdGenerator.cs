using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyServiceLibrary.Abstract;
using MyServiceLibrary.Models;

namespace MyServiceLibrary.Tests
{
    public class CustomIdGenerator : IIdGenerator
    {
        private int current;

        public CustomIdGenerator(int start)
        {
            this.current = start;
        }

        public int GenerateId()
        {
            return this.current += 100;
        }

        public int GenerateId(User user)
        {
            return user.GetHashCode();
        }
    }
}
