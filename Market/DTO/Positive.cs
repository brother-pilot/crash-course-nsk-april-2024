using System.ComponentModel.DataAnnotations;

namespace Market.DTO
{
    
    public class Positive:ValidationAttribute
    {
        public Positive() : base("Value should not be negative!")
        {
                
        }

        public override bool IsValid(object? value)
        {
            if (value is not intValue)
                return true;
            return intValue>=0;
        }
    }
}