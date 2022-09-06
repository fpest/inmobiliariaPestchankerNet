using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using inmobiliariaPestchanker.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace inmobiliariaPestchanker.Controllers
{
    public class PropietariosController : Controller
    {

        private RepositorioPropietario repo = new RepositorioPropietario();
        // GET: Propietarios
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

        // GET: Propietarios/Details/5
        public ActionResult Details(int id)
        {

                      try{
  var entidad = repo.ObtenerPorId(id);
            return View(entidad);

          }catch(Exception e){

            throw;
          }
           
        }

        // GET: Propietarios/Create
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
        [ValidateAntiForgeryToken]
        public ActionResult Create(Propietario propietario)
        {
            try
            {
                // TODO: Add insert logic here
                repo.Alta(propietario);


                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Propietarios/Edit/5
        public ActionResult Edit(int id)
        {

          try{

            var entidad = repo.ObtenerPorId(id);
            return View(entidad);

          }catch(Exception e){

            throw;
          }


        }

        // POST: Propietarios/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
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

        // GET: Propietarios/Delete/5
        public ActionResult Delete(int id)
        {
            var entidad = repo.ObtenerPorId(id);
            return View(entidad);
        }

        // POST: Propietarios/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Propietario propietario)
        {
            try
            {
                // TODO: Add delete logic here
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