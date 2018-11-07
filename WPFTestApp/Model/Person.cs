using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFTestApp.Model
{
    public class Person
    {
        public int Id { get; set; }
        public string Names { get; set; }
        public string LastName { get; set; }
        public string FamilyLastName { get; set; }
        public string ParentsNames { get; set; }
        public string MotherFamilyLastName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string PlaceOfBirth { get; set; }
        public DateTime? DateOfDeath { get; set; }
        public string PlaceOfDeath { get; set; }
        public DateTime? DateOfFuneral { get; set; }
        public string PlaceOfFuneral { get; set; }
        public int PartnerId { get; set; }
        public List<int> ChildrenIdList { get; set; }
        public List<Wedding> WeddingList { get; set; }

        public Person()
        {
            ClearAllValues();
        }

        public Person(Person person, bool deepCopy=false)
        {
            Id = person.Id;
            Names = person.Names;
            LastName = person.LastName;
            FamilyLastName = person.FamilyLastName;
            ParentsNames = person.ParentsNames;
            MotherFamilyLastName = person.MotherFamilyLastName;
            DateOfBirth = person.DateOfBirth;
            PlaceOfBirth = person.PlaceOfBirth;
            DateOfDeath = person.DateOfDeath;
            PlaceOfDeath = person.PlaceOfDeath;
            DateOfFuneral = person.DateOfFuneral;
            PlaceOfFuneral = person.PlaceOfFuneral;
            PartnerId = person.PartnerId;
            if (deepCopy)
            {
                ChildrenIdList = new List<int>();
                person.ChildrenIdList.ForEach(x => ChildrenIdList.Add(x));
                WeddingList = new List<Wedding>();
                person.WeddingList.ForEach(x => WeddingList.Add(x));
            }
            else
            {
                ChildrenIdList = person.ChildrenIdList;
                WeddingList = person.WeddingList;
            }
        }

        public virtual void ClearAllValues()
        {
            Id = 0;
            Names = null;
            LastName = null;
            FamilyLastName = null;
            ParentsNames = null;
            MotherFamilyLastName = null;
            DateOfBirth = null;
            PlaceOfBirth = null;
            DateOfDeath = null;
            PlaceOfDeath = null;
            DateOfFuneral = null;
            PlaceOfFuneral = null;
            PartnerId = 0;
            ChildrenIdList = null;
            WeddingList = null;
        }
    }

    public class Wedding
    {
        public DateTime? DateOfWedding { get; set; }
        public string PlaceOfWedding { get; set; }
        public string WeddingWitness { get; set; }
        public int PartnerId { get; set; }
        public int PartnerId2 { get; set; }

        public Wedding()
        {
            ClearAllValues();
        }

        public Wedding(Person person, Person person2)
        {
            ClearAllValues();
            PartnerId = person.Id;
            PartnerId2 = person2.Id;
        }

        public Wedding(Wedding wedding)
        {
            DateOfWedding = wedding.DateOfWedding;
            PlaceOfWedding = wedding.PlaceOfWedding;
            WeddingWitness = wedding.WeddingWitness;
            PartnerId = wedding.PartnerId;
            PartnerId2 = wedding.PartnerId2;
        }

        public void ClearAllValues()
        {
            DateOfWedding = null;
            PlaceOfWedding = null;
            WeddingWitness = null;
            PartnerId = 0;
            PartnerId2 = 0;
        }
    }
}
