using System;
using System.Collections.Generic;
namespace MagisterDotNet2Lib
{
    public class Link
    {
        public string href { get; set; }
    }

    public class Link2
    {
        public string Rel { get; set; }
        public string Href { get; set; }
    }

    public class Links
    {
        public Link self { get; set; }
        public Link account { get; set; }
        public Link rollen { get; set; }
        public Link ouder { get; set; }
        public Link emailadresverificatie { get; set; }

    }

    public class Session
    {
        public int id { get; set; }
        public string state { get; set; }
        public bool isVerified { get; set; }
        public bool iamEnabled { get; set; }
        public int timeToLive { get; set; }
        public DateTime expiresOn { get; set; }
        public Links links { get; set; }
    }

    public class Account
    {
        public int id { get; set; }
        public string naam { get; set; }
        public string emailadres { get; set; }
        public string mobielTelefoonnummer { get; set; }
        public string softtokenStatus { get; set; }
        public bool isEmailadresGeverifieerd { get; set; }
        public bool moetEmailadresVerifieren { get; set; }
        public Guid uuId { get; set; }
        public Links links { get; set; }
    }

    public class Rol 
    {
        public int id { get; set; }
        public string naam { get; set; }

        //1 Leerling
        //2 Ouder
    }

    public class Studie
    {
        public int Id { get; set; }
        public string Omschrijving { get; set; }
    }

    public class Groep
    {
        public int Id { get; set; }
        public List<Link2> Links { get; set; } 
        public string Omschrijving { get; set; }
        public int LocatieId { get; set; }
    }
    public class Aanmelding
    {
        public int id { get; set; }
        public List<Link2> Links { get; set; }
        public int LeerlingId { get; set; }
        public DateTime Start { get; set; }
        public DateTime Einde { get; set; }
        public string Lespriode { get; set; }
        public Studie Studie { get; set; }
        public Groep Groep { get; set; }
        public string Profiel { get; set; }
        public string Profiel2 { get; set; }
        public bool AanBronMelden { get; set; }
    }

    public class Aanmeldingen
    {
        public List<Aanmelding> Items { get; set; }
        public int TotalCount { get; set; }
        public List<Link2> Links { get; set; }

    }

    public class Persoon {
        public int Id { get; set; }
        public string Roepnaam { get; set; }
        public string Tussenvoegsel { get; set; }
        public string Achternaam { get; set; }
        public string OfficieleVoornamen { get; set; }
        public string Voorletters { get; set; }
        public string OfficieleTussenvoegsels { get; set; }
        public string OfficieleAchternaam { get; set; }
        public string Geboortedatum { get; set; }
        public string GeboorteAchternaam { get; set; }
        public string GeboortenaamTussenvoegsel { get; set; }
        public string GebruikGeboortenaam { get; set; }
    }

    public class Privilege {
        public string Naam { get; set; }
        public List<string> AccessType { get; set; }
    }

    public class Groep2
    {
        public string Naam { get; set; }
        public List<Privilege> Privileges {get; set;}
    }

    public class Account2
    {
        public Guid Uuid { get; set; }
        public Persoon Persoon { get; set; }
        public List<Groep2> Groep { get; set; }
    }

	public class Kind : Persoon
	{
		public int StamNummer { get; set; }
		public bool ZichtbaarVoorOuders { get; set; }
	}

	public class Kinderen
	{
		public List<Kind> Items { get; set; }
		public int TotalCount { get; set; }
	}
}
