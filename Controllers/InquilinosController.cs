using inmobiliariaPestchanker.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;

namespace inmobiliariaPestchanker.Controllers
{

  
public class InquilinosController : Controller
    {
        private RepositorioInquilino repo = new RepositorioInquilino();  
        // GET: Inquilinos

 [Authorize]
        public ActionResult Index()
        {
          try{
            var lista = repo.ObtenerTodos();
            ViewBag.Mensaje = TempData["Mensaje"];
            return View(lista);

          }catch(Exception e){
            throw;
          }
        }

        [Authorize]
        public ActionResult Details(int id)
        {
          try{
              var entidad = repo.ObtenerPorId(id);
            return View(entidad);
          }catch(Exception e){
            throw;
          }
        }

        [Authorize]
        public ActionResult Create()
        {
          try{
           return View();

          }catch(Exception e){
            throw;
          }
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]

        [Authorize]
        public ActionResult Create(Inquilino inquilino)
        {
            try
            {
                repo.Alta(inquilino);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        [Authorize]
        public ActionResult Edit(int id)
        {
                      try{

            var entidad = repo.ObtenerPorId(id);
            return View(entidad);
        }catch(Exception e){
            throw;
        }
            }

        [HttpPost]
        //[ValidateAntiForgeryToken]

        [Authorize]
        public ActionResult Edit(int id, Inquilino inquilino)
        {
            Inquilino i = null;
            try
            {
                i = repo.ObtenerPorId(id);
                i.Nombre = inquilino.Nombre;
                i.Apellido = inquilino.Apellido;
                i.Dni = inquilino.Dni;
                i.Telefono = inquilino.Telefono;
                i.Email = inquilino.Email;
                repo.Modificacion(i);
                
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        [Authorize(Policy = "Administrador")]
        public ActionResult Delete(int id)
        {
            try{
            var entidad = repo.ObtenerPorId(id);
            return View(entidad);
            }
            catch(Exception e){
            throw;
            }
            
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        [Authorize(Policy = "Administrador")]
        public ActionResult Delete(int id, Inquilino inquilino)
        {
            try
            {
               repo.Baja(id);
               return RedirectToAction(nameof(Index));
            }
            catch
            {
                TempData["Mensaje"] = "No se pudo Borrar el Registro";
                return RedirectToAction(nameof(Index));
            }
        }
    }
}