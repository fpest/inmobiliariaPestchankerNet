using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using inmobiliariaPestchanker.Models;
using Microsoft.AspNetCore.Mvc;

namespace inmobiliariaPestchanker.Controllers
{


    public class ContratosController : Controller

    {


        private RepositorioContrato repo = new RepositorioContrato();
        private RepositorioInmueble repoInmueble = new RepositorioInmueble();
        private RepositorioInquilino repoInquilino = new RepositorioInquilino();
        private RepositorioPago repoPago = new RepositorioPago();


 [Authorize]
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
        public ActionResult Renovar()
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
        //[ValidateAntiForgeryToken]
        
        [Authorize]
        public ActionResult Renovar(Contrato contrato)
        {
            var contratoBase = repo.ObtenerPorId(contrato.Id);
            var alquilado = repo.inmuebleAlquilado(contrato.FechaInicio,contrato.FechaFin,contrato.IdInmueble);	
            
            DateTime hoy = DateTime.Today;
            TimeSpan interval = contrato.FechaInicio - contratoBase.FechaFin;
            
            TimeSpan inicioFin = contrato.FechaFin - contrato.FechaInicio;
            
            //(!alquilado && interval.Days>0 && inicioFin.Days > 0)
            if (!alquilado && inicioFin.Days > 0){
            
            try
            {

              

              repo.Alta(contrato);
              return RedirectToAction(nameof(Index));
            }
            catch(Exception e)
            {

               
                return View();

                          }
            }else{

              string msj = "No se puede registrar contrato. Este inmueble tiene contrato vigente en esas fechas.";  
              if (inicioFin.Days < 0) msj = "La fecha de inicio de contrato debe ser menor que la de fin de contrato."; 
            TempData["Mensaje"] = msj;
               ViewBag.Inmueble = repoInmueble.ObtenerTodos();
                ViewBag.Inquilino = repoInquilino.ObtenerTodos();
           
           return RedirectToAction(nameof(Index));
           
            return View();


            }

        }








        [Authorize]
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
        //[ValidateAntiForgeryToken]
        
        [Authorize]
        public ActionResult Create(Contrato contrato)
        {
           
            var alquilado = repo.inmuebleAlquilado(contrato.FechaInicio,contrato.FechaFin,contrato.IdInmueble);	
            
            DateTime hoy = DateTime.Today;
           
            
            TimeSpan inicioFin = contrato.FechaFin - contrato.FechaInicio;
            
            //(!alquilado && interval.Days>0 && inicioFin.Days > 0)
            if (!alquilado && inicioFin.Days > 0){
            
            try
            {

              

              repo.Alta(contrato);
              return RedirectToAction(nameof(Index));
            }
            catch(Exception e)
            {

               
                return View();

                          }
            }else{
            
              string msj = "No se puede registrar contrato. Este inmueble tiene contrato vigente en esas fechas.";  
              if (inicioFin.Days < 0) msj = "La fecha de inicio de contrato debe ser menor que la de fin de contrato."; 
            TempData["Mensaje"] = msj;
               ViewBag.Inmueble = repoInmueble.ObtenerTodos();
                ViewBag.Inquilino = repoInquilino.ObtenerTodos();
           
           return RedirectToAction(nameof(Index));
           
           //return RedirectToAction(nameof(Create));
           
           // return View();


            }



        }

         [Authorize]
        public ActionResult Terminar(int id)
        {
          var i = repo.ObtenerPorId(id);
          DateTime hoy = DateTime.Today;
          TimeSpan interval = hoy - i.FechaInicio;
          Decimal mesesAlquilados = (Decimal) interval.Days/30;
          //Cantidad de meses a pagar
          int mesesAlquiladosEnteros = (int)Math.Ceiling(mesesAlquilados);
          Decimal montoAbonadoContrato = (Decimal) repoPago.ObtenerMontoPagadoPorContrato(id);
          //Saldo adeudado
          Decimal saldoAdeudado = (mesesAlquiladosEnteros* i.Precio) - montoAbonadoContrato;
          //Calculo de Mora
          TimeSpan diasTotalContrato = i.FechaFin - i.FechaInicio;
          Decimal mesesTotalContrato = (Decimal) diasTotalContrato.Days/31;
          int mesesTotalContratoEntero = (int)Math.Ceiling(mesesTotalContrato);
          Decimal mora;
          if (mesesAlquilados < mesesTotalContrato/2){
            mora = i.Precio*2;
          }else{mora = i.Precio;}
          

        try{
              var entidad = repo.ObtenerPorId(id);
              ViewBag.Inmueble = repoInmueble.ObtenerTodos();
              ViewBag.Inquilino = repoInquilino.ObtenerTodos();
              ViewBag.SaldoDeuda = saldoAdeudado;
              ViewBag.Mora = mora;
              if(TempData.ContainsKey("Mensaje"))
              ViewBag.Mensaje = TempData["Mensaje"];
              if(TempData.ContainsKey("Error"))
              ViewBag.Mensaje = TempData["Error"];
            return View(entidad);
          }catch(Exception e){
           throw;
          }
        }


      
        //[ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult TerminarDefinitivo(int id)
        {
        
          Contrato i = repo.ObtenerPorId(id);
          DateTime hoy = DateTime.Today;
          TimeSpan interval = hoy - i.FechaInicio;
          Decimal mesesAlquilados = (Decimal) interval.Days/30;
          //Cantidad de meses a pagar
          int mesesAlquiladosEnteros = (int)Math.Ceiling(mesesAlquilados);
          Decimal montoAbonadoContrato = (Decimal) repoPago.ObtenerMontoPagadoPorContrato(id);
          //Saldo adeudado
          Decimal saldoAdeudado = (mesesAlquiladosEnteros* i.Precio) - montoAbonadoContrato;
          //Calculo de Mora
          TimeSpan diasTotalContrato = i.FechaFin - i.FechaInicio;
          Decimal mesesTotalContrato = (Decimal) diasTotalContrato.Days/31;
          int mesesTotalContratoEntero = (int)Math.Ceiling(mesesTotalContrato);
          Decimal mora;
          if (mesesAlquilados < mesesTotalContrato/2){
            mora = i.Precio*2;
          }else{mora = i.Precio;}
          



        if (saldoAdeudado<=0){
           

            try
            {
                i = repo.ObtenerPorId(id);

                i.FechaFin = hoy;
                repo.Modificacion(i);
                
                return RedirectToAction(nameof(Index));
            }
            catch(Exception e)
            {
                throw;
                
                
            }
        }else{

                TempData["Mensaje"] = "No se puede terminar contrato si tiene deuda";
                    return RedirectToAction(nameof(Index));
                //return RedirectToAction(nameof(Index));

        }

        }











        [Authorize]
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
        //[ValidateAntiForgeryToken]
        
        [Authorize]
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

        [Authorize(Policy = "Administrador")]      
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
        //[ValidateAntiForgeryToken]

        [Authorize(Policy = "Administrador")]
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