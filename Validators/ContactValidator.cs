using ContactsAPI.Models;
using FluentValidation;

namespace ContactsAPI.Validators
{
    public class ContactValidator : AbstractValidator<Contact>
    {
        public ContactValidator()
        {
            RuleFor( c => c.Fullname).NotNull().NotEmpty();
            RuleFor( c => c.Email).EmailAddress();
            RuleFor( c => c.Phone).NotNull().InclusiveBetween(10,20);
            RuleFor( c => c.Address).NotNull().MaximumLength(10);
            RuleFor(c => c.Address)
                .Must(a => a?.ToLower().Contains("street") == true).WithMessage("Address must require street");
            //RuleFor(expression: employee => employee.DateOfBirth).NotEmpty().WithMessage("DateOfBirth is required");
        }
   
    }
}
