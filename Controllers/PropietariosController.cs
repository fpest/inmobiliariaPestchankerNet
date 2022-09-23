using inmobiliariaPestchanker.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;

namespace inmobiliariaPestchanker.Controllers
{
    public class PropietariosController : Controller
    {

        private RepositorioPropietario repo = new RepositorioPropietario();


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

        // POST: Propietarios/Create
        [HttpPost]
        //[ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Create(Propietario propietario)
        {
            try
            {
                repo.Alta(propietario);
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
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Edit(int id, Propietario propietario)
        {
            Propietario p = null;
            try
            {
                p = repo.ObtenerPorId(id);
                p.Nombre = propietario.Nombre;
                p.Apellido = propietario.Apellido;
                p.Dni = propietario.Dni;
                p.Telefono = propietario.Telefono;
                p.Email = propietario.Email;
                repo.Modificacion(p);
                
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
            var entidad = repo.ObtenerPorId(id);
            return View(entidad);
        }

        // POST: Propietarios/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
  
        [Authorize(Policy = "Administrador")]
        public ActionResult Delete(int id, Propietario propietario)
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
                //return View();
            }
        }
    }
}