using System.Collections.Generic;

namespace NptExplorer.AzureFunctions.Models.Transient;

// Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class Geometry
    {
        public string type { get; set; }
        public List<List<List<double>>> coordinates { get; set; }
    }

    public class Properties
    {
        public object accessibility { get; set; }
        public object accessibility_cy { get; set; }
        public string bio_images { get; set; }
        public string bio_images_cy { get; set; }
        public object comments { get; set; }
        public object comments_cy { get; set; }
        public string description { get; set; }
        public string description_cy { get; set; }
        public string difficulty { get; set; }
        public string difficulty_cy { get; set; }
        public string formatted_length { get; set; }
        public string formatted_length_cy { get; set; }
        public string formatted_time { get; set; }
        public string formatted_time_cy { get; set; }
        public string gid { get; set; }
        public object grid_ref { get; set; }
        public string habitat { get; set; }
        public string habitat_cy { get; set; }
        public string hours { get; set; }
        public string hours_cy { get; set; }
        public string length_km { get; set; }
        public string length_km_cy { get; set; }
        public string mins { get; set; }
        public string mins_cy { get; set; }
        public string name { get; set; }
        public string name_cy { get; set; }
        public string nearest_town { get; set; }
        public string nearest_valley { get; set; }
        public string nearest_valley_cy { get; set; }
        public string neatest_town_cy { get; set; }
        public string start_id { get; set; }
        public string start_id_cy { get; set; }
        public string start_point { get; set; }
        public string start_point_cy { get; set; }
        public string visible { get; set; }
        public string visible_cy { get; set; }
        public string walk_time { get; set; }
        public string walk_time_cy { get; set; }
        public string worksheet { get; set; }
        public string worksheet_cy { get; set; }
        public List<string> images { get; set; }
        public List<string> images_bio { get; set; }
    }

    public class TrailRouteResponse
    {
        public string type { get; set; }
        public Geometry geometry { get; set; }
        public Properties properties { get; set; }
    }

