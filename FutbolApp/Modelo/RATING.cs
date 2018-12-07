using System;
namespace FutbolApp.Modelo
{
    public class RATING
    {
        [Newtonsoft.Json.JsonProperty("Id")]
        public string Id { get; set; }





        public string UserID { get; set; }
        public string PlayerInfoID { get; set; }
        public string Pass { get; set; }
        public string Dribbling { get; set; }
        public string Speed { get; set; }
        public string Shot { get; set; }
        public string Defense { get; set; }
        public bool deleted { get; set; }

        public double getGeneralRating()
        {
            try
            {
                double general = (Int32.Parse(Pass) + Int32.Parse(Dribbling) + Int32.Parse(Speed) + Int32.Parse(Shot) + Int32.Parse(Defense)) / 5.0;
                return general;
            }catch(Exception ex)
            {
                return 0.0;
            }
        }

        public DateTime DateUtc { get; set; }
    }
}
