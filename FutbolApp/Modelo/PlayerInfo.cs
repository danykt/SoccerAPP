using System;
namespace FutbolApp.Modelo
{
    public class PlayerInfo
    {
        //in Azure
        [Newtonsoft.Json.JsonProperty("Id")]
        public string id { get; set; }
        /// <summary>
        /// Gets or sets the user identifier.
        /// </summary>
        /// <value>The user identifier.</value>
        /// 


        /// <summary>
        /// Gets or sets the date UTC.
        /// </summary>
        /// <value>The date UTC.</value>
        /// in Azure
        public DateTime DateUtc { get; set; }

       
                  

      
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string OS { get; set; }
     

        public string Team { get; set; }

      

        //Not in Azure 
        [Newtonsoft.Json.JsonIgnore]
        public string DateDisplay { get { return DateUtc.ToLocalTime().ToString("d"); } }

        //Not in Azure
        [Newtonsoft.Json.JsonIgnore]
        public string TeamDisplay { get { return Team; } }

        //Not in Azure
        [Newtonsoft.Json.JsonIgnore]
        public string NameDisplay { get { return FirstName + " " + LastName; } }
    }
}
