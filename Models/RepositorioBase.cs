using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace inmobiliariaPestchanker.Models;


	public abstract class RepositorioBase
	{
    protected String connectionString = "Server=localhost;User=root;Password=;Database=inmobiliariapestchanker;SslMode=none";

        protected RepositorioBase()
        {
            
            }
    }

