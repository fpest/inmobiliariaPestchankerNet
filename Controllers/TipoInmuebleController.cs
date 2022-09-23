
using inmobiliariaPestchanker.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;

namespace inmobiliariaPestchanker.Controllers
{
          
    public class TipoInmueblesController : Controller
    {
       private RepositorioTipoInmueble repo = new RepositorioTipoInmueble();
 [Authorize]
       public IActionResult Index()
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

        public ActionResult Create()
        {
        try{
             return View();

          }catch(Exception e){

            throw;
          }
           
        }

        // POST: Inmueble/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
       

        [Authorize]
        public ActionResult Create(TipoInmueble tipoinmueble)
        {
            try
            {
                repo.Alta(tipoinmueble);
               return RedirectToAction("Create", "Inmuebles"); 
            }
            catch(Exception e)
            {
                throw;
                
            }
        }

        [Authorize]
        public ActionResult Edit(int id)
        {
                      try{
               var entidad = repo.ObtenerTodos();
               ViewBag.TipoInmueble = repo.ObtenerTodos();
               if(TempData.ContainsKey("Mensaje"))
               ViewBag.Mensaje = TempData["Mensaje"];
               if(TempData.ContainsKey("Error"))
               ViewBag.Mensaje = TempData["Error"];
               
            return View(entidad);
          }catch(Exception e){
            throw;
          }
                   }

        [HttpPost]
        //[ValidateAntiForgeryToken]
    
        [Authorize]
        public ActionResult Edit(int id, TipoInmueble tipoinmueble)
        {
            TipoInmueble i = null;
            try
            {
                i.Id = tipoinmueble.Id;
                i.Descripcion = tipoinmueble.Descripcion;
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Inmueble/Delete/5
        /*
        public ActionResult Delete(int id)
        {
          //     var entidad = repo.ObtenerPorId(id);
         //   return View(entidad);
        }
        */

        // POST: Inmueble/Delete/5
        [HttpPost]
        //[ValidateAntiForgeryToken]

      [Authorize(Policy = "Administrador")]
       public ActionResult Delete(int id, TipoInmueble tipoinmueble)
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