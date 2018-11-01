using System;
namespace FutbolApp.Modelo
{
    public class PlayerRating
    {
        //in Azure
        [Newtonsoft.Json.JsonProperty("Id")]
        public string Id { get; set; }
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

        /// <summary>
        /// </summary>
        /// <value><c>true</c> if made at home; otherwise, <c>false</c>.</value>
        /// In azure
        public bool Favorite { get; set; }
        public string PlayerID { get; set; }

        /// <summary>
        /// Gets or sets the location of the coffee
        /// </summary>
        /// in azure


        public string Speed { get; set; }
        public string Shot { get; set; }
        public string Defense { get; set; }
        public string Dribbling { get; set; }
        public string Pass { get; set; }
        public string General { get; set; }


        public string Team { get; set; }

        /// <summary>
        /// Gets or sets the OS of the user
        /// </summary>
        /// <value>The OS</value>
        /// in Azure




    }
}
