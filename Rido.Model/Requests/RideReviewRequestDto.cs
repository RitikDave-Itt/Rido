using System.ComponentModel.DataAnnotations;

namespace Rido.Model.Requests
{
    public class RideReviewRequestDto
    {
      

        public string BookingId { get; set; }

        [Required(ErrorMessage = "Rating is required.")]
        [Range(0.0, 5.0, ErrorMessage = "Rate between 0 and 5.")]
        public decimal Rating { get; set; }

        [MaxLength(500, ErrorMessage = "Comment exceed 500 characters.")]
        public string Comment { get; set; }
    }
}
