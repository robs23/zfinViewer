using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace zfinViewer
{
    class Company: ISearchable
    {
        public int ID { get; set; }
        public string SearchableString
        {
            get
            {
                return CompanyString + " " + Name + ", " + City + ", " + Country;
            }
        }

        public string Type { get; set; }
        public int Index
        {
            get
            {
                return ID;
            }
            set { }
        }
        public string Name { get; set; }
        public string CompanyString { get; set; }
        public string Address { get; set; }
        public string Zip { get; set; }
        public string City { get; set; }
        public string Country { get; set; }


    }
}
