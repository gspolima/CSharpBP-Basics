using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acme.Common
{
    // A singleton
    public class User
    {
        // the ctor must be private
        private User()
        {

        }

        private User instance;

        // the getter must verify wheter an instance has been created or not
        // and if it hasn't, create one.
        public User Instance
        {
            get 
            {
                if (instance == null)
                {
                    instance = new User();
                }
                return instance;
            }
        }

    }
}
