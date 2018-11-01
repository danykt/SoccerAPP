using System;
namespace CoffeeCups
{
    public class Player
    {
        [Newtonsoft.Json.JsonProperty("Id")]
        public string playerId { get; set; }
        /// <summary>
        /// Gets or sets the user identifier.
        /// </summary>
        /// <value>The user identifier.</value>
       

        /// <summary>
        /// Gets or sets the date UTC.
        /// </summary>
        /// <value>The date UTC.</value>
        public DateTime DateUtc { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="CoffeeCups.CupOfCoffee"/> made at home.
        /// </summary>
        /// <value><c>true</c> if made at home; otherwise, <c>false</c>.</value>
        public bool deleted { get; set; }

        /// <summary>
        /// Gets or sets the location of the coffee
        /// </summary>
        public string Location { get; set; }

        /// <summary>
        /// Gets or sets the OS of the user
        /// </summary>
        /// <value>The OS</value>
        public string OS { get; set; }

        public string firstName;
        public string lastName;

        public double speed;
        public double dribbling;
        public double shot;
        public double defence;
        public double pass;




        [Newtonsoft.Json.JsonIgnore]
        public string DateDisplay { get { return DateUtc.ToLocalTime().ToString("d"); } }

        [Newtonsoft.Json.JsonIgnore]
        public string TimeDisplay { get { return DateUtc.ToLocalTime().ToString("t") + " " + OS.ToString(); } }

        [Newtonsoft.Json.JsonIgnore]
        public string AtHomeDisplay { get { return deleted ? "Unknow" : firstName + lastName; } }



        [Microsoft.WindowsAzure.MobileServices.Version]
        public string AzureVersion { get; set; }

    }
}
