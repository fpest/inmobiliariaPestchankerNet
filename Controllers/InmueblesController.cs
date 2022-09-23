using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using inmobiliariaPestchanker.Models;
using Microsoft.AspNetCore.Mvc;

namespace inmobiliariaPestchanker.Controllers
{
          
    public class InmueblesController : Controller
    {

       private RepositorioInmueble repo = new RepositorioInmueble();
       private RepositorioPropietario repoPro = new RepositorioPropietario();
       private RepositorioTipoInmueble repoTipoInmueble = new RepositorioTipoInmueble();
 [Authorize]
       public IActionResult Index()
        {
            try{
                var lista = repo.ObtenerTodosLista();
                 ViewBag.Mensaje = TempData["Mensaje"];
            return View(lista);
            }
            catch(Exception ex)
            {
                throw;
            }
            
        }
    
        [Authorize]
        public ActionResult Details(int id)
        {
                  try{
              var entidad = repo.ObtenerPorId(id);
              ViewBag.TipoInmueble = repoTipoInmueble.ObtenerPorId(entidad.IdTipoInmueble);
               
               if(TempData.ContainsKey("Mensaje"))
               ViewBag.Mensaje = TempData["Mensaje"];
               if(TempData.ContainsKey("Error"))
               ViewBag.Mensaje = TempData["Error"];
            return View(entidad);
          }catch(Exception e){
            throw;
          }
        }

        [Authorize]
        public ActionResult Create()
        {
                      try{

               ViewBag.Propietario = repoPro.ObtenerTodos();
               ViewBag.TipoInmueble = repoTipoInmueble.ObtenerTodos();
              
               if(TempData.ContainsKey("Mensaje"))
               ViewBag.Mensaje = TempData["Mensaje"];
               if(TempData.ContainsKey("Error"))
               ViewBag.Mensaje = TempData["Error"];
           
               return View();
          }catch(Exception e){
            throw;
          }
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Create(Inmueble inmueble)
        {
            try
            {
                repo.Alta(inmueble);
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
               ViewBag.Propietario = repoPro.ObtenerTodos();
               ViewBag.TipoInmueble = repoTipoInmueble.ObtenerTodos();
               
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
        public ActionResult Edit(int id, Inmueble inmueble)
        {
            Inmueble i = null;
            try
            {
                i = repo.ObtenerPorId(id);
                i.IdPropietario = inmueble.IdPropietario;
                i.Direccion = inmueble.Direccion;
                i.CoordenadaN = inmueble.CoordenadaN;
                i.CoordenadaE = inmueble.CoordenadaE;
                i.IdTipoInmueble = inmueble.IdTipoInmueble;
                i.TipoUso = inmueble.TipoUso;
                i.CantidadAmbientes = inmueble.CantidadAmbientes;
                i.PrecioInmueble = inmueble.PrecioInmueble;
                i.Disponible = inmueble.Disponible;
            
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
             ViewBag.TipoInmueble = repoTipoInmueble.ObtenerPorId(entidad.IdTipoInmueble);
                
               if(TempData.ContainsKey("Mensaje"))
               ViewBag.Mensaje = TempData["Mensaje"];
               if(TempData.ContainsKey("Error"))
               ViewBag.Mensaje = TempData["Error"];
            return View(entidad);
          }catch(Exception e){
            throw;
          }
        }

        // POST: Inmueble/Delete/5
        [HttpPost]
        //[ValidateAntiForgeryToken]

        [Authorize(Policy = "Administrador")]
        public ActionResult Delete(int id, Inmueble inmueble)
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
               // return View();
            }
        }
    }
}