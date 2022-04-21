using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Classifieds.Domain.Models
{
    [Table(name: "advert")]
    public class Advert
    {
        [Key]
        [Column(name: "pk_advert_id")]
        [ReadOnly(true)]
        public long ID { set; get; }

        [Column(name: "approved_date")]
        public DateTime? ApprovedDate { set; get; }

        [Column(name: "published_date")]
        public DateTime? PublishedDate { set; get; }

        [Column(name: "rejected_date")]
        public DateTime? RejectedDate { set; get; }

        [Column(name: "submitted_date")]
        public DateTime SubmittedDate { set; get; }

        [Column(name: "fk_detail_id")]
        [ReadOnly(true)]
        public long DetailID { set; get; }

        [ForeignKey("DetailID")]
        [ReadOnly(true)]
        public AdvertDetail? Detail { set; get; }
    }
}