using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace TodoAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Deconnexion : ControllerBase
    {
        //Post : api/Deconnexion
        [HttpPost]
        public IActionResult PostDeconnexion()
        {
            Response.Cookies.Delete("Authorization");
            return RedirectToAction("Déconnexion réussi");
        } 

    }
}
