﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReizzzToDo.BAL.Services.Utils.PasswordHasher
{
    public interface IPasswordHasher
    {
        public string Hash(string password);
        public bool Verify(string password, string passwordHashed);
    }
}
