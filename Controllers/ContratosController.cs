using System;
using System.Collections.Generic;
using System.Linq;
using inmobiliariaPestchanker.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace inmobiliariaPestchanker.Controllers
{


    public class ContratosController : Controller

    {


        private RepositorioContrato repo = new RepositorioContrato();
         private RepositorioInmueble repoInmueble = new RepositorioInmueble();
          private RepositorioInquilino repoInquilino = new RepositorioInquilino();
        public IActionResult Index()
        {
             try{
                var lista = repo.ObtenerTodos();
                 ViewBag.Mensaje = TempData["Mensaje"];

                return View(lista);
            }
            catch(Exception e)
            {
                throw;
            }
        }

        
    
        // GET: Inquilinos/Details/5
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

        // GET: Inquilinos/Create
        public ActionResult Create()
        {
          try{
                ViewBag.Inmueble = repoInmueble.ObtenerTodos();
                ViewBag.Inquilino = repoInquilino.ObtenerTodos();
               
                if(TempData.ContainsKey("Mensaje"))
                ViewBag.Mensaje = TempData["Mensaje"];
                if(TempData.ContainsKey("Error"))
                ViewBag.Mensaje = TempData["Error"];
           
            return View();

          }catch(Exception e){

            throw;
          }
           
        }

        // POST: Inquilinos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Contrato contrato)
        {
            try
            {
                // TODO: Add insert logic here

                repo.Alta(contrato);


                return RedirectToAction(nameof(Index));
            }
            catch(Exception e)
            {
                return View();
            }
        }

        // GET: Inquilinos/Edit/5
        public ActionResult Edit(int id)
        {




                      try{
               var entidad = repo.ObtenerPorId(id);
              ViewBag.Inmueble = repoInmueble.ObtenerTodos();
               ViewBag.Inquilino = repoInquilino.ObtenerTodos();
               
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
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Contrato contrato)
        {
            Contrato i = null;
            try
            {
                i = repo.ObtenerPorId(id);
                i.IdInmueble = contrato.IdInmueble;
                i.IdInquilino = contrato.IdInquilino;
                i.FechaInicio = contrato.FechaInicio;
                i.FechaFin = contrato.FechaFin;
                i.Precio = contrato.Precio;
                repo.Modificacion(i);
                
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
        
        // GET: Inquilinos/Delete/5
        public ActionResult Delete(int id)
        {

          try{
  var entidad = repo.ObtenerPorId(id);
             ViewBag.Inmueble = repoInmueble.ObtenerPorId(entidad.IdInmueble);
             ViewBag.Inquilino = repoInquilino.ObtenerPorId(entidad.IdInquilino);
                
               if(TempData.ContainsKey("Mensaje"))
               ViewBag.Mensaje = TempData["Mensaje"];
               if(TempData.ContainsKey("Error"))
               ViewBag.Mensaje = TempData["Error"];
            return View(entidad);
        

          }catch(Exception e){

            throw;
          }



        }

        // POST: Inquilinos/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Contrato contrato)
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


                return View();
            }
        }
 

    }


}