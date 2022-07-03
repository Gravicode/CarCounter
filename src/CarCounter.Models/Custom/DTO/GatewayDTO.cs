using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace CarCounter.Models.Custom.DTO
{
    public class CreateGatewayDTO : GatewayDTO
    {
        public string? Nama { get; set; }
        public DateTime? TanggalPasang { get; set; }
        public string? Lokasi { get; set; }
        public string? Keterangan { get; set; }
    }

    public class GatewayDTO
    {
        public long Id { get; set; }
    }
}
