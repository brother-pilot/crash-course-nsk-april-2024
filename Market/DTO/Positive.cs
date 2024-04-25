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
            if (value is not int)
                return true;
            if (value is int)
                if ((int) value >= 0)
                    return true;
                else 
                    return false;
            return false;
        }
    }
}