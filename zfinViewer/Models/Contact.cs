using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
public interface ISearchable
{
    int ID { get; set; }
    int Index { get; set; }
    string Type { get; set; }
    string SearchableString { get; }
    bool IsActive { get; set; }

}

namespace zfinViewer
{
    class Contact : ISearchable
    {
        public int ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string SearchableString { get
            {
                return FirstName +' ' + LastName;
            }
        }
        public int Index
        {
            get
            {
                return ID;
            }
            set { }
        }
        public string Name
        {
            get
            {
                return FirstName + ' ' + LastName;
            }
        }

        public string Type
        {
            get
            {
                return "Kontakt";
            }
            set { }
        }

        public string PrimaryMail { get; set; }
        public string SecondaryMail { get; set; }
        public string Phone { get; set; }
        public string Mobile { get; set; }
        public bool IsActive { get; set; }
    }
}
