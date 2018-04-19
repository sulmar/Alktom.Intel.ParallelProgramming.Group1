using Alktom.Intel.ParallelDev.WPFClient.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alktom.Intel.ParallelDev.WPFClient.ViewModels
{
    public class PersonsViewModel
    {
        public IList<Person> Persons { get; set; }

        public PersonsViewModel()
        {
            this.Persons = new List<Person>
            {
                new Person { FirstName = "Łukasz" },
                new Person { FirstName = "Jacek" },
                new Person { FirstName = "Michał" },
            };
        }

           
    }
}
