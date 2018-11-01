using System;
namespace FutbolApp.Modelo
{
    public class USUARIO
    {
        [Newtonsoft.Json.JsonProperty("Id")]
        public string Id { get; set; }

        public string Username { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool deleted { get; set; }
        public string OS { get; set; }
        public string customerId { get; set; }
        public string Email { get; set; }

        public DateTime DateUtc { get; set; }

        [Newtonsoft.Json.JsonIgnore]
        public string DateLabel { get { return DateUtc.ToLocalTime().ToString("d");}}

        [Newtonsoft.Json.JsonIgnore]
        public string TimeLabel { get { return DateUtc.ToLocalTime().ToString("t"); }}

        [Newtonsoft.Json.JsonIgnore]
        public string NameLabel { get { return deleted ? "deleted" : FirstName + LastName; } }

        [Microsoft.WindowsAzure.MobileServices.Version]
        public string AzureVersion { get; set; }

        public USUARIO(){}

        public USUARIO(string Username, string Password)
        {
            this.Username = Username;
            this.Password = Password;
        }

        public bool CheckInformation()
        {
            if (this.Username != null && this.Password != null)
                return true;
            else
                return false;
        }

    }
}
