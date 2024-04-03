using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace HouseholdAppliancesApp.Server.Models.ConData
{
    [Table("HouseholdAppliances", Schema = "dbo")]
    public partial class HouseholdAppliance
    {

        [NotMapped]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("@odata.etag")]
        public string ETag
        {
                get;
                set;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ApplianceID { get; set; }

        [Required]
        [ConcurrencyCheck]
        public string ApplianceName { get; set; }

        [ConcurrencyCheck]
        public decimal? Price { get; set; }

    }
}