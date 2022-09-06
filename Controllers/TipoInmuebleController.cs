using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using inmobiliariaPestchanker.Models;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace inmobiliariaPestchanker.Controllers
{
          
    public class TipoInmueblesController : Controller
    {

       private RepositorioTipoInmueble repo = new RepositorioTipoInmueble();
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
    
        // GET: Inmueble/Details
        /*
        public ActionResult Details(int id)
        {
           //  var entidad = repo.ObtenerTodos(id);
                      // return View(entidad);
        }
        */

        // GET: Inmueble/Create
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
        public ActionResult Create(TipoInmueble tipoinmueble)
        {
            try
            {
                // TODO: Add insert logic here

                repo.Alta(tipoinmueble);


                return RedirectToAction("Create", "Inmuebles");
            }
            catch(Exception e)
            {
                throw;
                
            }
        }

        // GET: Inmueble/Edit/5
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

        // POST: Inmueble/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, TipoInmueble tipoinmueble)
        {
            TipoInmueble i = null;
            try
            {
               // i = repo.ObtenerPorId(id);
                i.Id = tipoinmueble.Id;
                i.Descripcion = tipoinmueble.Descripcion;


             //   repo.Modificacion(i);
                
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
        [ValidateAntiForgeryToken]
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