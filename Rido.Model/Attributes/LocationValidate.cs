//using System.ComponentModel.DataAnnotations;
//using Rido.Model.Requests;

//public class LocationValidateAttribute : ValidationAttribute
//{
//    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
//    {
//        var location = value as LocationType;

//        if (location != null && !location.HasCoordinates && string.IsNullOrEmpty(location.LocationName))
//        {
//            return new ValidationResult("Either coordinates or a location name must be provided.");
//        }

//        return ValidationResult.Success;
//    }
//}
