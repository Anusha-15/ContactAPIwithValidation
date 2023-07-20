using ContactsAPI.Data;
using ContactsAPI.Models;
using ContactsAPI.Validators;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ContactsAPI.Controllers
{ 
    [ApiController]
    [Route("api/[controller]")]
    public class ContactsController : Controller
    {
        private readonly ContactsAPIDbContext dbContext;
        public ContactsController(ContactsAPIDbContext dbContext)
        {
            this.dbContext = dbContext;
         
        }
        [HttpGet]
        
        public async Task<IActionResult> GetContacts()
        {
            return Ok(await dbContext.Contacts.ToListAsync());


            
        }
        [HttpGet]
        [Route("{id:guid}")]
        public async Task<IActionResult> GetContact([FromRoute]Guid id)
        {
            var Contact = await dbContext.Contacts.FindAsync(id);

            if(Contact== null)
            {
                return NotFound();
            }
            return Ok (Contact);
        }
        
        [HttpPost]
        public async Task< IActionResult> AddContacts(AddContactRequest addContactRequest)
        {
            var Contact = new Contact()

            {
                Id = Guid.NewGuid(),
                Address = addContactRequest.Address,
                Email = addContactRequest.Email,
                Fullname = addContactRequest.Fullname,
                Phone = addContactRequest.Phone,


            };
            var validator = new ContactValidator();
            var errors = validator.Validate(Contact);
            if (!errors.IsValid)
            {
                return BadRequest(errors);
            }

            await dbContext.Contacts.AddAsync(Contact);
            await dbContext.SaveChangesAsync();

            return Ok(Contact);
            
           
        }
        [HttpPost]

        [HttpPut]
        [Route("{id:guid}")]
        public async Task<ActionResult> UpdateContact([FromRoute]Guid id, UpdateContactRequest updateContactRequest)
        {
            var Contact =await dbContext.Contacts.FindAsync(id);

            if(Contact != null)
            {
                Contact.Fullname = updateContactRequest.Fullname;
                Contact.Phone = updateContactRequest.Phone;
                Contact.Email = updateContactRequest.Email;
                Contact.Address = updateContactRequest.Address;

                await dbContext.SaveChangesAsync();

                return Ok(Contact);

            }
            return NotFound();


        }

        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteContact([FromRoute] Guid id)
        {
            var Contact = await dbContext.Contacts.FindAsync(id);

            if (Contact != null)
            {
                dbContext.Remove(Contact);
                await dbContext.SaveChangesAsync();
                return Ok(Contact);
            }

            return NotFound();
        }
    }
}
