using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using inmobiliariaPestchanker.Models;
using Microsoft.AspNetCore.Mvc;

namespace inmobiliariaPestchanker.Controllers
{


    public class PagosController : Controller

    {


        
        private RepositorioPago repo = new RepositorioPago();
        
        private RepositorioContrato repoContrato = new RepositorioContrato();
        private RepositorioInmueble repoInmueble = new RepositorioInmueble();
        private RepositorioInquilino repoInquilino = new RepositorioInquilino();
 [Authorize]
        public IActionResult Index(int idContrato)
        {
             try{
                var lista = repo.ObtenerPorContrato(idContrato);
              
              ViewBag.IdContrato = idContrato;
                 if(TempData.ContainsKey("Mensaje"))
                ViewBag.Mensaje = TempData["Mensaje"];
                if(TempData.ContainsKey("Error"))
                ViewBag.Mensaje = TempData["Error"];
  
                return View(lista);
               
            }
            catch(Exception e)
            {
                throw;
            }
        }

        
        [Authorize]
        public ActionResult Details(int id)
        {
              try{
                var detalle = repo.ObtenerPorId(id);
                return View(detalle);
            }
            catch(Exception ex)
            {
                throw;
            }        }

        [Authorize]
        public ActionResult Create(int idContrato)
        {
          try{
                   var lista = repo.ObtenerInfoPorContrato(idContrato);
                 ViewBag.Mensaje = TempData["Mensaje"];

                return View(lista);

          }catch(Exception e){

            throw;
          }
      }

        // POST: Inquilinos/Create
        [HttpPost]
        //[ValidateAntiForgeryToken]
        
        [Authorize]
        public ActionResult Create(Pago pago)
        {
            try
            {
              repo.Alta(pago);
                return RedirectToAction("Index", "Contratos"); 
            }
            catch(Exception e)
            {
                return View();
            }
        }

        //Sólo el administrador puede Editar un pago(me parece que seria mejor asi) 
         [Authorize(Policy = "Administrador")]
        public ActionResult Edit(int id)

        
        {
        try{
              var entidad = repo.ObtenerPorId(id);
              if(TempData.ContainsKey("Mensaje"))
               ViewBag.Mensaje = TempData["Mensaje"];
               if(TempData.ContainsKey("Error"))
               ViewBag.Mensaje = TempData["Error"];
           
            return View(entidad);

          }catch(Exception e){
           throw;
          }

        }

        // POST: Inquilinos/Edit/5
        [HttpPost]
        //[ValidateAntiForgeryToken]
        
        //Sólo el administrador puede Editar un pago(me parece que seria mejor asi) 
         [Authorize(Policy = "Administrador")]
        public ActionResult Edit(int id, PagoLista pagoLista)
        {
            PagoLista i = null;
            try
            {
                i = repo.ObtenerPorId(id);
                i.FechaPago = pagoLista.FechaPago;
                i.Importe = pagoLista.Importe;
                repo.Modificacion(i);
                
                return RedirectToAction("Index", new { IdContrato = pagoLista.IdContrato});
            }
            catch
            {
                throw;
            }
        }

        [Authorize(Policy = "Administrador")]      
        public ActionResult Delete(int id)
        {
          try{
            var entidad = repo.ObtenerPorId(id);
           
              
            return View(entidad);
          }catch(Exception e){

            throw;
          }
        }

        // POST: Inquilinos/Delete/5
        [HttpPost]
        //[ValidateAntiForgeryToken]

        [Authorize(Policy = "Administrador")]
        public ActionResult Delete(int id, PagoLista pagolista)
        {
            try
            {
               repo.Baja(id);

                return RedirectToAction("Index", "Contratos");
            
            }
            catch
            {
                    TempData["Mensaje"] = "No se pudo Borrar el Registro";
                return RedirectToAction(nameof(Index));
            }
        }
    }
}